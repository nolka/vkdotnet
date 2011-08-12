using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ApiCore
{
    public static class CommentsFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static CommentsInfo GetCommentsInfo(XmlNode node)
        {
            if (node != null)
            {
                XmlUtils.UseNode(node);
                CommentsInfo c = new CommentsInfo();
                c.Count = XmlUtils.Int("count");
                c.CanComment = XmlUtils.Bool("can_post");
                return c;
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static EntryComment GetEntryComment(XmlNode node)
        {
            XmlUtils.UseNode(node);
            EntryComment e = new EntryComment();
            e.Id = XmlUtils.Int("cid");
            e.Date = CommonUtils.FromUnixTime(XmlUtils.Int("date"));
            e.Text = XmlUtils.String("text");
            e.UserId = XmlUtils.Int("uid");
            e.ReplyToComment = XmlUtils.Int("reply_to_сid");
            e.ReplyToUser = XmlUtils.Int("reply_to_uid");
            return e;
        }
    }
}
