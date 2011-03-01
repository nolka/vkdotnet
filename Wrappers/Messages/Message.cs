using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.Messages
{
    /// <summary>
    /// 
    /// </summary>
    public enum MessageType
    {
        Incoming = 0,
        Outgoing = 1,
        Spam = 2,
        History,
        Dialogs
    }

    /// <summary>
    /// 
    /// </summary>
    public enum MessageState
    {
        Read = 1,
        Unread = 0
    }

    /// <summary>
    /// 
    /// </summary>
    public enum MessageFilter
    {
        OnlyUnread = 1,
        NotFromChat = 2,
        OnlyFromFriends = 4
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SendMessageType
    {
        StandardMessage = 0,
        FromChat = 1
    }

    /// <summary>
    /// 
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Message id
        /// </summary>
        public int Id;
        /// <summary>
        /// User id
        /// </summary>
        public int UserId;
        /// <summary>
        /// Message title
        /// </summary>
        public string Title;
        /// <summary>
        /// Message body
        /// </summary>
        public string Body;
        /// <summary>
        /// Message date and time
        /// </summary>
        public DateTime Date;
        /// <summary>
        /// Message state
        /// </summary>
        public MessageState State;
    }
}
