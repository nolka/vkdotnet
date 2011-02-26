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
    public partial class BrowserAuthorization : Form
    {
        public int Permissions;
        public int AppId;

        public SessionInfo si;
        public bool LoginInfoReceived = false;

        public BrowserAuthorization()
        {
            InitializeComponent();
        }

        private void LoginWnd_Shown(object sender, EventArgs e)
        {
            this.LoginBrowser.Navigate("http://vkontakte.ru/login.php?app="+this.AppId.ToString()+"&layout=popup&type=browser&settings="+this.Permissions.ToString());
        }

        private void LoginBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (e.Url.ToString().Contains("#"))
            {
                Regex r = new Regex(@"\{(.*)\}");
                string[] json = r.Match(e.Url.ToString()).Value.Replace("{", "").Replace("}", "").Replace("\"", "").Split(',');
                Hashtable h = new Hashtable();
                foreach (string str in json)
                {
                    string[] kv = str.Split(':');
                    h[kv[0]] = kv[1];
                }

                this.si = new SessionInfo();
                this.si.AppId = this.AppId;
                this.si.Permissions = this.Permissions;
                this.si.MemberId = (string)h["mid"];
                this.si.SessionId = (string)h["sid"];
                this.si.Expire = Convert.ToInt32(h["expire"]);
                this.si.Secret = (string)h["secret"];
                this.si.Signature = (string)h["sig"];

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
