using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Boilerplate.Infrastructure.Persistence.Mongo;

public static class MongoModule
{
    public static IServiceCollection AddMongoModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(
            configuration.GetSection(nameof(MongoDbSettings)));
     
        services.AddSingleton<IMongoClient>(provider =>
        {
            var mongoDbSettings = provider.GetRequiredService<IOptions<MongoDbSettings>>();
            
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);

            return mongoClient;
        });
        
        return services;
    }
}