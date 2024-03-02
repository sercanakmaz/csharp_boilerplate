using Boilerplate.Infrastructure.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Api.Application;

public static class UseCaseModule
{
    public static IServiceCollection AddUseCasesModule(this IServiceCollection services)
    {
        services.AddScoped<EventContext>();

        services.AddScoped<CreateOrderUseCaseHandler>();
        services.AddScoped<CancelOrderLineUseCaseHandler>();

        return services;
    }
}