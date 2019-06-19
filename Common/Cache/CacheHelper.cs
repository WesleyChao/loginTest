using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Cache
{
    public class CacheHelper
    {
        // Spring.Net 直接注入一个Cache
        //
        public static ICacheWriter CacheWriter = new HttpRuntiomeCacheWriter();


        // 执行这个操作,只是为了保证, 对静态属性的注入的成功, 静态属性 只需要给赋值一次就行了, 但是如果如果使用者, 在使用之前, 没有创建过对象, 也就没有为静态属性赋值, 所以我们这里 在静态构造函数(在类执行之前) 通过容器, 创建一个对象, 实现对静态属性的注入,
        static CacheHelper()
        {
            // 通过spring容器, 创建一个实例
            //  IApplicationContext ctx = ContextRegistry.GetContext();

            // 方法1, 创建一个CacheHelper对象, 静态属性就会被赋值, 
            //ctx.GetObject("CacheHelper");
            // 方法二, 手动赋值
            //   CacheHelper.CacheWriter = ctx.GetObject("CacheWriter") as ICacheWriter;
        }

        public static void AddCache(string key, object value, DateTime expDate)
        {
            // 往缓存写: 单机, 分布式    观察者模式可以. 修改一下配置, 就能切换
            CacheWriter.AddCache(key, value, expDate);
        }
        public static void AddCache(string key, object value)
        {
            CacheWriter.AddCache(key, value);

        }
        public static object GetCache(string key)
        {
            return CacheWriter.GetCache(key);
        }

        public static void SetCache(string key, object value, DateTime extTime)
        {
            CacheWriter.SetCache(key, value, extTime);
        }
        public static void SetCache(string key, object value)
        {

            CacheWriter.SetCache(key, value);
        }
        public static void RemoveCache(string key)
        {
            CacheWriter.RemoveCache(key);
        }

    }
}
