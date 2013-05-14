using System;
using System.Collections.Generic;
using System.Text;
using ApiCore.Utils.Authorization;

namespace ApiCore
{
    public class SessionInfo
    {
        public int AppId;
        public int Expire;
		public int UserId;
        public AuthType AuthType;
    }
}
