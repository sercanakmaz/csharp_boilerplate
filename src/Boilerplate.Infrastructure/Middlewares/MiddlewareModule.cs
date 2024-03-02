using Boilerplate.Infrastructure.Persistence.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Boilerplate.Infrastructure.Middlewares;

public static class MiddlewareModule
{
    public static IServiceCollection AddMiddlewareModule(this IServiceCollection services)
    {
        services.AddTransient<CorrelationIdMiddleware>();
        services.AddTransient<ActionLoggingMiddleware>();
        
        return services;
    }
    
    public static IApplicationBuilder UseMiddlewareModule(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<CorrelationIdMiddleware>();
        builder.UseMiddleware<ActionLoggingMiddleware>();
        
        return builder;
    }
}