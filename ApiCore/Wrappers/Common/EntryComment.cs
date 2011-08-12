using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore
{
    /// <summary>
    /// 
    /// </summary>
    public class EntryComment
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id;
        /// <summary>
        /// 
        /// </summary>
        public int UserId;
        /// <summary>
        /// 
        /// </summary>
        public int ReplyToUser;
        /// <summary>
        /// 
        /// </summary>
        public int ReplyToComment;
        /// <summary>
        /// 
        /// </summary>
        public DateTime Date;
        /// <summary>
        /// 
        /// </summary>
        public string Text;
    }
}
