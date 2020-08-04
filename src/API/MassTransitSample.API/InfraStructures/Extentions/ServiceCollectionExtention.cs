using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransitSample.Contracts;
using NSwag.Generation.Processors.Security;
using NSwag;

namespace MassTransitSample.API.InfraStructures.Extentions
{
    public static class ServiceCollectionExtention
    {
        public static void ConfigMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq://localhost");                    
                });

                x.AddRequestClient<OrderRegistered>();
            });

            services.AddMassTransitHostedService();
        }

        public static void ConfigSwagger(this IServiceCollection services)
        {
            services.AddOpenApiDocument(option =>
            {
                option.Title = "MassTransit Sample API";
                option.Version = "1";

                option.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT"));
                option.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: {your GUID token}."
                }));
            });
        }
    }
}
