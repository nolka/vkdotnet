using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using System.ComponentModel;

namespace ApiCore
{
    public delegate void ApiManagerLogHandler(object sender, string msg);

    public enum ResponseType
    {
        Xml, Json
    }

    /// <summary>
    /// This class provides factory to communicating with vkontakte.api
    /// </summary>
    public class ApiManager
    {
        private ApiQueryBuilder builder;
        private SessionInfo session;

        private XmlDocument apiResponseXml;
        private string apiResponseString;

        private int appId = 0;

        /// <summary>
        /// If previous query successed - true, else false
        /// </summary>
        public bool MethodSuccessed = false;

        /// <summary>
        /// Debug mode shifter
        /// </summary>
        public bool DebugMode = false;

        private void debugMsg(string msg)
        {
            if (this.DebugMode)
            {
                this.OnLog("DBG: " + msg + "\n");
            }
        }

        /// <summary>
        /// ApiManager version
        /// </summary>
        public string Version
        {
            get
            {
                return "0.6.9.5 beta";
            }
        }

        /// <summary>
        /// Per-query timeout
        /// </summary>
        public int Timeout = 15000;

        /// <summary>
        /// Sets the type of response.
        /// ResponseType.Xml - all responses will be returned in XML
        /// ResponseType.Json - all responses will be returned in JSON
        /// </summary>
        public ResponseType ResponseAs = ResponseType.Xml;

        /// <summary>
        /// Init new API manager
        /// </summary>
        /// <param name="apiId">Registered desktop application id</param>
        /// <param name="si">Session information</param>
        public ApiManager(SessionInfo si)
        {
            this.appId = si.AppId;
            this.session = si;
        }

        /// <summary>
        /// When new log data available
        /// </summary>
        public event ApiManagerLogHandler Log;
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
        /// Sets the method name wich has been executed
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <returns>this</returns>
        public ApiManager Method(string methodName)
        {
            this.MethodSuccessed = false;
            ApiRequest.Timeout = this.Timeout;
            this.builder = new ApiQueryBuilder(this.appId, this.session);
            this.builder.Add("method", methodName);
            if (this.ResponseAs == ResponseType.Json)
            {
                this.builder.Add("format", "json");
            }
            this.debugMsg("Method: " + methodName);
            return this;
        }

        /// <summary>
        /// Add parameters to the method
        /// </summary>
        /// <param name="key">Parameter name</param>
        /// <param name="value">Parameter value</param>
        /// <returns>this</returns>
        public ApiManager Params(string key, object value)
        {
            this.builder.Add(key, value.ToString());
            this.debugMsg("Params: " + key + " => " + value);
            return this;
        }

        /// <summary>
        /// Execute method
        /// </summary>
        /// <returns>himself</returns>
        public ApiManager Execute()
        {
            //try
            //{
            //BackgroundWorker bw = new BackgroundWorker();
            //bw.DoWork += (o, e) =>
            //    {
                    this.apiResponseXml = null;
                    string req = this.builder.BuildQuery();
                    this.OnLog("Request string: " + req);
                    ApiRequest.Timeout = this.Timeout;
                    this.apiResponseString = ApiRequest.Send(req);
                    this.debugMsg(this.apiResponseString);
                    if (!this.apiResponseString.Equals("") || this.apiResponseString.Length > 0)
                    {
                        this.apiResponseXml = new XmlDocument();
                        this.apiResponseXml.LoadXml(this.apiResponseString);
                        XmlNode isError = this.apiResponseXml.SelectSingleNode("/error");
                        if (isError == null)
                        {
                            this.MethodSuccessed = true;
                        }
                        else
                        {
                            int code = Convert.ToInt32(isError.SelectSingleNode("error_code").InnerText);
                            string msg = isError.SelectSingleNode("error_msg").InnerText;
                            Hashtable ht = new Hashtable();
                            XmlNodeList pparams = isError.SelectNodes("request_params/param");
                            foreach (XmlNode n in pparams)
                            {
                                ht[n.SelectSingleNode("key").InnerText.ToString()] = n.SelectSingleNode("value").InnerText.ToString();
                            }

                            throw new ApiRequestErrorException("Server error occurred", code, msg, ht);
                        }
                        //return this;
                    }
                    else
                    {
                        throw new ApiRequestEmptyAnswerException("API Server returns an empty answer or request timeout");
                    }
                    
                //};
            //bw.RunWorkerCompleted += (o, e) =>
            //    {
                    
            //    }
            //bw.WorkerSupportsCancellation = true;
            //bw.RunWorkerAsync();
            return this;
            //}
            //catch(Exception e)
            //{
            //    throw new ApiRequestNullResult("source message: "+e.Message);
            //}
        }

        public XmlNode GetResponseXml()
        {
            if (this.apiResponseXml != null)
            {
                return this.apiResponseXml.SelectSingleNode("/response");
            }
            return null;
        }

        public string GetResponseString()
        {
            return this.apiResponseString;
        }

        public object GetResponseJson()
        {
            return this.apiResponseString;
        }
       

        /// <summary>
        /// Get XmlDocument instance from string
        /// </summary>
        /// <param name="str">xml string</param>
        /// <returns>XmlDocument</returns>
        public XmlDocument GetXmlDocument(string str)
        {
            XmlDocument x = new XmlDocument();
            x.LoadXml(str);
            return x;
        }

        /// <summary>
        /// Create LongPoll connection unit
        /// </summary>
        /// <returns>LongPollServerConnection unit</returns>
        public LongPollServerConnection GetLongPollServerConnection()
        {
            LongPollServerConnection connection = new LongPollServerConnection(this);
            return connection;
        }

        /// <summary>
        /// Create LongPoll connection unit
        /// </summary>
        /// <returns>LongPollServerConnection unit</returns>
        public LongPollServerConnection GetLongPollServerConnection(int waitTime)
        {
            LongPollServerConnection connection = new LongPollServerConnection(this);
            connection.WaitTime = waitTime;
            return connection;
        }
    }
}
