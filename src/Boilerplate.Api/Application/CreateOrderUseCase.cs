using System;
using System.Threading.Tasks;
using Boilerplate.Domain.Aggregates.Orders;
using Boilerplate.Infrastructure.Domain;

namespace Boilerplate.Api.Application;

public class CreateOrderUseCase : BaseUseCase
{
    public string OrderNumber { get; set; }
    public string UserId { get; set; }
}

public class CreateOrderUseCaseHandler(
    IOrderService orderService,
    IServiceProvider serviceProvider,
    EventContext eventContext) : BaseUseCaseHandler<CreateOrderUseCase, Order>(serviceProvider, eventContext)
{
    protected override async Task<UseCaseResult<Order>> OnHandle(CreateOrderUseCase useCase)
    {
        var createOrderResult = await orderService.Create(useCase.OrderNumber, useCase.UserId);
        var useCaseResult = UseCaseResult.FromContent(createOrderResult);

        return useCaseResult;
    }
}