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
}

public class OrderService: IOrderService
{
    private readonly IOrderRepository _OrderRepository;

    public OrderService(IOrderRepository OrderRepository)
    {
        _OrderRepository = OrderRepository;
    }

    public async Task<Order> GetById(string id)
    {
        return await _OrderRepository.FindById(id.ToObjectId());
    }
    
    public async Task<List<Order>> GetByUserId(string userId)
    {
        var orders = await _OrderRepository.GetByUserId(userId);

        return orders;
    }

    public async Task<Order> Create(string orderNumber, string userId)
    {
        var order = new Order(orderNumber, userId);
        
        await _OrderRepository.InsertAsync(order);
        
        return order;
    }
}