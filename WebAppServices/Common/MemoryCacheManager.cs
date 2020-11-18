using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppServices
{
    public class MemoryCacheManager
    {
        static MemoryCacheOptions cacheOps = new MemoryCacheOptions()
        {
            //缓存最大为100份
            //##注意netcore中的缓存是没有单位的，缓存项和缓存的相对关系
            //SizeLimit = 10000000,
            //缓存满了时，压缩20%（即删除20份优先级低的缓存项）
            CompactionPercentage = 0.2,
            //三秒钟查找一次过期项
            ExpirationScanFrequency = TimeSpan.FromSeconds(3)
        };
        public static MemoryCache myCache = new MemoryCache(cacheOps);


        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="timespan"></param>
        public static void SetCache<T>(String Key, T Value,TimeSpan ? timespan)
        {
            if(timespan.HasValue)
                myCache.Set<T>(Key, Value, timespan.Value);
            else
                myCache.Set<T>(Key, Value);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="timespan"></param>
        public static void SetRefushCache<T>(String Key, T Value, TimeSpan? timespan)
        { 
            myCache.Set<T>(Key, Value,new MemoryCacheEntryOptions
            {
                SlidingExpiration = timespan,
            }); 
        }


        public static void SetCache(String Key, String Value)
        {
            myCache.Set(Key, Value);
        }


        public static T GetCache<T>(String Key)
        {
            return myCache.Get<T>(Key);
        }


        public static object GetCache(String Key)
        {
            return myCache.Get(Key);
        }
    }
}
