using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;

namespace ApiCore.Messages
{
    public delegate void LongPollServerConnectionDataReceivedEventHandler(object sender, LongPollServerEventArgs e);
    public delegate void LongPollServerConnectionClosedEventHandler(object sender);

    public class LongPollServerEventArgs : EventArgs
    {
        /// <summary>
        /// Last event id
        /// </summary>
        public long LastEventId = 0;

        /// <summary>
        /// Full json source code from longpoll server
        /// </summary>
        public string EventSourceCode;
    }

    /// <summary>
    /// Class represents vkontakte longPollServer connection
    /// </summary>
    public class LongPollServerConnection : Object
    {
        private ApiManager manager;
        
        private volatile bool stopPending = false;

        private object locker = new object();

        private List<string> longPollMessages;

        private string lastLongPollMessage;

        private Thread connectionListenerThread;

        private SynchronizationContext sync;

        private ApiRequest request;

        /// <summary>
        /// When data recieved
        /// </summary>
        public event LongPollServerConnectionDataReceivedEventHandler ReceivedData;
        /// <summary>
        /// When long poll client stopped
        /// </summary>
        public event LongPollServerConnectionClosedEventHandler Stopped;

        /// <summary>
        /// LongPoll server key
        /// </summary>
        public string Key;

        /// <summary>
        /// LongPoll server url
        /// </summary>
        public string Server;

        /// <summary>
        /// Last event id
        /// </summary>
        public int LastEventId;

        /// <summary>
        /// Timeout to longpoll server. default=25
        /// </summary>
        public int WaitTime
        {
            get
            {
                return this.waitTime;
            }
            set
            {
                this.waitTime = value;
            }
        }
        private int waitTime = 25;

        /// <summary>
        /// Create LongPoll Connection
        /// </summary>
        /// <param name="manager">ApiManager</param>
        public LongPollServerConnection(ApiManager manager)
        {
            this.longPollMessages = new List<string>();
            this.manager = manager;
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Trace.AutoFlush = true;
            Trace.Indent();
        }

        /// <summary>
        /// Method, that received LongPoll server connecion info
        /// </summary>
        /// <returns>if success - true, otherwise - false</returns>
        private bool GetLongPollServerConnectionData()
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(this.manager.Method("messages.getLongPollServer").Execute().GetResponseString());
            if (xdoc.SelectSingleNode("/error") == null && xdoc.InnerText != "")
            {
                this.Key = xdoc.SelectSingleNode("/response/key").InnerText;
                this.Server = xdoc.SelectSingleNode("/response/server").InnerText;
                this.LastEventId = Convert.ToInt32(xdoc.SelectSingleNode("/response/ts").InnerText);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Occurs when LongPoll server received some data
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnReceivedData(LongPollServerEventArgs e)
        {
            if (this.ReceivedData != null)
            {
                this.ReceivedData(this, e);
            }
        }

        /// <summary>
        /// Stop LongPoll cient
        /// </summary>
        protected virtual void OnStopped()
        {
            if (this.Stopped != null)
            {
                this.Stopped(this);
            }
        }

        /// <summary>
        /// Start listening server connection
        /// </summary>
        public void Start()
        {
            this.sync = SynchronizationContext.Current;
            this.stopPending = false;
            if (this.connectionListenerThread != null)
            {

                if (this.connectionListenerThread.ThreadState == System.Threading.ThreadState.Stopped)
                {
                    //ThreadStart ts = new ThreadStart(this.run);
                    this.connectionListenerThread = new Thread(this.run);
                    this.connectionListenerThread.Start(this.sync);
                }
                if (this.connectionListenerThread.ThreadState == System.Threading.ThreadState.Unstarted || this.connectionListenerThread.ThreadState == System.Threading.ThreadState.Suspended)
                {
                    this.connectionListenerThread = new Thread(this.run);
                    this.connectionListenerThread.Start(this.sync);
                }
                this.connectionListenerThread.IsBackground = true;
            }
            else
            {
                //ThreadStart ts = new ThreadStart(this.run);
                this.connectionListenerThread = new Thread(this.run);
                this.Start();
            }
        }

        private void run(object state)
        {
            try
            {
                SynchronizationContext context = state as SynchronizationContext;
                context.Send(this.doGetLongPollServerConnectionData, null);
                context.Send(this.doGetSendRequest, null);
                if (this.request == null)
                {
                    this.request = new ApiRequest();
                    this.request.Timeout = this.WaitTime;
                } 
                while (!this.stopPending)
                {
                    this.lastLongPollMessage = this.request.Send("http://" + this.Server + "?act=a_check&key=" + this.Key + "&ts=" + this.LastEventId.ToString() + "&wait=" + this.WaitTime.ToString() + "&mode=0");
                    if (this.lastLongPollMessage == "")
                    {
                        continue;
                    }
                    // {"ts":727820493,"updates":[[8,-696076,0]]}
                    if (new Regex("\\\"?failed\\\"?\\s*?\\:\\s*?\\d+").Match(this.lastLongPollMessage).Success)
                    {
                        Console.WriteLine(this.lastLongPollMessage);
                        context.Send(this.doGetLongPollServerConnectionData, null);

                    }
                    else
                    {
                        this.LastEventId = Convert.ToInt32(new Regex("\\{[\\s]*?\\\"ts\\\"[\\s]*?\\:[\\s]*?(\\d+)[\\s]*?").Match(this.lastLongPollMessage).Groups[1].Value);
                        if (this.lastLongPollMessage != "" && this.lastLongPollMessage != null)
                        {
                            if (!new Regex("\\\"updates\\\"\\:\\[\\]").Match(this.lastLongPollMessage).Success)
                            {
                                this.longPollMessages.Add(this.lastLongPollMessage);
                                LongPollServerEventArgs args = new LongPollServerEventArgs();
                                args.LastEventId = this.LastEventId;
                                args.EventSourceCode = this.lastLongPollMessage;

                                context.Post(this.doOnDataReceived, args);
                            }
                        }

                    }
                }
            }
            catch(System.Net.WebException e)
            {
                switch (e.Status)
                {
                    case System.Net.WebExceptionStatus.Timeout:
                        {
                            if (!this.stopPending)
                            {
                                this.run(state);
                            }
                            break;
                        }
                }
            }
            //finally
            {
            }
        }

        private void doGetLongPollServerConnectionData(object data)
        {
            this.GetLongPollServerConnectionData();
        }

        private void doOnDataReceived(object data)
        {
            this.OnReceivedData((LongPollServerEventArgs)data);
        }

        private void doOnStopped(object data)
        {
            this.OnStopped();
        }

        private void doGetSendRequest(object data)
        {
            this.request.Timeout = this.WaitTime * 1000;
        }

        /// <summary>
        /// Stop listening server connection
        /// </summary>
        public void Stop()
        {
            if (this.Stopped != null)
            {
                this.Stopped(this);
            }
            lock (this.locker)
            {
                this.stopPending = true;
            }
            //this.connectionListenerThread.Join();
            //this.connectionListenerThread.Suspend();
        }

    }
}
