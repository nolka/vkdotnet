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
        private Dictionary<string, string> paramData;
        //private string query;

        private int apiId;
        private SessionInfo session;

        /// <summary>
        /// Initializes api query builder
        /// </summary>
        /// <param name="apiId">Your desktop api id</param>
        /// <param name="si">Session info, provided by SessionManager</param>
        public ApiQueryBuilder(int apiId, SessionInfo si)
        {
            this.paramData = new Dictionary<string, string>();
            this.apiId = apiId;
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
        public string BuildQuery()
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

            StringBuilder md5sig = new StringBuilder(this.session.MemberId);
            foreach (KeyValuePair<string, string> rec in myList)
            {
                md5sig.Append(rec.Key + "=" + rec.Value);
                sb.Append(rec.Key + "=" + rec.Value + "&");
            }

            md5sig.Append(this.session.Secret);
            sb.Append("sig=" + CommonUtils.Md5(md5sig.ToString()).ToLower());
            sb.Append("&sid=" + this.session.SessionId);
            //sb.Append(
            return sb.ToString();//this.query;
        }
    }
}
