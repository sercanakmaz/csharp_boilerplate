using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Boilerplate.Infrastructure.Logging;

public class Logger : ILogger
{
    private readonly IOptions<LoggerSettings> _consoleLoggerSettingOptions;

    public Logger(IOptions<LoggerSettings> consoleLoggerSettingOptions)
    {
        _consoleLoggerSettingOptions = consoleLoggerSettingOptions;
    }

    public void Log(LogLevel logLevel, string message, Exception exception = null, string responseBody = null,
        string requestBody = null, HttpMethod httpMethod = null, HttpStatusCode? httpStatusCode = null,
        long? duration = null, string hostName = null, string url = null)
    {
        if (_consoleLoggerSettingOptions.Value.MinimumLogLevel > logLevel)
        {
            return;
        }

        Console.WriteLine(JsonConvert.SerializeObject(new
        {
            CorrelationId = Trace.CorrelationManager.ActivityId,
            DateTime = DateTime.Now,
            LogLevel = logLevel.ToString(),
            Message = message,
            Exception = exception?.ToString(),
            ResponseBody = responseBody,
            RequestBody = requestBody,
            HttpMethod = httpMethod?.Method,
            HttpStatusCode = (int?)httpStatusCode,
            Duration = duration,
            HostName = hostName,
            Url = url
        }));
    }

    public void Log(LogLevel logLevel, Exception exception = null, string responseBody = null, string requestBody = null,
        HttpMethod httpMethod = null, HttpStatusCode? httpStatusCode = null, long? duration = null, string hostName = null,
        string url = null)
    {
        throw new NotImplementedException();
    }

    public void LogTrace(string message, Exception exception = null, string responseBody = null,
        string requestBody = null, HttpMethod httpMethod = null, HttpStatusCode? httpStatusCode = null,
        long? duration = null,
        string hostName = null, string url = null)
    {
        Log(LogLevel.Trace, message, exception, responseBody, requestBody, httpMethod,
            httpStatusCode.GetValueOrDefault(), duration, hostName, url);
    }

    public void LogDebug(string message, Exception exception = null, string responseBody = null,
        string requestBody = null, HttpMethod httpMethod = null, HttpStatusCode? httpStatusCode = null,
        long? duration = null, string hostName = null, string url = null)
    {
        Log(LogLevel.Debug, message, exception, responseBody, requestBody, httpMethod,
            httpStatusCode.GetValueOrDefault(), duration, hostName, url);
    }

    public void LogInformation(string message, Exception exception = null, string responseBody = null,
        string requestBody = null, HttpMethod httpMethod = null, HttpStatusCode? httpStatusCode = null,
        long? duration = null, string hostName = null, string url = null)
    {
        Log(LogLevel.Information, message, exception, responseBody, requestBody, httpMethod,
            httpStatusCode.GetValueOrDefault(), duration, hostName, url);
    }

    public void LogWarning(string message, Exception exception = null, string responseBody = null,
        string requestBody = null, HttpMethod httpMethod = null, HttpStatusCode? httpStatusCode = null,
        long? duration = null, string hostName = null, string url = null)
    {
        Log(LogLevel.Warning, message, exception, responseBody, requestBody, httpMethod,
            httpStatusCode.GetValueOrDefault(), duration, hostName, url);
    }

    public void LogError(string message, Exception exception = null, string responseBody = null,
        string requestBody = null, HttpMethod httpMethod = null, HttpStatusCode? httpStatusCode = null,
        long? duration = null, string hostName = null, string url = null)
    {
        Log(LogLevel.Error, message, exception, responseBody, requestBody, httpMethod,
            httpStatusCode.GetValueOrDefault(), duration, hostName, url);
    }

    public void LogCritical(string message, Exception exception = null, string responseBody = null,
        string requestBody = null, HttpMethod httpMethod = null, HttpStatusCode? httpStatusCode = null,
        long? duration = null, string hostName = null, string url = null)
    {
        Log(LogLevel.Critical, message, exception, responseBody, requestBody, httpMethod,
            httpStatusCode.GetValueOrDefault(), duration, hostName, url);
    }
}