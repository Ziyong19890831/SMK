using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Web.Services
{
    [ScopedService]
    public class SessionManager
    {
        private readonly HttpContext httpContext;

        private readonly Dictionary<string, object> cache = new Dictionary<string, object>();

        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContext = httpContextAccessor.HttpContext;
        }

        public SessionManager(HttpContext httpContext)
        {
            this.httpContext = httpContext;
        }

        /// <summary>
        /// auto insert a key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>(bool useCache = true)
        {
            var key = typeof(T).FullName;
            if (this.cache.Keys.Contains(key))
            {
                return (T)this.cache[key];
            }
            if (!httpContext.Session.Keys.Contains(key))
            {
                return default(T);
            }
            var value = httpContext.Session.GetString(key);
            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        ///  auto use a key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public SessionManager Set<T>(T value)
        {
            var key = typeof(T).FullName;

            // also update cache
            this.cache[key] = value;

            var serializationResult = JsonConvert.SerializeObject(value);

            httpContext.Session.SetString(key, serializationResult);

            return this;
        }

        public void Remove<T>(T value) {
            var key = typeof(T).FullName;
            this.cache.Remove(key);
            httpContext.Session.Remove(key);
        }
    }
}
