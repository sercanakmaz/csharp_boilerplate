using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Boilerplate.Domain.Aggregates.Orders;
using Boilerplate.Infrastructure.Domain;
using Boilerplate.Infrastructure.Exceptions;
using Boilerplate.Infrastructure.Logging;
using Boilerplate.Infrastructure.Middlewares;
using Boilerplate.Infrastructure.Persistence;
using Boilerplate.Infrastructure.Persistence.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ILogger = Boilerplate.Infrastructure.Logging.ILogger;

namespace Boilerplate.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new ObjectIdConverter()); });
            ;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Boilerplate.Api", Version = "v1" });
            });
            services.AddOptions();

            services.AddMiddlewareModule()
                .AddMongoModule(Configuration)
                .AddOrdersModule();

            services.AddScoped<EventContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
                using var scope = app.ApplicationServices.CreateScope();
                var repositories = scope.ServiceProvider.GetServices<IRepository>().ToList();
                foreach (var repository in repositories)
                {
                    repository.CreateIndexes().GetAwaiter().GetResult();
                }
            }

            app.UseMiddlewareModule();

            app.UseExceptionHandler(builder =>
            {
                builder.Run(async handler =>
                {
                    var ehf = handler.Features.Get<IExceptionHandlerFeature>();

                    if (ehf?.Error != null)
                    {
                        HttpStatusCode responseStatus;
                        Object responseObject; 

                        if (ehf.Error is DuplicateEntityException duplicateEntityException)
                        {
                            responseObject = new
                            {
                                ExistingEntity = duplicateEntityException.ExistingEntity,
                                ErrorMessage = ehf.Error.Message,
                                ExceptionCode = "DuplicateEntityException"
                            };
                            responseStatus = duplicateEntityException.HttpStatusCode;
                        }
                        else
                        {
                            var exceptionBase = ehf.Error as ExceptionBase;
                                
                            responseObject = new
                            {
                                ErrorMessage = ehf.Error.Message,
                                ExceptionCode = exceptionBase?.Code ?? ehf.Error.GetType().Name
                            };
                            responseStatus = exceptionBase?.HttpStatusCode ?? HttpStatusCode.InternalServerError;
                        }

                        logger.LogError("ExceptionHandlerMiddleware", ehf.Error);

                        handler.Response.Clear();

                        handler.Response.StatusCode = (int)responseStatus;
                        handler.Response.ContentType = @"application/json; charset=utf-8";

                        var error = Newtonsoft.Json.JsonConvert.SerializeObject(responseObject);

                        await handler.Response.WriteAsync(error);
                    }
                });
            });
            
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Boilerplate.Api v1"));
        }
    }
}