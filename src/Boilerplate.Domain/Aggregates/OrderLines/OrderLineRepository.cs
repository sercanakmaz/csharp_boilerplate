using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boilerplate.Infrastructure.Domain;
using Boilerplate.Infrastructure.Persistence.Mongo;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace  Boilerplate.Domain.Aggregates.OrderLines;

public interface IOrderLineRepository : IRepository<OrderLine>
{
}

public class OrderLineRepository(
    EventContext eventContext,
    IMongoClient client,
    IOptions<MongoDbSettings> mongoDbSettings,
    string databaseName = null)
    : Repository<OrderLine>(eventContext, client, mongoDbSettings, databaseName), IOrderLineRepository
{
    public override async Task CreateIndexes()
    {
        await this.CreateIndex(true, p => p.Sku);
    }
}