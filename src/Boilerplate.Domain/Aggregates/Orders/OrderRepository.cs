using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boilerplate.Infrastructure.Domain;
using Boilerplate.Infrastructure.Persistence.Mongo;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace  Boilerplate.Domain.Aggregates.Orders;

public interface IOrderRepository : IRepository<Order>
{
    Task<List<Order>> GetByUserId(string userId);
}

public class OrderRepository(
    EventContext eventContext,
    IMongoClient client,
    IOptions<MongoDbSettings> mongoDbSettings,
    string databaseName = null)
    : Repository<Order>(eventContext, client, mongoDbSettings, databaseName), IOrderRepository
{
    public override async Task CreateIndexes()
    {
        await this.CreateIndex(false, p => p.UserId);
    }

    public Task<List<Order>> GetByUserId(string userId)
    {
        return base.FindAsync(p => p.UserId == userId);
    }
}