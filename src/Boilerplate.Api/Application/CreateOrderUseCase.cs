using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boilerplate.Domain.Aggregates.OrderLines;
using Boilerplate.Domain.Aggregates.Orders;
using Boilerplate.Infrastructure.Domain;

namespace Boilerplate.Api.Application;

public class CreateOrderUseCase : BaseUseCase
{
    public string OrderNumber { get; set; }
    public string UserId { get; set; }

    public List<OrderLine> OrderLines { get; set; }

    public class OrderLine
    {
        public string Sku { get; set; }
        public decimal Price { get; set; }
    }
}

public class CreateOrderUseCaseHandler(
    IOrderService orderService,
    IOrderLineService orderLineService,
    IServiceProvider serviceProvider,
    EventContext eventContext) : BaseUseCaseHandler<CreateOrderUseCase, Order>(serviceProvider, eventContext)
{
    protected override async Task<UseCaseResult<Order>> OnHandle(CreateOrderUseCase useCase)
    {
        var createOrderResult = await orderService.Create(useCase.OrderNumber, useCase.UserId);
        
        foreach (var orderLine in useCase.OrderLines)
        {
            await orderLineService.Create(orderLine.Sku, orderLine.Price, useCase.OrderNumber);
        }

        var useCaseResult = UseCaseResult.FromContent(createOrderResult);

        return useCaseResult;
    }
}