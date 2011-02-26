using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Net;

namespace ApiCore
{
    public class NoBrowser
    {
        
        private XmlDocument getFormByTargetFrame(string code, string formName)
        {
            code = code.Replace("\n", "");
            Regex r = new Regex(@"\<form[^\>]*\>(.*?)\<\/form\>");
            MatchCollection matches = r.Matches(code);
            foreach (Match m in matches)
            {
                if (new Regex(formName).IsMatch(m.Value))
                {
                    XmlDocument x = new XmlDocument();
                    x.LoadXml(m.Value);
                    return x;
                }
            }
            return null;
        }

        private XmlDocument getFormById(string code, string formid)
        {
            code = code.Replace("\n", "");
            Regex r = new Regex("\\<form[^\\>]+id\\=\\\""+formid+"\\\"[^\\>]*\\>(.*?)\\<\\/form\\>");
            Match m = r.Match(code);

            XmlDocument x = new XmlDocument();
            //x.LoadXml(m.Value);
            return x;
                
        }

        public NoBrowser(string login, string pwd)
        {
            //HttpDownloaderFactory hd = new HttpDownloaderFactory(new HttpDownloaderOptions("GET", "http://vkontakte.ru/login.php?app=1928531&layout=popup&type=browser&settings=16378", null, null, 5000, true, new CookieContainer()));//"http://vkontakte.ru/login.php?app=1928531&layout=popup&type=browser&settings=16378");
            //HttpDownloaderFactory hd = new HttpDownloaderFactory(new HttpDownloaderOptions("POST", "http://84.22.154.19/ordering/cli/indigo/test.php?test=yes", null, null, 5000));
           
            //string LoginForm = hd.DownloadToString();
            //MessageBox.Show(LoginForm+"\n\n" + hd.GetUrlParams("http://vkontakte.ru/login.php?app=1928531&layout=popup&type=browser&settings=16378"));
            //XmlDocument x = this.getFormByTargetFrame(LoginForm, "login_frame");
            //UrlBuilder ub = new UrlBuilder("http://login.vk.com?");
            //ub.Add("email", login);
            //ub.Add("pass", pwd);

            //XmlNodeList nodes = x.SelectNodes("/form/input");
            //foreach (XmlNode node in nodes)
            //{
            //    if (node.Attributes["name"].Value == "email")
            //    {
            //        ub.Add(node.Attributes["name"].Value, login);
            //        continue;
            //    }
            //    if (node.Attributes["name"].Value == "pass")
            //    {
            //        ub.Add(node.Attributes["name"].Value, pwd);
            //        continue;
            //    }
            //    ub.Add(node.Attributes["name"].Value, ((node.Attributes["value"] == null) ? "" : node.Attributes["value"].Value));
            //}

            //hd = new HttpDownloaderFactory(new HttpDownloaderOptions("POST", ub.BuildQuery(), null, null, 5000, true, new CookieContainer()));
            //x = this.getFormById(hd.DownloadToString(), "l");
            //MessageBox.Show(hd.DownloadToString());
            //MessageBox.Show.Matches(LoginForm).Count.ToString());
        }
    }
}
