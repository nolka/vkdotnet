using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore
{
    public class UrlBuilder
    {
        private Dictionary<string, string> paramData;
        //private string query;

        private string host;

        /// <summary>
        /// Initializes api query builder
        /// </summary>
        /// <param name="apiId">Your desktop api id</param>
        /// <param name="si">Session info, provided by SessionManager</param>
        public UrlBuilder(string host)
        {
            this.paramData = new Dictionary<string, string>();
            this.host = host;
        }

        /// <summary>
        /// Adds parameters to query
        /// </summary>
        /// <param name="key">Parameter name</param>
        /// <param name="value">Parameter value</param>
        /// <returns>Return this</returns>
        public UrlBuilder Add(string key, string value)
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
            StringBuilder sb = new StringBuilder(this.host);

            // sorting params
            List<KeyValuePair<string, string>> myList = new List<KeyValuePair<string, string>>(this.paramData);
            myList.Sort(
                delegate(KeyValuePair<string, string> keyfirst,
                KeyValuePair<string, string> keylast)
                {
                    return keyfirst.Key.CompareTo(keylast.Key);
                }
            );

            foreach (KeyValuePair<string, string> rec in myList)
            {
                sb.Append(rec.Key + "=" + rec.Value + "&");
            }

            //sb.Append(
            return sb.ToString();//this.query;
        }
    }
}
