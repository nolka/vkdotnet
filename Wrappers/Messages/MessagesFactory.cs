using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using System.Web;

namespace ApiCore
{    
    //private enum MessageType
    //{
    //    Incoming,
    //    Outgoing,
    //    History
    //}

    public class MessagesFactory: BaseFactory
    {

        public int IncomingMessagesCount = 0;
        public int OutgoingMessagesCount = 0;
        public int HistoryMessagesCount = 0;

        public MessagesFactory(ApiManager manager) : base(manager)
        {
            this.Manager = manager;
        }

        private List<Message> buildMessagesList(XmlDocument x, MessageType type)
        {
            XmlUtils.UseNode(x);
            int msgCount = XmlUtils.Int("/response/count");
            //int msgCount = Convert.ToInt32(x.SelectNodes("/response/count"));
            XmlNodeList msgsNodes = x.SelectNodes("/response/message");
            if (msgsNodes.Count > 0)
            {
                List<Message> msgList = new List<Message>();
                foreach (XmlNode msgNode in msgsNodes)
                {
                    
                    Message message = new Message();
                    message.Id = Convert.ToInt32(msgNode.SelectSingleNode("mid").InnerText);
                    message.Body = msgNode.SelectSingleNode("body").InnerText;
                    if (type == MessageType.History)
                    {
                        this.HistoryMessagesCount = msgCount;
                        message.UserId = Convert.ToInt32(msgNode.SelectSingleNode("from_id").InnerText);
                    }
                    else
                    {
                        switch(type)
                        {
                            case MessageType.Incoming: this.IncomingMessagesCount = msgCount; break;
                            case MessageType.Outgoing: this.OutgoingMessagesCount = msgCount; break;
                        }
                        message.Title = msgNode.SelectSingleNode("title").InnerText;
                        message.UserId = Convert.ToInt32(msgNode.SelectSingleNode("uid").InnerText);
                    }
                    message.Date = CommonUtils.FromUnixTime(msgNode.SelectSingleNode("date").InnerText);
                    message.State = (MessageState)Convert.ToInt32(msgNode.SelectSingleNode("read_state").InnerText);
                    msgList.Add(message);
                }
                return msgList;
            }
            return null;
        }

        /// <summary>
        /// Gets the personal messages list
        /// </summary>
        /// <param name="type">type of messages: inbox or outbox</param>
        /// <param name="filter">message filter. null allowed</param>
        /// <param name="offset">offset. null allowed</param>
        /// <param name="count">count messages to be returned. null allowed</param>
        /// <param name="previewLen">messages preview len. null allowed</param>
        /// <param name="timeOffset">messages time offset. null allowed</param>
        /// <returns>Messages list</returns>
        public List<Message> Get(MessageType type, MessageFilter? filter, int? offset, int? count, int? previewLen, int? timeOffset)
        {
            this.Manager.Method("messages.get");
            this.Manager.Params("out", (int)type);
            if (filter != null)
            {
                this.Manager.Params("filters", (int)filter);
            }
            if (offset != null)
            {
                this.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                this.Manager.Params("count", count);
            }
            if (previewLen != null)
            {
                this.Manager.Params("preview_length", previewLen);
            }
            if (timeOffset != null)
            {
                this.Manager.Params("time_offset", timeOffset);
            }
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                if (x.SelectSingleNode("/response").InnerText.Equals("0"))
                {
                    return null;
                }

                if (type == MessageType.Outgoing)
                {
                    this.OutgoingMessagesCount = Convert.ToInt32(x.SelectSingleNode("/response/count").InnerText);
                }
                else
                {
                    this.IncomingMessagesCount = Convert.ToInt32(x.SelectSingleNode("/response/count").InnerText);
                }
                switch (type)
                {
                    case MessageType.History: return this.buildMessagesList(x, MessageType.History); break;
                    case MessageType.Incoming: return this.buildMessagesList(x, MessageType.Incoming); break;
                    case MessageType.Outgoing: return this.buildMessagesList(x, MessageType.Outgoing); break;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the dialogs
        /// </summary>
        /// <param name="offset">offset. null allowed</param>
        /// <param name="count">count messages to be returned. null allowed</param>
        /// <param name="previewLen">messages preview len. null allowed</param>
        /// <returns>Messages list</returns>
        public List<Message> GetDialogs(int? offset, int? count, int? previewLen)
        {
            this.Manager.Method("messages.getDialogs");
            if (offset != null)
            {
                this.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                this.Manager.Params("count", count);
            }
            if (previewLen != null)
            {
                this.Manager.Params("preview_length", previewLen);
            }
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                if (x.SelectSingleNode("/response").InnerText.Equals("0"))
                {
                    return null;
                }

                return this.buildMessagesList(x, MessageType.Dialogs);
            }
            return null;
        }

        /// <summary>
        /// Gets the messages history
        /// </summary>
        /// <param name="userId">from user</param>
        /// <param name="offset">offset. null allowed</param>
        /// <param name="count">count messages to be returned. null allowed</param>
        /// <returns>Messages list</returns>
        public List<Message> GetHistory(int userId, int? offset, int? count)
        {
            this.Manager.Method("messages.getHistory");
            this.Manager.Params("uid", userId);
            if (offset != null)
            {
                this.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                this.Manager.Params("count", count);
            }
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                if (x.SelectSingleNode("/response").InnerText.Equals("0"))
                {
                    return null;
                }

                return this.buildMessagesList(x, MessageType.History);
            }
            return null;
        }

        /// <summary>
        /// Search the messages by query mask
        /// </summary>
        /// <param name="query">pattern to search</param>
        /// <param name="offset">offset. null allowed</param>
        /// <param name="count">count messages to be returned. null allowed</param>
        /// <returns>Messages list</returns>
        public List<Message> Search(string query, int? offset, int? count)
        {
            this.Manager.Method("messages.search");
            this.Manager.Params("q", query);
            if (offset != null)
            {
                this.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                this.Manager.Params("count", count);
            }
            string resp = this.Manager.Execute().GetResponseString(); 
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                if (x.SelectSingleNode("/response").InnerText.Equals("0"))
                {
                    return null;
                }

                return this.buildMessagesList(x, MessageType.Dialogs);
            }
            return null;
        }

        /// <summary>
        /// Send personal message
        /// </summary>
        /// <param name="userId">to user</param>
        /// <param name="message">message body</param>
        /// <param name="title">message title. null allowed</param>
        /// <param name="type">message type. null allowed</param>
        /// <returns>sent message id</returns>
        public int Send(int userId, string message, string title, SendMessageType? type)
        {
            this.Manager.Method("messages.send");
            this.Manager.Params("uid", userId);//((type == MessageType.Outgoing) ? "1" : "0"));
            this.Manager.Params("message", message);
            if (title != null)
            {
                this.Manager.Params("title", title);
            }
            if (type != null)
            {
                this.Manager.Params("type", (int)type);
            }
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return Convert.ToInt32(x.SelectSingleNode("/response").InnerText);
            }
            return -1;
        }

        /// <summary>
        /// Mark messages as UNREAD
        /// </summary>
        /// <param name="msg_ids">comma separated messages ids</param>
        /// <returns>true if ok, else false</returns>
        public bool MarkAsNew(string msg_ids)
        {
            this.Manager.Method("messages.markAsNew");
            this.Manager.Params("mids", msg_ids);
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return ((x.SelectSingleNode("/response").InnerText.Equals("1"))? true: false);
            }
            return false;
        }

        /// <summary>
        /// Mark messages as READ
        /// </summary>
        /// <param name="msg_ids">comma separated messages ids</param>
        /// <returns>true if ok, else false</returns>
        public bool MarkAsRead(string msg_ids)
        {
            this.Manager.Method("messages.markAsRead");
            this.Manager.Params("mids", msg_ids);
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return ((x.SelectSingleNode("/response").InnerText.Equals("1")) ? true : false);
            }
            return false;
        }

        /// <summary>
        /// Delete personal message
        /// </summary>
        /// <param name="msg_id">message id to delete</param>
        /// <returns>true if ok, else false</returns>
        public bool Delete(int msg_id)
        {
            this.Manager.Method("messages.delete");
            this.Manager.Params("mid", msg_id);
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return ((x.SelectSingleNode("/response").InnerText.Equals("1")) ? true : false);
            }
            return false;
        }

        /// <summary>
        /// Restore deleted personal message
        /// </summary>
        /// <param name="msg_id">message id to restore</param>
        /// <returns>true if ok, else false</returns>
        public bool Restore(int msg_id)
        {
            this.Manager.Method("messages.restore");
            this.Manager.Params("mid", msg_id);
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return ((x.SelectSingleNode("/response").InnerText.Equals("1")) ? true : false);
            }
            return false;
        }

    }
}
