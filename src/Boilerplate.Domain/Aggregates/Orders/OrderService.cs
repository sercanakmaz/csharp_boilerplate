using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Boilerplate.Domain.Shared;
using Boilerplate.Infrastructure.Persistence.Mongo;

namespace  Boilerplate.Domain.Aggregates.Orders;

public interface IOrderService
{
    Task<List<Order>> GetByUserId(string userId);
    Task<Order> Create(string orderNumber, string userId);
    Task<Order> GetById(string id);
    Task<Order> Complete(string orderNumber, string completeReason);
}

public class OrderService(IOrderRepository orderRepository) : IOrderService
{
    public async Task<Order> GetById(string id)
    {
        return await orderRepository.FindById(id.ToObjectId());
    }
    
    public async Task<List<Order>> GetByUserId(string userId)
    {
        var orders = await orderRepository.GetByUserId(userId);

        return orders;
    }

    public async Task<Order> Create(string orderNumber, string userId)
    {
        var order = new Order(orderNumber, userId);
        
        await orderRepository.InsertAsync(order);
        
        return order;
    }
    

    public async Task<Order> Complete(string orderNumber, string completeReason)
    {
        var order = await orderRepository.GetByOrderNumber(orderNumber);

        order.Complete(OrderCompleteReasons.Cancelled);

        await orderRepository.ReplaceOneAsync(order);
        
        return order;
    }
}