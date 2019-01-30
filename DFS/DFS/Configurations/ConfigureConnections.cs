using DFS.DataEFCore;
using DFS.Domain.DbInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DFS.API.Configurations
{
    public static class ConfigureConnections
    {
        public static IServiceCollection AddConnectionProvider(this IServiceCollection services,
            IConfiguration configuration)
        {
            string connection = string.Empty;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                connection = configuration.GetConnectionString("DfsDbWindows") ??
                    "Server=.;Database=DFS;Trusted_Connection=True;";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)||RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                connection = configuration.GetConnectionString("DfsDbDocker") ??
                    "Server=localhost,1433;Database=DFS;User=sa;Password=sa;Trusted_Connection=False;";
            }

            services.AddDbContextPool<DfsContext>(options => options.UseSqlServer(connection));

            services.AddSingleton<IDbInfo>(new DbInfo(connection));

            return services;
        }
    }
}
