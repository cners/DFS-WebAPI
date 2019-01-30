using DFS.DataEFCore.Repositories;
using DFS.Domain.Repositories;
using DFS.Domain.Supervisor;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DFS.API.Configurations
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IDeveloperRepository, DeveloperRepository>();

            return services;
        }

        public static IServiceCollection ConfigureSupervisor(this IServiceCollection services)
        {
            services.AddScoped<IDfsSupervisor, DfsSupervisor>();

            return services;
        }

        public static IServiceCollection AddMiddleware(this IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            return services;
        }

        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", new CorsPolicyBuilder()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowCredentials()
                    .Build());
            });
    }
}
