using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;

using ApiCore;

namespace ApiCore
{
    /// <summary>
    /// Class for building url queries to vkontakte api
    /// </summary>
    public class ApiQueryBuilder
    {
        // pointer to a function that is responsible for the construction of Uri query api
        private delegate string BuildApiQuery();

        private Dictionary<string, string> paramData;

        private int apiId;

        private SessionInfo session;

        private BuildApiQuery builderFunc;


        /// <summary>
        /// Initializes api query builder
        /// </summary>
        /// <param name="apiId">Your desktop api id</param>
        /// <param name="si">Session info, provided by SessionManager</param>
        public ApiQueryBuilder(int apiId, SessionInfo si)
        {
            this.paramData = new Dictionary<string, string>();
            this.apiId = apiId;
            // If si is instance of VKSessionInfo(old auth method)
            if (si is VKSessionInfo)
            {
                this.session = new VKSessionInfo();
                this.builderFunc = new BuildApiQuery(this.VKBuildQuery);
            }
            else
            {
                this.session = new OAuthSessionInfo();
                this.builderFunc = new BuildApiQuery(this.OAuthBuildQuery);
            }
            this.session = si;
            this.Add("api_id", this.apiId.ToString());
        }

        /// <summary>
        /// Adds parameters to API request
        /// </summary>
        /// <param name="key">Parameter name</param>
        /// <param name="value">Parameter value</param>
        /// <returns>Return this</returns>
        public ApiQueryBuilder Add(string key, string value)
        {
            this.paramData.Add(key, value);
            return this;
        }

        /// <summary>
        /// Clear api parameters
        /// </summary>
        public void Clear()
        {
            this.paramData.Clear();
        }

        /// <summary>
        /// Build query string
        /// </summary>
        /// <returns>Ready query string</returns>
        /// 
        public string BuildQuery()
        {
            return this.builderFunc();
        }

        private string VKBuildQuery()
        {
            StringBuilder sb = new StringBuilder("http://api.vkontakte.ru/api.php?");
            
            this.Add("v", "3.0");
            // sorting params
            List<KeyValuePair<string, string>> myList = new List<KeyValuePair<string, string>>(this.paramData); 
            myList.Sort( 
                delegate(KeyValuePair<string, string> keyfirst, 
                KeyValuePair<string, string> keylast) 
                { 
                    return keyfirst.Key.CompareTo(keylast.Key); 
                } 
            );

            StringBuilder md5sig = new StringBuilder(((VKSessionInfo)this.session).MemberId);
            foreach (KeyValuePair<string, string> rec in myList)
            {
                md5sig.Append(rec.Key + "=" + rec.Value);
                sb.Append(rec.Key + "=" + rec.Value + "&");
            }

            md5sig.Append(((VKSessionInfo)this.session).Secret);
            sb.Append("sig=" + CommonUtils.Md5(md5sig.ToString()).ToLower());
            sb.Append("&sid=" + ((VKSessionInfo)this.session).SessionId);
            //sb.Append(
            return sb.ToString();//this.query;
        }

        private string OAuthBuildQuery()
        {
            StringBuilder sb = new StringBuilder("https://api.vkontakte.ru/method/");

            if (this.paramData.ContainsKey("method"))
            {
                sb.Append(this.paramData["method"]);
                this.paramData.Remove("method");
                if (!this.paramData.ContainsKey("format"))
                {
                    sb.Append(".xml?");
                }
                else
                {
                    sb.Append("?");
                    this.paramData.Remove("format");
                }

                foreach (KeyValuePair<string, string> rec in this.paramData)
                {
                    sb.Append(rec.Key + "=" + rec.Value + "&");
                }

                sb.Append("access_token=");
                sb.Append(((OAuthSessionInfo)this.session).Token);

                return sb.ToString();
            }
            return "";
        }
    }
}
