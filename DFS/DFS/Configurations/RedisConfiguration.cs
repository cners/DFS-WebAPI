using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFS.API.Configurations
{
    /// <summary>
    /// Redis 缓存配置
    /// </summary>
    public static class RedisConfiguration
    {
        public static IServiceCollection AddRedisConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            var redis = new
            {
                Start = bool.Parse(configuration["AppSettings:Redis:Start"] ?? "false"),
                Master = new
                {
                    IP = configuration["AppSettings:Redis:Master:Connection"],
                    Password = configuration["AppSettings:Redis:Master:Password"]
                }
            };

            services.AddSingleton<DFS.CacheRedis.ICacheService>(new DFS.CacheRedis.RedisProvider(redis.Master.IP, redis.Master.Password));

            return services;
        }
    }
}
