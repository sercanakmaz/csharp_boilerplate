using Boilerplate.Domain.Events.Orders;
using Boilerplate.Infrastructure.Domain;
using Boilerplate.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace  Boilerplate.Domain.Aggregates.Orders;

public static class OrderModule
{
    public static IServiceCollection AddOrdersModule(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IRepository, OrderRepository>();
        services.AddSingleton<BaseEventHandler<Created>, OrderLinesCancelledEventHandler>();

        return services;
    }
}