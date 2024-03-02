using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Boilerplate.Domain.Aggregates.Orders;
using Boilerplate.Domain.Shared;
using Boilerplate.Infrastructure.Persistence.Mongo;
using MongoDB.Bson;

namespace  Boilerplate.Domain.Aggregates.OrderLines;

public interface IOrderLineService
{
    Task<OrderLine> Create(string sku, decimal price, string orderNumber);
    Task Cancel(string id, string reason);
}

public class OrderLineService(IOrderLineRepository orderLineRepository) : IOrderLineService
{
    public async Task<OrderLine> Create(string sku, decimal price, string orderNumber)
    {
        var order = new OrderLine(sku, price, orderNumber);
        
        await orderLineRepository.InsertAsync(order);
        
        return order;
    }

    public async Task Cancel(string id, string reason)
    {
        var orderLine = await orderLineRepository.FindById(ObjectId.Parse(id));

        orderLine.Cancel(reason);

        await orderLineRepository.ReplaceOneAsync(orderLine);
    }
}