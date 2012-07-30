using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace ApiCore
{
    public class ApiRequest
    {
        private  HttpWebRequest request;
        private  HttpWebResponse response;

        public int Timeout = 15000;

        /// <summary>
        /// Send request to API
        /// </summary>
        /// <param name="url">Url to request</param>
        /// <returns>String, contains response from api. by default, it's - XML</returns>
        public string Send(string url)
        {
            //try
            //{
                this.request = (HttpWebRequest)HttpWebRequest.Create(url);
                this.request.UserAgent = "xternalx vkontakte api wrapper/0.1a";
                this.request.Timeout = this.Timeout;
                this.request.Method = "GET";

                this.response = (HttpWebResponse)this.request.GetResponse();

                Stream responseStream = this.response.GetResponseStream();

                if (responseStream == null)
                {
                    throw new ApiRequestNullResult("No content was been downloaded");
                }
                else
                {
                    StreamReader sr = new StreamReader(responseStream);

                    return sr.ReadToEnd();
                }
            //}
            //catch (WebException e)
            //{
            //    Console.WriteLine("ololo: "+e.Message);
            //    return null;
            //}
            //catch (Exception e)
            //{
            //    throw new ApiRequestNullResult("Unknown result: " + e.Message);
            //}
        }

        
    }
}
