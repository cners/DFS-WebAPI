using DFS.DataEFCoreMySQL;
using DFS.Domain.DbInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;

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
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                connection = configuration.GetConnectionString("DfsDbDocker") ??
                    "Server=localhost,1433;Database=DFS;User=sa;Password=sa;Trusted_Connection=False;";
            }

            // 使用MySQL数据库，如需SQLserver数据请切换UseSqlServer(connection)
            services.AddDbContextPool<DfsContext>(options => options.UseMySQL(connection));

            services.AddSingleton<IDbInfo>(new DbInfo(connection));

            return services;
        }
    }
}
