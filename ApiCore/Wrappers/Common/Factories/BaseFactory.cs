using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore
{
    public abstract class BaseFactory
    {
        public ApiManager Manager;

        public bool CacheRequests = false;

        public BaseFactory(ApiManager manager)
        {
            this.Manager = manager;
        }

        public bool LoadFromXmlCacheString(string cacheString)
        {
            return false;
        }

        public bool SaveToXmlCacheString()
        { return false; }
    }
}
