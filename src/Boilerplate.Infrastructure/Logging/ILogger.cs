using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Infrastructure.Logging;

public interface ILogger
{
    void Log(LogLevel logLevel, string message, Exception exception = null, string responseBody = null,
        string requestBody = null, HttpMethod httpMethod = null, HttpStatusCode? httpStatusCode = null,
        long? duration = null, string hostName = null, string url = null);

    void LogTrace(string message, Exception exception = null, string responseBody = null,
        string requestBody = null, HttpMethod httpMethod = null, HttpStatusCode? httpStatusCode = null,
        long? duration = null, string hostName = null, string url = null);

    void LogDebug(string message, Exception exception = null, string responseBody = null, string requestBody = null,
        HttpMethod httpMethod = null, HttpStatusCode? httpStatusCode = null, long? duration = null,
        string hostName = null, string url = null);

    void LogInformation(string message, Exception exception = null, string responseBody = null,
        string requestBody = null, HttpMethod httpMethod = null, HttpStatusCode? httpStatusCode = null,
        long? duration = null, string hostName = null, string url = null);

    void LogWarning(string message, Exception exception = null, string responseBody = null,
        string requestBody = null, HttpMethod httpMethod = null, HttpStatusCode? httpStatusCode = null,
        long? duration = null, string hostName = null, string url = null);

    void LogError(string message, Exception exception = null, string responseBody = null, string requestBody = null,
        HttpMethod httpMethod = null, HttpStatusCode? httpStatusCode = null, long? duration = null,
        string hostName = null, string url = null);

    void LogCritical(string message, Exception exception = null, string responseBody = null,
        string requestBody = null, HttpMethod httpMethod = null, HttpStatusCode? httpStatusCode = null,
        long? duration = null, string hostName = null, string url = null);
}