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
    [ClassMetadata("/message")]
    public class Message
    {
        /// <summary>
        /// Message id
        /// </summary>
        [Metadata("mid")]
        public int Id;
        /// <summary>
        /// User id
        /// </summary>
        [Metadata("uid")]
        public int UserId;
        /// <summary>
        /// Message title
        /// </summary>
        [Metadata("title")]
        public string Title;
        /// <summary>
        /// Message body
        /// </summary>
        [Metadata("body")]
        public string Body;
        /// <summary>
        /// Message date and time
        /// </summary>
        [Metadata("date")]
        public DateTime Date;
        /// <summary>
        /// Message state
        /// </summary>
        [Metadata("read_state", CastTo = typeof(int))]
        public MessageState ReadState;

        [Metadata("attachments", CastTo = typeof(MessageAttachment), IsCollection = true)]
        public MessageAttachment[] Attachments;

        [Metadata("fwd_messages", CastTo = typeof(Message), IsCollection = true)]
        public Message[] ForwardMessages;

        private int chat_id;
        [Metadata("chat_id")]
        public int ChatId {
            get { return this.chat_id; }
            set { this.chat_id = value; }
        }

        [Metadata("chat_active", CastTo = typeof(int))]
        public int ChatActive;

        [Metadata("users_count")]
        public int UsersCount;

        [Metadata("admin_id")]
        public int AdminId;

        public override string ToString()
        {
            return this.Title + " " + ((this.Body.Length > 20) ? this.Body.Substring(0, 20) : this.Body);
        }
    }


}
