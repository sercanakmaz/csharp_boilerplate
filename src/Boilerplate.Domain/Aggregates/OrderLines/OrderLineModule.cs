using Boilerplate.Domain.Aggregates.Orders;
using Boilerplate.Infrastructure.Domain;
using Boilerplate.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace  Boilerplate.Domain.Aggregates.OrderLines;

public static class OrderLineModule
{
    public static IServiceCollection AddOrderLinesModule(this IServiceCollection services)
    {
        services.AddScoped<IOrderLineService, OrderLineService>();
        services.AddScoped<IOrderLineRepository, OrderLineRepository>();
        services.AddScoped<IRepository, OrderLineRepository>();

        return services;
    }
}