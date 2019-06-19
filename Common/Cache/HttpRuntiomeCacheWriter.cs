using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Cache
{
    public class HttpRuntiomeCacheWriter : ICacheWriter
    {
        public void AddCache(string key, object value, DateTime expDate)
        {

            HttpRuntime.Cache.Insert(key, value, null, expDate, TimeSpan.Zero);


        }

        public void AddCache(string key, object value)
        {

            HttpRuntime.Cache.Insert(key, value);

        }

        public object GetCache(string key)
        {
            return HttpRuntime.Cache[key];
        }

        public T GetCache<T>(string key)
        {


            return (T)HttpRuntime.Cache[key];

        }

        public void RemoveCache(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        public void SetCache(string key, object value, DateTime extDate)
        {
            HttpRuntime.Cache.Remove(key);
            AddCache(key, value, extDate);

        }


        public void SetCache(string key, object value)
        {
            HttpRuntime.Cache.Remove(key);
            AddCache(key, value);
        }
    }
}
