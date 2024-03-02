using System;
using System.Transactions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Boilerplate.Infrastructure.Logging;

public class MicrosoftLoggingProvider : ILoggerProvider
{
    private readonly ILogger _logger;

    public MicrosoftLoggingProvider(ILogger logger)
    {
        _logger = logger;
    }

    public void Dispose()
    {
    }

    public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
    {
        return new MicrosoftLogger(_logger);
    }
}

public class MicrosoftLogger : Microsoft.Extensions.Logging.ILogger
{
    private readonly ILogger _logger;

    public MicrosoftLogger(ILogger logger)
    {
        _logger = logger;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
        Func<TState, Exception, string> formatter)
    {
        var formattedMessage = formatter(state, exception);
        
        _logger.Log(logLevel, message: $"EventId: {eventId.ToString()} Message: {formattedMessage}", exception: exception);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }
}