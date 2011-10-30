using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using System.ComponentModel;

namespace ApiCore
{

    public delegate void ApiManagerLogHandler(object sender, string msg);
    public delegate void ApiManagerRequestCompleded(object sender, RequestResult result);

    public enum ResponseType
    {
        Xml,
        Json
    }

    public enum RequestResult
    {
        Running,
        Success,
        Error
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

        private static ApiManager instance = null;

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
                return "0.12 beta";
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
        /// When new log data available this event occurs
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

        public event ApiManagerRequestCompleded RequestCompleted;
        protected virtual void OnRequestCompleted(RequestResult result)
        {
            if (this.RequestCompleted != null)
            {
                this.RequestCompleted(this, result);
            }
        }

        /// <summary>
        /// Set new session info
        /// </summary>
        /// <param name="session">New session info</param>
        protected void UseSession(SessionInfo session)
        {
            this.session = session;
        }

        /// <summary>
        /// Get previous assigned session
        /// </summary>
        /// <returns>api session</returns>
        protected SessionInfo GetSession()
        {
            return this.session;
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
        /// Set api method to execution
        /// </summary>
        /// <param name="methodName">name of api method</param>
        /// <param name="args">array of arguments(example: new object[] { "owner_id", ownerId, "mid", msg_id })</param>
        /// <returns></returns>
        public ApiManager Method(string methodName, object[] args)
        {
            this.Method(methodName);
            for (int i = 0; i < args.Length; i += 2)
            {
                if (args[i + 1] != null)
                {
                    if (args[i + 1].GetType().ToString().Equals("System.Int32[]"))
                    {
                        this.Params(args[i].ToString(), CommonUtils.ArrayIntToCommaSeparatedString((int[])args[i + 1]));
                    }
                    else if (args[i + 1].GetType().ToString().Equals("System.String[]"))
                    {
                        this.Params(args[i].ToString(), CommonUtils.ArrayStringToCommaSeparatedString((string[])args[i + 1]));
                    }
                    else if (args[i + 1].GetType().ToString().Equals("System.Boolean"))
                    {
                        this.Params(args[i].ToString(), Convert.ToInt32(args[i + 1]));
                    }
                    else if (args[i + 1] is Object)
                    {
                        this.Params(args[i].ToString(), args[i + 1].ToString());
                    }
                    else
                    {
                        this.Params(args[i].ToString(), args[i + 1]);
                    }
                }
            }
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
                    this.OnRequestCompleted(RequestResult.Success);
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

                    this.OnRequestCompleted(RequestResult.Error);
                    throw new ApiRequestErrorException(msg, code, ht);
                }
                //return this;
            }
            else
            {
                this.OnRequestCompleted(RequestResult.Error);
                throw new ApiRequestEmptyAnswerException("API Server returns an empty answer or request timeout");
            }

            return this;
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
        public ApiCore.Messages.LongPollServerConnection GetLongPollServerConnection()
        {
            ApiCore.Messages.LongPollServerConnection connection = new ApiCore.Messages.LongPollServerConnection(this);
            return connection;
        }

        /// <summary>
        /// Create LongPoll connection unit
        /// </summary>
        /// <returns>LongPollServerConnection unit</returns>
        public ApiCore.Messages.LongPollServerConnection GetLongPollServerConnection(int waitTime)
        {
            ApiCore.Messages.LongPollServerConnection connection = new ApiCore.Messages.LongPollServerConnection(this);
            connection.WaitTime = waitTime;
            return connection;
        }
    }
}
