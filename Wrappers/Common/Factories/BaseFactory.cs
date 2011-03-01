using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore
{
    public abstract class BaseFactory
    {
        public ApiManager Manager;

        public BaseFactory(ApiManager manager)
        {
            this.Manager = manager;
        }
    }
}
