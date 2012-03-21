using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using System.Web;

using ApiCore;

namespace ApiCore.Messages
{
    //private enum MessageType
    //{
    //    Incoming,
    //    Outgoing,
    //    History
    //}

    public class MessagesFactory : BaseFactory
    {

        public int IncomingMessagesCount = 0;
        public int OutgoingMessagesCount = 0;
        public int HistoryMessagesCount = 0;

        public MessagesFactory(ApiManager manager)
            : base(manager)
        {
            this.Manager = manager;
        }

        private List<Message> buildMessagesList(XmlDocument x, MessageType type)
        {
            XmlUtils.UseNode(x);
            int msgCount = XmlUtils.Int("/response/count");
            //int msgCount = Convert.ToInt32(x.SelectNodes("/response/count"));
            XmlNodeList msgsNodes = x.SelectNodes("/response/message");
            // если количество сообщений больше 0, работаем
            if (msgsNodes.Count > 0)
            {
                // создали новый список, в котором будут храниться сообщения
                List<Message> msgList = new List<Message>();
                // пробегаем по коллекции нод с сообщениями в xml
                foreach (XmlNode msgNode in msgsNodes)
                {
                    // используем текущую ноду волшебным XmlUtils )
                    XmlUtils.UseNode(msgNode);
                    Message message = new Message();
                    message.Id = XmlUtils.Int("mid"); //раньше было так: Convert.ToInt32(msgNode.SelectSingleNode("mid").InnerText);
                    message.Title = XmlUtils.String("title"); //msgNode.SelectSingleNode("title").InnerText;
                    message.Body = XmlUtils.String("body"); // раньше было так: msgNode.SelectSingleNode("body").InnerText;
                    // если указан тип желаемых сообщений, продолжаем
                    if (type != null)
                    {
                        // если историческое сообщение
                        if (type == MessageType.History)
                        {
                            // зачем то устанавливаем внутри фабрики значение счетчика
                            // TODO: разобраться, нахуйа это надо :D
                            this.HistoryMessagesCount = msgCount;
                            message.UserId = XmlUtils.Int("from_id");//Convert.ToInt32(msgNode.SelectSingleNode("from_id").InnerText);
                        }
                        else
                        {
                            // иначе если входящее или исходящее сообщение.
                            // зачем то устанавливаются внутри фабрики значения о количествве входящих
                            // или исходящих сообщений.
                            // бля, чо я курил когда писал это?
                            switch (type)
                            {
                                case MessageType.Incoming:
                                this.IncomingMessagesCount = msgCount;
                                break;
                                case MessageType.Outgoing:
                                this.OutgoingMessagesCount = msgCount;
                                break;
                            }
                            message.Title = XmlUtils.String("title"); //msgNode.SelectSingleNode("title").InnerText;
                            message.UserId = XmlUtils.Int("uid"); //Convert.ToInt32(msgNode.SelectSingleNode("uid").InnerText);
                        }
                    }
                    // делаем рутиную работу про преобразованию xml в объекты
                    message.Date = CommonUtils.FromUnixTime(XmlUtils.Int("date")); // CommonUtils.FromUnixTime(msgNode.SelectSingleNode("date").InnerText);
                    message.State = (MessageState)XmlUtils.Int("read_state"); //(MessageState)Convert.ToInt32(msgNode.SelectSingleNode("read_state").InnerText);
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
            this.Manager.Method("messages.get",
                                new object[] { "out", (int)type,  
                                    "offset", offset, 
                                    "count", count, 
                                    "preview_length", previewLen, 
                                    "time_offset", timeOffset });
            if (filter != null)
            {
                this.Manager.Params("filters", (int)filter);
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
                    case MessageType.History:
                    return this.buildMessagesList(x, MessageType.History);
                    //break;
                    case MessageType.Incoming:
                    return this.buildMessagesList(x, MessageType.Incoming);
                    //break;
                    case MessageType.Outgoing:
                    return this.buildMessagesList(x, MessageType.Outgoing);
                    //break;
                }
            }
            return null;
        }

        public Message GetById(int msgId, int? previewLen)
        {
            this.Manager.Method("messages.getById",
                                   new object[] { "mid", msgId});
            this.Manager.Execute();
            XmlNode resp = this.Manager.GetResponseXml();

            return null;
        }

        public List<Message> GetById(int[] msgIds, int? previewLen)
        {
            this.Manager.Method("messages.getById",
                                   new object[] { "mids", CommonUtils.ArrayIntToCommaSeparatedString(msgIds) });
            this.Manager.Execute();
            XmlNode resp = this.Manager.GetResponseXml();

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
            this.Manager.Method("messages.getDialogs", 
                                    new object[] { "offset", offset, 
                                        "count", count, 
                                        "preview_length", previewLen });

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

        public bool DeleteDialogs(int dialogId, bool isChat, int? offset, int? limit)
        {
            this.Manager.Method("messages.deleteDialog", new object[]{
                (isChat)?"chat_id": "uid", 
                "offset", offset,
                "limit", limit
            });
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                XmlUtils.UseNode(x.SelectSingleNode("/response"));
                return XmlUtils.BoolVal();
            }
            return false;
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
            this.Manager.Method("messages.getHistory", 
                                new object[] { "uid", userId, 
                                    "offset", offset, 
                                    "count", count });

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
            this.Manager.Method("messages.search", 
                                    new object[] { "q", query, 
                                        "offset", offset, 
                                        "count", count });

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
        /// <param name="userId">user id if no chat id presented</param>
        /// <param name="chatId">chat id if no user id presented</param>
        /// <param name="title">message title</param>
        /// <param name="message">message body</param>
        /// <param name="attachment">attachment</param>
        /// <param name="forwardMessages">list of messages ids to forwarding</param>
        /// <param name="type">message type</param>
        /// <returns>id of message that was sended</returns>
        public int Send(int? userId, int? chatId, string title, string message, MessageAttachment attachment, int?[] forwardMessages, SendMessageType? type)
        {
            this.Manager.Method("messages.send",
                                    new object[] { "uid", userId,
                                        "chat_id", chatId,
                                        "title", title,
                                        "message", message,
                                        "attachment", attachment,
                                        "forward_messages", forwardMessages,
                                        "type", (int)type });
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return Convert.ToInt32(x.SelectSingleNode("/response").InnerText);
            }
            return -1;
        }

        /// <summary>
        /// Send personal message
        /// </summary>
        /// <param name="userId">to user</param>
        /// <param name="message">message body</param>
        /// <param name="title">message title. null allowed</param>
        /// <param name="type">message type. null allowed</param>
        /// <returns>sent message id</returns>
        public int Send(int userId, string message, string title, SendMessageType type)
        {
            return this.Send(userId, null, title, message, null, null, type);
        }

        public int Send(int userId, string message, string title, MessageAttachment attachment)
        {
            return this.Send(userId, null, title, message, attachment, null, SendMessageType.StandardMessage);
        }


        /// <summary>
        /// Mark messages as UNREAD
        /// </summary>
        /// <param name="msg_ids">comma separated messages ids</param>
        /// <returns>true if ok, else false</returns>
        public bool MarkAsNew(int[] msg_ids)
        {
            this.Manager.Method("messages.markAsNew",
                                new object[] 
                                { 
                                    "mids", String.Join(",", Array.ConvertAll<int, string>( msg_ids, Convert.ToString)) 
                                });
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return ((x.SelectSingleNode("/response").InnerText.Equals("1")) ? true : false);
            }
            return false;
        }

        /// <summary>
        /// Mark messages as READ
        /// </summary>
        /// <param name="msg_ids">comma separated messages ids</param>
        /// <returns>true if ok, else false</returns>
        public bool MarkAsRead(int[] msg_ids)
        {
            this.Manager.Method("messages.markAsRead",
                                new object[] 
                                { 
                                    "mids", String.Join(",", Array.ConvertAll<int, string>( msg_ids, Convert.ToString)) 
                                });
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
            this.Manager.Method("messages.delete", new object[] { "mid", msg_id });
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
            this.Manager.Method("messages.restore", new object[] { "mid", msg_id });
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return ((x.SelectSingleNode("/response").InnerText.Equals("1")) ? true : false);
            }
            return false;
        }

        /// <summary>
        /// Create LongPoll connection unit
        /// </summary>
        /// <returns>LongPollServerConnection unit</returns>
        public LongPollServerConnection GetLongPollServerConnection()
        {
            ApiCore.Messages.LongPollServerConnection connection = new LongPollServerConnection(this.Manager);
            return connection;
        }

        /// <summary>
        /// Create LongPoll connection unit
        /// </summary>
        /// <returns>LongPollServerConnection unit</returns>
        public LongPollServerConnection GetLongPollServerConnection(int waitTime)
        {
            ApiCore.Messages.LongPollServerConnection connection = new LongPollServerConnection(this.Manager);
            connection.WaitTime = waitTime;
            return connection;
        }

    }
}
