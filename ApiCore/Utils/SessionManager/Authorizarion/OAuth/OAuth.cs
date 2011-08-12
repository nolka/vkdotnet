using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ApiCore
{
    public partial class OAuth : Form
    {
        public string Scope;
        public int AppId;
        public string Display;

        public OAuthSessionInfo si;
        public bool LoginInfoReceived = false;

        public OAuth(int appId, string scope, string display)
        {
            this.AppId = appId;
            this.Scope = scope;
            this.Display = display;
            InitializeComponent();
        }

        private void LoginWnd_Shown(object sender, EventArgs e)
        {
            string urlTemplate = "http://api.vkontakte.ru/oauth/authorize?client_id={0}&scope={1}&redirect_uri={2}&display={3}&response_type=token";
            this.LoginBrowser.Navigate(string.Format(urlTemplate, new object[] { this.AppId, this.Scope, "http://vkontakte.ru/api/login_success.html", this.Display }));
        }

        private void LoginBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (e.Url.ToString().Contains("#"))
            {
                Regex r = new Regex(@"\#(.*)");
                string[] json = r.Match(e.Url.ToString()).Value.Replace("#","").Split('&');
                Hashtable h = new Hashtable();
                foreach (string str in json)
                {
                    string[] kv = str.Split('=');
                    h[kv[0]] = kv[1];
                }

                this.si = new OAuthSessionInfo();
                this.si.AppId = this.AppId;
                this.si.Scope = this.Scope;
                this.si.Token = (string)h["access_token"];
                this.si.Expire = Convert.ToInt32(h["expires_in"]);

                this.LoginInfoReceived = true;
                this.Close();
            }
            
        }

        private void LoginBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            //this.wnd.LogIt("loading: "+ e.Url );
            
        }


    }
}
