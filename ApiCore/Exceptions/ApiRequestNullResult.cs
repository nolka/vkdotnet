using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore
{
    public class ApiRequestNullResult: Exception
    {
        public ApiRequestNullResult()
        {
        }

        public ApiRequestNullResult(string message)
        : base(message)
        {
        }

        public ApiRequestNullResult(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
