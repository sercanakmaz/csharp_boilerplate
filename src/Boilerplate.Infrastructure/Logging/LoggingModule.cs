using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Infrastructure.Logging;

public static class LoggingModule
{
    public static ILoggingBuilder AddLoggingModule(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
    {
        loggingBuilder.Services.Configure<LoggerSettings>(
            configuration.GetSection(nameof(LoggerSettings)));

        loggingBuilder.Services.AddSingleton<ILoggerProvider, MicrosoftLoggingProvider>();
        loggingBuilder.Services.AddSingleton<ILogger, Logger>();
        
        return loggingBuilder;
    }
}