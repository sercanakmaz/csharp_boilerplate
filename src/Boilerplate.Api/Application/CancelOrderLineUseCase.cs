using System;
using System.Threading.Tasks;
using Boilerplate.Domain.Aggregates.OrderLines;
using Boilerplate.Infrastructure.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Api.Application;

public class CancelOrderLineUseCase : BaseUseCase
{
    public string Id { get; set; }
    public string CancellationReason { get; set; }
}

public class CancelOrderLineUseCaseHandler(
    IServiceProvider serviceProvider,
    EventContext eventContext,
    IOrderLineService orderLineService)
    : BaseUseCaseHandler<CancelOrderLineUseCase, bool>(serviceProvider, eventContext)
{
    protected override async Task<UseCaseResult<bool>> OnHandle(CancelOrderLineUseCase useCase)
    {
        await orderLineService.Cancel(useCase.Id, useCase.CancellationReason);
        
        return UseCaseResult.FromContent(true);
    }
}