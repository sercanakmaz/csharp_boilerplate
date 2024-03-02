using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Boilerplate.Infrastructure.Domain;
using Humanizer;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Boilerplate.Infrastructure.Persistence.Mongo
{
    public interface IRepository<T> : IRepository where T : Entity
    {
        Task<T> FindById(ObjectId id);

        Task InsertAsync(T entity);

        Task<T> DeleteHardAsync(ObjectId id);

        Task ReplaceOneAsync(T entity);
    }

    public abstract class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly EventContext _eventContext;

        protected Repository(EventContext eventContext, IMongoClient client, IOptions<MongoDbSettings> mongoDbSettings,
            string databaseName = null)
        {
            _eventContext = eventContext;
            var database = client.GetDatabase(databaseName ?? mongoDbSettings.Value.DatabaseName);

            Collection = database.GetCollection<T>(typeof(T).Name.Pluralize());
        }

        private IMongoCollection<T> Collection { get; set; }
        protected SortDefinitionBuilder<T> SortDefinitionBuilder => Builders<T>.Sort;

        public async Task InsertAsync(T entity)
        {
            try
            {
                await this.Collection.InsertOneAsync(entity);
            }
            catch (MongoWriteException mongoWriteException)
            {
                var writeErrorCode = mongoWriteException.WriteError.Code;

                if (writeErrorCode == 11000)
                {
                    throw new IdempotencyException(mongoWriteException.Message, mongoWriteException);
                }

                throw;
            }
        }

        protected async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter,
            ProjectionDefinition<T> projectionDefinition)
        {
            return await this.Collection.Find(filter).Project<T>(projectionDefinition).ToListAsync();
        }


        protected async Task<T> FindOneAsync(Expression<Func<T, bool>> filter,
            ProjectionDefinition<T> projectionDefinition)
        {
            return await this.Collection.Find(filter).Project<T>(projectionDefinition).FirstOrDefaultAsync();
        }


        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> filter)
        {
            return await this.Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter, int skip, int limit,
            SortDefinition<T> sortDefinitionBuilder)
        {
            if (sortDefinitionBuilder == null)
            {
                return await this.Collection.Find(filter).Skip(skip).Limit(limit).ToListAsync();
            }

            return await this.Collection.Find(filter).Sort(sortDefinitionBuilder).Skip(skip).Limit(limit).ToListAsync();
        }

        protected async Task<IEnumerable<T>> FindAsync(FilterDefinition<T> filterDefinition, int skip, int limit,
            SortDefinition<T> sortDefinitionBuilder = null)
        {
            if (sortDefinitionBuilder == null)
            {
                return await this.Collection.Find(filterDefinition).Skip(skip).Limit(limit).ToListAsync();
            }

            return await this.Collection.Find(filterDefinition).Sort(sortDefinitionBuilder).Skip(skip).Limit(limit)
                .ToListAsync();
        }

        protected async Task<long> CountAsync(FilterDefinition<T> filterDefinition)
        {
            return await this.Collection.CountDocumentsAsync(filterDefinition);
        }

        public async Task<T> FindOneAsync(Expression<Func<T, bool>> filter)
        {
            return await this.Collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> filter)
        {
            return await this.Collection.CountDocumentsAsync(filter);
        }

        public async Task<T> DeleteHardAsync(ObjectId id)
        {
            var result = await this.Collection.FindOneAndDeleteAsync(d => d.Id == id);
            return result;
        }

        public async Task ReplaceOneAsync(T entity)
        {
            try
            {
                await Collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
            }
            catch (MongoWriteException mongoWriteException)
            {
                var writeErrorCode = mongoWriteException.WriteError.Code;

                if (writeErrorCode == 11000)
                {
                    throw new IdempotencyException(mongoWriteException.Message, mongoWriteException);
                }

                throw;
            }
        }

        public abstract Task CreateIndexes();

        public async Task<T> FindById(ObjectId id)
        {
            return await FindOneAsync(p => p.Id == id);
        }

        public async Task Cursor(ObjectId? minimumId, ObjectId? maximumId,
            DateTime? minimumUpdatedDate, DateTime? maximumUpdatedDate,
            bool isDescending, bool isByUpdatedDate, int batchSize,
            Func<T, Task> action, CancellationToken cancellationToken = default(CancellationToken))
        {
            var filters = new List<FilterDefinition<T>>();

            if (isByUpdatedDate)
            {
                if (minimumUpdatedDate != null)
                {
                    filters.Add(Builders<T>.Filter.Gte(t => t.UpdatedDate, minimumUpdatedDate));
                }

                if (maximumUpdatedDate != null)
                {
                    filters.Add(Builders<T>.Filter.Lte(t => t.UpdatedDate, maximumUpdatedDate));
                }
            }
            else
            {
                if (minimumId != null)
                {
                    filters.Add(Builders<T>.Filter.Gte(t => t.Id, minimumId));
                }

                if (maximumId != null)
                {
                    filters.Add(Builders<T>.Filter.Lte(t => t.Id, maximumId));
                }
            }

            var orderFilterDefinition =
                filters.Any() ? Builders<T>.Filter.And(filters.ToArray()) : Builders<T>.Filter.Empty;

            var orderFindOptions = new FindOptions<T>
            {
                BatchSize = batchSize,
                Sort =
                    isByUpdatedDate
                        ? (
                            isDescending
                                ? new SortDefinitionBuilder<T>().Descending(t => t.UpdatedDate)
                                : new SortDefinitionBuilder<T>().Ascending(t => t.UpdatedDate)
                        )
                        : (
                            isDescending
                                ? new SortDefinitionBuilder<T>().Descending(t => t.Id)
                                : new SortDefinitionBuilder<T>().Ascending(t => t.Id)
                        )
            };
            using (var cursor = await this.Collection.FindAsync(orderFilterDefinition, orderFindOptions,
                       cancellationToken: cancellationToken))
            {
                while (await cursor.MoveNextAsync(cancellationToken: cancellationToken))
                {
                    foreach (var entity in cursor.Current)
                    {
                        await action(entity);
                    }
                }
            }
        }

        protected async Task CreateIndex(bool isUnique, params Expression<Func<T, object>>[] expressions)
        {
            var indexKeysDefinition = Builders<T>
                .IndexKeys
                .Combine(
                    expressions.Select(
                            expression =>
                                Builders<T>
                                    .IndexKeys
                                    .Ascending(expression))
                        .ToArray());

            var createIndexOptions = new CreateIndexOptions { Unique = isUnique, Background = true };

            await this.Collection.Indexes.CreateOneAsync(new CreateIndexModel<T>(indexKeysDefinition,
                createIndexOptions));
        }

        private void DispatchDomainEvents(Entity entity)
        {
            var uncommitedEvents = entity.GetUncommittedEvents();

            foreach (var uncommitedEvent in uncommitedEvents)
            {
                _eventContext.AddRaised(uncommitedEvent);
            }
        }
    }
}