using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore
{
    public class OAuthSessionInfo: SessionInfo
    {
        public string Token;
        public string Scope;
    }
}
