using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ApiCore
{
    /// <summary>
    /// Represents an a Vkontakte activity manager, that can get, or set user activities and actoivities of her friends
    /// </summary>
    public class ActivityFactory: BaseFactory
    {

        public ActivityFactory(ApiManager manager)
            : base(manager)
        {
            this.Manager = manager;
        }

        private List<ActivityEntry> buildEntryList(XmlDocument x)
        {
            XmlNodeList msgsNodes = x.SelectNodes("/response/activity");
            if (msgsNodes.Count > 0)
            {
                List<ActivityEntry> msgList = new List<ActivityEntry>();
                foreach (XmlNode msgNode in msgsNodes)
                {
                    ActivityEntry act = new ActivityEntry();
                    act.Id = Convert.ToInt32(msgNode.SelectSingleNode("id").InnerText);
                    act.Text = msgNode.SelectSingleNode("text").InnerText;
                    act.Date = CommonUtils.FromUnixTime(msgNode.SelectSingleNode("created").InnerText);
                    msgList.Add(act);
                }
                return msgList;
            }
            return null;
        }

        /// <summary>
        /// Gets the activity history for current user or friend
        /// </summary>
        /// <param name="userId">if null - current user history was been returned, if specified - friend history was been returned</param>
        /// <returns>Activity history</returns>
        public List<ActivityEntry> GetHistory(int? userId)
        {
                this.Manager.Method("activity.getHistory");
                if (userId != null)
                {
                    this.Manager.Params("uid", userId);//((type == MessageType.Outgoing) ? "1" : "0"));;
                }
                string resp = this.Manager.Execute().GetResponseString();
                if (this.Manager.MethodSuccessed)
                {
                    XmlDocument x = this.Manager.GetXmlDocument(resp);
                    if (x.SelectSingleNode("/response").InnerText.Equals("0"))
                    {
                        return null;
                    }

                    return this.buildEntryList(x);
                }
                return null;
        }

        /// <summary>
        /// Gets the current activity 
        /// </summary>
        /// <param name="userId">if null - current user activity was been returned, if specified - friend activity was been returned</param>
        /// <returns>Current activity</returns>
        public ActivityEntry Get(int? userId)
        {
            this.Manager.Method("activity.get");
            if (userId != null)
            {
                this.Manager.Params("uid", userId);//((type == MessageType.Outgoing) ? "1" : "0"));;
            }
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                if (x.SelectSingleNode("/response").InnerText.Equals("0"))
                {
                    return null;
                }
                XmlNode actNode = x.SelectSingleNode("/response");
                ActivityEntry act = new ActivityEntry();
                act.Id = Convert.ToInt32(actNode.SelectSingleNode("id").InnerText);
                act.Text = actNode.SelectSingleNode("activity").InnerText;
                act.Date = CommonUtils.FromUnixTime(actNode.SelectSingleNode("time").InnerText);
                return act;
            }
            return null;
        }

        /// <summary>
        /// Sets the current user activity
        /// </summary>
        /// <param name="message">Activity message</param>
        /// <returns>activity id</returns>
        public int Set(string message)
        {
            this.Manager.Method("activity.set");
            this.Manager.Params("text", message);
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return Convert.ToInt32(x.SelectSingleNode("/response").InnerText);
            }
            return -1;
        }

    }
}
