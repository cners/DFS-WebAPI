using System;
using System.Collections.Generic;
using System.Text;

namespace DFS.CacheRedis
{
    /// <summary>
    /// 缓存服务的公共接口
    /// </summary>
    public interface ICacheService
    {
        void StartUp(string redisIP, string password);
        void SetDbIndex(int index);

        void Dispose();

        bool RemoveKey(string key);

        bool KeyExpire(string key, int timeOut);

        bool HasKey(string key);

        bool Set(string key, string value, int timeOut);

        string Get(string key);

        bool Set<T>(string key, T value, int timeOut);

        long StringIncrement(string key);

        long StringDecrement(string key);

        bool HashSet(string key, Dictionary<string, string> values);

        string HashGet(string key, string filed);

        Dictionary<string, string> HashGet(string key);

        T Get<T>(string key);
    }
}
