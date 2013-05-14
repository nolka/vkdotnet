using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.Utils.Authorization.Exceptions
{
    public class AuthorizationFailedException: Exception
    {
        public AuthorizationFailedException(string message): base(message)
        {
        }
    }
}
