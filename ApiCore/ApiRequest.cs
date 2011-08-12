using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace ApiCore
{
    public static class ApiRequest
    {
        private static HttpWebRequest request;
        private static HttpWebResponse response;

        public static int Timeout = 15000;

        /// <summary>
        /// Send request to API
        /// </summary>
        /// <param name="url">Url to request</param>
        /// <returns>String, contains response from api. by default, it's - XML</returns>
        public static string Send(string url)
        {
            try
            {
                ApiRequest.request = (HttpWebRequest)HttpWebRequest.Create(url);
                ApiRequest.request.UserAgent = "xternalx vkontakte api wrapper/0.1a";
                ApiRequest.request.Timeout = ApiRequest.Timeout;
                ApiRequest.request.Method = "GET";

                ApiRequest.response = (HttpWebResponse)ApiRequest.request.GetResponse();

                Stream responseStream = ApiRequest.response.GetResponseStream();

                if (responseStream == null)
                {
                    throw new ApiRequestNullResult("No content was been downloaded");
                }
                else
                {
                    StreamReader sr = new StreamReader(responseStream);
                    
                    return sr.ReadToEnd();
                }
            }
            catch(Exception e)
            {
                throw new ApiRequestNullResult("Unknown result: "+e.Message);
            }
        }

        
    }
}
