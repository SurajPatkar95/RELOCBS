using RELOCBS.Common.ExceptionHandling;
using RELOCBS.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Utility
{
    public class CustomSessionStore : ISession
    {
        public T Get<T>(string key)
        {
            HttpContext currentContext = GetSessionContext();

            return (T)currentContext.Session[key];
        }

        public void Set<T>(string key, T entry)
        {
            HttpContext currentContext = GetSessionContext();
            currentContext.Session[key] = entry;
        }

        
        private static HttpContext GetSessionContext()
        {
            HttpContext currentContext = HttpContext.Current;

            if (currentContext == null)
            {
                throw new SessionException("Session Expires");
            }
            return currentContext;
        }

    }
}