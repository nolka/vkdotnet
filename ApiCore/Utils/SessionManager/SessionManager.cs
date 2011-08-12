using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace ApiCore
{
    public delegate void SessionManagerLogHandler(object sender, string msg);
    public class SessionManager
    {

        private List<SessionInfo> sessions;

        public  AuthType AuthorizationType;

        public int AppId;
        public int Permissions = 0;
        public string Scope = null;

        public SessionManager(int appId, int perms)
        {
            this.sessions = new List<SessionInfo>();
            this.AppId = appId;
            this.Permissions = perms;
            this.AuthorizationType = AuthType.VKAuth;
        }

        public SessionManager(int apiId, string scope)
        {
            this.sessions = new List<SessionInfo>();
            this.AppId = apiId;
            this.Scope = scope;
            this.AuthorizationType = AuthType.OAuth;
        }

        public event SessionManagerLogHandler Log;
        /// <summary>
        /// Logging event function
        /// </summary>
        /// <param name="msg">Message to log</param>
        protected virtual void OnLog(string msg)
        {
            if (this.Log != null)
            {
                this.Log(this, msg);
            }
        }

        /// <summary>
        /// Gets the session with browser
        /// </summary>
        /// <returns>SessionInfo with info about user session. Null - if login failed</returns>
        public SessionInfo GetSession()
        {
            this.OnLog("Creating login wnd...");
            VKAuth wnd = new VKAuth();
            wnd.Permissions = this.Permissions;
            wnd.AppId = this.AppId;
            wnd.ShowDialog();
            if (wnd.LoginInfoReceived)
            {
                this.OnLog("Authorization successed!");
                return wnd.si;
            }
            else
            {
                this.OnLog("Authorization failed!");
                return null;
            }
        }

        public SessionInfo GetOAuthSession()
        {
            this.OnLog("Creating OAuth login wnd...");
            OAuth wnd = new OAuth(this.AppId, this.Scope, AuthDisplay.Popup);
            wnd.ShowDialog();
            if (wnd.LoginInfoReceived)
            {
                this.OnLog("Authorization successed!");
                return wnd.si;
            }
            else
            {
                this.OnLog("Authorization failed!");
                return null;
            }
        }

        public SessionInfo GetSession(string login, string pwd)
        {
            this.OnLog("Trying to get session with login "+login);
            NoBrowser no = new NoBrowser(login, pwd);
            return null;
        }


        public void CloseSession()
        {
        }


    }
}
