using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ApiCore.Utils.Authorization
{
    public partial class Relogin : Form
    {
        public int AppId;
        public string Scope;
        public string Display;

        public OAuthSessionInfo SessionData;
        public bool Authenticated = false;
        public bool LoginInfoReceived = false;

        public Relogin(int appId, string scope, string display)
        {
            this.AppId = appId;
            this.Scope = scope;
            this.Display = display;
            InitializeComponent();
        }

        private void LoginWnd_Shown(object sender, EventArgs e)
        {
            string urlTemplate = "http://api.vkontakte.ru/oauth/authorize?client_id={0}&scope={1}&redirect_uri={2}&display={3}&response_type=token&revoke=1";
            this.LoginBrowser.Navigate(string.Format(urlTemplate, new object[] { this.AppId, this.Scope, "http://vkontakte.ru/api/login_success.html", this.Display }));
        }

        private void LoginBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (e.Url.ToString().Contains("#"))
            {
                Regex r = new Regex(@"\#(.*)");
                string[] json = r.Match(e.Url.ToString()).Value.Replace("#", "").Split('&');
                Hashtable h = new Hashtable();
                foreach (string str in json)
                {
                    string[] kv = str.Split('=');
                    h[kv[0]] = kv[1];
                }

                this.SessionData = new OAuthSessionInfo();
                this.SessionData.AppId = this.AppId;
                this.SessionData.Scope = this.Scope;
                this.SessionData.Token = (string)h["access_token"];
                this.SessionData.Expire = Convert.ToInt32(h["expires_in"]);
                this.SessionData.UserId = Convert.ToInt32(h["user_id"]);
                this.LoginInfoReceived = true;

                this.Authenticated = true;
                this.Close();
            }

        }

        private void LoginBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            //this.wnd.LogIt("loading: "+ e.Url );

        }

    }
}
