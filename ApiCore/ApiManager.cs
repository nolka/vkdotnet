using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using System.ComponentModel;
using ApiCore.Handlers;

namespace ApiCore
{

    public delegate void ApiManagerLogHandler(object sender, string msg);
    public delegate void ApiManagerResponseData(object sender, string responseString, bool isCacheResult);
    public delegate void ApiManagerCacheUpdated(object sender, string key, string cache);
    public delegate void ApiManagerRequestCompleded(object sender, RequestResult result);
    public delegate void ApiManagerCapthaRequired(object sender, string url, string hash);

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

    public enum ErrorCodes
    {
        UnknownError = 1,
        ApplicationIsDisabled = 2,
        IncorrectSignature = 4,
        UserAuthFailed = 5,
        TooManyRequests = 6,
        PermissionToActionDenied = 7,
        CapthaRequired = 14,
        ParametersInvalid = 100
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

        public string LastMethod = null;

        private string cacheString = null;

        private Dictionary<string, string> cache = null;

        private static ApiManager instance = null;

        private bool isCancelled = false;
        private bool isCacheEnabled = false;

        /// <summary>
        /// Debug mode shifter
        /// </summary>
        public bool DebugMode = false;

        public void DebugMessage(string msg)
        {
            if (this.DebugMode)
            {
                this.Log("DBG: " + msg + "\n");
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
            this.OnCapthaRequired += new ApiManagerCapthaRequired(ApiManager_CapthaRequired);
        }

        void ApiManager_CapthaRequired(object sender, string url, string hash)
        {
            ApiCore.Handlers.CapthaWnd wnd = new CapthaWnd(this, url, hash);
            wnd.ShowDialog();
        }

        #region events

        /// <summary>
        /// When new log data available this event occurs
        /// </summary>
        public event ApiManagerLogHandler OnLog;
        /// <summary>
        /// Logging event function
        /// </summary>
        /// <param name="msg">Message to log</param>
        protected virtual void Log(string msg)
        {
            if (this.OnLog != null)
            {
                this.OnLog(this, msg);
            }
        }

        public event ApiManagerRequestCompleded OnRequestCompleted;
        protected virtual void RequestCompleted(RequestResult result)
        {
            if (this.OnRequestCompleted != null)
            {
                this.OnRequestCompleted(this, result);
            }
        }

        public event ApiManagerCapthaRequired OnCapthaRequired;
        public void CapthaRequired(string capthaUrl, string hash)
        {
            if (this.OnCapthaRequired != null)
            {
                this.OnCapthaRequired(this, capthaUrl, hash);
            }
        }


        public event ApiManagerResponseData OnResponseData;
        public void ResponseData(string responseString, bool isCacheResult)
        {
            if (this.OnResponseData != null)
            {
                this.OnResponseData(this, responseString, isCacheResult);
            }
        }

        public event ApiManagerCacheUpdated OnCacheUpdated;
        public void CacheUpdated(string key, string cache)
        {
            if (this.OnCacheUpdated != null)
            {
                this.OnCacheUpdated(this, key, cache);
            }
        }

#endregion


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


        public void EnableCaching(bool clearIfExists = false)
        {
            this.isCacheEnabled = true;
            if (this.cache == null)
                this.ClearCacheData();
            if (clearIfExists && this.cache != null && this.cache.Count > 0)
                this.ClearCacheData();

        }

        public void DisableCaching(bool clearCache = false)
        {
            this.isCacheEnabled = false;
            if (clearCache)
                this.ClearCacheData();
        }

        public void AppendCache(string key, string cache)
        {
            key = CommonUtils.Md5(key).ToLower();
            if (this.cache.ContainsKey(key))
            {
                if(this.cache[key].Equals(cache))
                {}
                else
                {
                    this.cache.Remove(key);
                    this.cache.Add(key, cache);
                    this.CacheUpdated(key, cache);
                }
            }
            else
            {
                this.cache.Add(key, cache);
                this.CacheUpdated(key, cache);
            }
        }

        private string getFromCache(string key)
        {
            if (this.cache.ContainsKey(key))
                return this.cache[key];
            else
                return null;
        }

        public void SetCacheString(string cache)
        {
            this.cacheString = cache;
        }

        public void ClearCacheData()
        {
            this.cache = new Dictionary<string, string>();
        }

        /// <summary>
        /// Sets the method name wich has been executed
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <returns>this</returns>
        public ApiManager Method(string methodName)
        {
            this.MethodSuccessed = false;
            this.isCancelled = false;
            this.LastMethod = methodName;
            
            this.builder = new ApiQueryBuilder(this.appId, this.session);
            this.builder.Add("method", methodName);
            if (this.ResponseAs == ResponseType.Json)
            {
                this.builder.Add("format", "json");
            }
            this.DebugMessage("Method: " + methodName);
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
                        this.Params(args[i].ToString(), CommonUtils.IntArrayToCommaSeparatedString((int[])args[i + 1]));
                    }
                    else if (args[i + 1].GetType().ToString().Equals("System.String[]"))
                    {
                        this.Params(args[i].ToString(), CommonUtils.StringArrayToCommaSeparatedString((string[])args[i + 1]));
                    }
                    else if (args[i + 1].GetType().ToString().Equals("System.Boolean"))
                    {
                        this.Params(args[i].ToString(), Convert.ToInt32(args[i + 1]));
                    }
                    else if (args[i + 1].GetType().ToString().Equals("ApiCore.MessageAttachment[]"))
                    {
                        this.Params(args[i].ToString(), CommonUtils.ObjectsArrayToCommaSeparatedString((object[])args[i + 1]));
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
            this.DebugMessage("Params: " + key + " => " + value);
            return this;
        }

        /// <summary>
        /// Execute method
        /// </summary>
        /// <returns>himself</returns>
        public ApiManager Execute()
        {
            return this.getResponse();
        }

        public ApiManager ExecuteFromCacheString(string cacheString)
        {
            this.SetCacheString(cacheString);
            return this.getResponse();
        }

        public ApiManager ExecuteFromCacheByKey(string key)
        {
            this.SetCacheString(this.getFromCache(key));
            return this.getResponse();
        }

        private ApiManager getResponse()
        {
            if (this.isCancelled)
                return this;
            try
            {
                this.apiResponseXml = null;
                string req = this.builder.BuildQuery();
                if (this.cacheString == null)
                {
                    this.Log("Request string: " + req);
                    ApiRequest request = new ApiRequest();
                    request.Timeout = this.Timeout;
                    this.apiResponseString = request.Send(req);
                    if (this.isCacheEnabled)
                    {
                        this.AppendCache(req, this.apiResponseString);
                    }
                    this.DebugMessage(this.apiResponseString);
                }
                else
                {
                    this.apiResponseString = cacheString;
                    this.DebugMessage("FROM CACHE: "+this.apiResponseString);
                }
                
                if (!this.apiResponseString.Equals("") || this.apiResponseString.Length > 0)
                {
                    this.ResponseData(this.apiResponseString, (cacheString != null));
                    this.apiResponseXml = new XmlDocument();
                    this.apiResponseXml.LoadXml(this.apiResponseString);
                    XmlNode isError = this.apiResponseXml.SelectSingleNode("/error");
                    if (isError == null)
                    {
                        this.MethodSuccessed = true;
                        this.RequestCompleted(RequestResult.Success);
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

                        this.RequestCompleted(RequestResult.Error);
                        throw new ApiRequestErrorException(msg, code, ht);
                    }
                    //return this;
                }
                else
                {
                    this.RequestCompleted(RequestResult.Error);
                    throw new ApiRequestEmptyAnswerException("API Server returns an empty answer or request timeout");
                }
            }
            catch (ApiRequestErrorException e)
            {
                if (e.Code == (int)ErrorCodes.CapthaRequired)
                {
                    if (!this.isCancelled)
                    {
                        XmlUtils.UseNode(this.apiResponseXml.SelectSingleNode("/error"));
                        this.CapthaRequired(XmlUtils.String("captcha_img"), XmlUtils.String("captcha_sid"));
                    }
                    //return this;
                }
                else
                    throw new ApiRequestErrorException(e.Message, e.Code, e.ParamsPassed);
            }
            return this;
        }


        public void Cancel()
        {
            this.isCancelled = true;
        }

        public XmlDocument GetResponseXml()
        {
            if (this.apiResponseXml != null)
            {
                XmlDocument xdoc = new XmlDocument();
                XmlNode response = this.apiResponseXml.SelectSingleNode("/response");
                if (response != null)
                {
                    XmlNode temporary = xdoc.ImportNode(response, true);
                    xdoc.AppendChild(temporary);
                    return xdoc;
                }
                else
                {
                    throw new Exception("Response string is empty!");
                }
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

        public XmlDocument GetXmlDocument()
        {
            XmlDocument x = new XmlDocument();
            x.LoadXml(this.GetResponseString());
            return x;
        }

        
    }
}
