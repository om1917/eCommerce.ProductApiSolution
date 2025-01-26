
using eCommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Infrastructure.Data;

namespace ProductApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfractureService(this IServiceCollection services, IConfiguration config) 
        {
            //Add DB Connectivity
            //Add Authentication Scheme

            SharedServiceContainer.AddSharedServices<ProductDbContext>(services, config, config["MySerilog: FileName"]!);

            //Create Dependency Injection
            services.AddScoped<ProductDbContext, ProductDbContext>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            //Register Middleware Such as:
            //Global Exception: hadle internal errors
            //Listen to only api gateway: block all outside calls
            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
