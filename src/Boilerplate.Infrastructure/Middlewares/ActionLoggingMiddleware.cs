using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Boilerplate.Infrastructure.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Boilerplate.Infrastructure.Middlewares;

    public class ActionLoggingMiddleware : IMiddleware
    {
        private readonly ILogger consoleLogger;

        public ActionLoggingMiddleware(ILogger consoleLogger)
        {
            this.consoleLogger = consoleLogger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var method = context.Request.Method;

            if (method.ToLowerInvariant() == "get")
            {
                await next(context);
                return;
            }
            
            
            var requestUrl = context.Request.GetDisplayUrl();
            string request = string.Empty;
            string response = string.Empty;

            if (IsApplyLogging(method))
            {
                using (var bodyReader = new StreamReader(context.Request.Body))
                {
                    request = await bodyReader.ReadToEndAsync();
                    context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(request));
                }
            }
            
            
            consoleLogger.LogInformation("Api OnActionExecuting", null, response, request,
               new HttpMethod(method), HttpStatusCode.Processing, null, context.Request.Host.Value);

            var stopWatch = new Stopwatch();
            stopWatch.Start();


            if (IsApplyLogging(method))
            {
                using (var buffer = new MemoryStream())
                {
                    var stream = context.Response.Body;
                    context.Response.Body = buffer;

                    await next(context);
                    stopWatch.Stop();

                    buffer.Seek(0, SeekOrigin.Begin);
                    using (var bufferReader = new StreamReader(buffer))
                    {
                        response = await bufferReader.ReadToEndAsync();
                        buffer.Seek(0, SeekOrigin.Begin);
                        await buffer.CopyToAsync(stream);
                    }
                }
            }
            else
            {
                await next(context);
                stopWatch.Stop();
            }

            var excetionTime = stopWatch.ElapsedMilliseconds;

            consoleLogger.LogInformation("Api OnActionExecuted", null, response, request,
                new HttpMethod(method), (HttpStatusCode) context.Response.StatusCode, excetionTime,
                context.Request.Host.Value, requestUrl);
        }

        private bool IsApplyLogging(string method)
        {
            if (method == "PUT" || method == "POST")
            {
                return true;
            }

            return false;
        }
    }