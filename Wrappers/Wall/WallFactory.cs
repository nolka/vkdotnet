using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ApiCore.AttachmentTypes;

namespace ApiCore
{
    public class WallFactory: BaseFactory
    {
        /// <summary>
        /// 
        /// </summary>
        public int PostsCount = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        public WallFactory(ApiManager manager) : base(manager)
        {
            this.Manager = manager;
        }

        private Attachment getAttachment(XmlNode fromPost)
        {
            
            if (fromPost != null)
            {
                Attachment attachment = new Attachment();
                string attachmentType = fromPost.SelectSingleNode("type").InnerText;
                switch (attachmentType)
                {
                    case "app": attachment.Type = AttachmentType.Application; break;
                    case "graffiti": attachment.Type = AttachmentType.Graffiti; break;
                    case "video": attachment.Type = AttachmentType.Video; break;
                    case "audio": attachment.Type = AttachmentType.Audio; break;
                    case "photo": attachment.Type = AttachmentType.Photo; break;
                    case "posted_photo": attachment.Type = AttachmentType.PostedPhoto; break;
                    case "note": attachment.Type = AttachmentType.Note; break;
                    case "poll": attachment.Type = AttachmentType.Poll; break;
                    case "link": attachment.Type = AttachmentType.Url; break;
                    case "checkin": attachment.Type = AttachmentType.Checkin; break;
                    case "share": attachment.Type = AttachmentType.Share; break;
                }
                XmlNode attachmentData = fromPost.SelectSingleNode(attachmentType);
                if(attachmentData != null)
                {
                    attachment.Data = AttachmentFactory.GetAttachment(attachment.Type, attachmentData);
                }
                else
                {
                    attachment.Data = null;
                }
                //attachment.ItemId = Convert.ToInt32(fromPost.SelectSingleNode("item_id").InnerText);
                //if (attachment.Type != AttachmentType.Graffiti)
                //{
                //    attachment.OwnerId = Convert.ToInt32(fromPost.SelectSingleNode("owner_id").InnerText);
                //}
                //if (attachment.Type == AttachmentType.Application)
                //{
                //    attachment.ApplicationId = Convert.ToInt32(fromPost.SelectSingleNode("app_id").InnerText);
                //}
                //if (fromPost.SelectSingleNode("thumb_src") != null)
                //{
                //    attachment.ThumbnailUrl = fromPost.SelectSingleNode("thumb_src").InnerText;
                //}
                return attachment;
            }
            return null;            
        }

        private AttachmentData getAttachmentData(AttachmentType type, XmlNode attachmentData)
        {
            return AttachmentFactory.GetAttachment(type, attachmentData);
        }

        private List<WallEntry> buildEntryList(XmlDocument x)
        {
            XmlNodeList msgsNodes = x.SelectNodes("/response/post");
            if (msgsNodes != null)
            {
                List<WallEntry> msgList = new List<WallEntry>();
                foreach (XmlNode msgNode in msgsNodes)
                {
                    XmlUtils.UseNode(msgNode);
                    WallEntry wall = new WallEntry();
                    wall.Id = XmlUtils.Int("id");
                    wall.Body = XmlUtils.String("text");
                    wall.FromUser = XmlUtils.Int("from_id");
                    wall.ToUser = XmlUtils.Int("to_id");
                    wall.Date = CommonUtils.FromUnixTime(XmlUtils.String("date"));
                    wall.Online = ((XmlUtils.String("online"))=="1"? true: false);
                    wall.Attachment = this.getAttachment(msgNode.SelectSingleNode("attachment"));
                    wall.CopyOwnerId = XmlUtils.Int("copy_owner_id");
                    wall.CopyPostId = XmlUtils.Int("copy_post_id");
                    wall.LikesInfo = LikesFactory.GetLikesInfo(msgNode.SelectSingleNode("likes"));
                    wall.CommentsInfo = CommentsFactory.GetCommentsInfo(msgNode.SelectSingleNode("comments"));
                    if(XmlUtils.Int("reply_count")!= -1)
                    {
                        wall.RepliesCount = XmlUtils.Int("reply_count");
                    }
                    msgList.Add(wall);
                }
                return msgList;
            }
            return null;
        }

        /// <summary>
        /// Get wall entries
        /// </summary>
        /// <param name="ownerId">if null - wall entries for current user will be returned, else - for specified user</param>
        /// <param name="count">messages count. null allowed</param>
        /// <param name="offset">message offset. null allowed</param>
        /// <returns>Wall entries list or null</returns>
        public List<WallEntry> Get( int? ownerId, int? count, int? offset, string filter)
        {
            this.Manager.Method("wall.get");
            if (ownerId != null)
            {
                this.Manager.Params("owner_id", ownerId);
            }
            if (offset != null)
            {
                this.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                this.Manager.Params("count", count);
            }
            if (filter != null)
            {
                this.Manager.Params("filter", filter);
            }
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                if (x.SelectSingleNode("/response").InnerText.Equals("0"))
                {
                    return null;
                }

                this.PostsCount = Convert.ToInt32(x.SelectSingleNode("/response/count").InnerText);

                return this.buildEntryList(x);
            }
            return null;
        }

        /// <summary>
        /// Post message in the wall
        /// </summary>
        /// <param name="ownerId">if null - wall entry for current user will be posted, else - for specified user</param>
        /// <param name="message">message to be posted</param>
        /// <returns>message id</returns>
        public int Post(int? ownerId, string message)
        {
            this.Manager.Method("wall.post");
            if (ownerId != null)
            {
                this.Manager.Params("owner_id", ownerId);//((type == MessageType.Outgoing) ? "1" : "0"));;
            }
            this.Manager.Params("message", message);
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return Convert.ToInt32(x.SelectSingleNode("/response").InnerText);
            }
            return -1;
        }

        /// <summary>
        /// Delete wall post
        /// </summary>
        /// <param name="ownerId">if null - wall entry for current user will be deleted, else - for specified user</param>
        /// <param name="msg_id">post id to delete</param>
        /// <returns>true if all ok, else false</returns>
        public bool Delete(int? ownerId, int msg_id)
        {
            this.Manager.Method("wall.delete");
            if (ownerId != null)
            {
                this.Manager.Params("owner_id", ownerId);//((type == MessageType.Outgoing) ? "1" : "0"));;
            }
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
        /// Restore deleted entry
        /// </summary>
        /// <param name="ownerId">if null - wall entry for current user will be restored, else - for specified user</param>
        /// <param name="msg_id">post id to restore</param>
        /// <returns>true if all ok, else false</returns>
        public bool Restore(int? ownerId, int msg_id)
        {
            this.Manager.Method("wall.restore");
            if (ownerId != null)
            {
                this.Manager.Params("owner_id", ownerId);//((type == MessageType.Outgoing) ? "1" : "0"));;
            }
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
        /// 
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="needPublish"></param>
        /// <returns></returns>
        public int AddLike(int postId, bool needPublish)
        {
            this.Manager.Method("wall.addLike");
            this.Manager.Params("post_id", postId);
            if (needPublish)
            {
                this.Manager.Params("need_publish", needPublish);
            }
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                XmlUtils.UseNode(x.SelectSingleNode("/response"));
                return XmlUtils.Int("likes");
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public int DeleteLike(int postId)
        {
            this.Manager.Method("wall.deleteLike");
            this.Manager.Params("post_id", postId);
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                XmlUtils.UseNode(x.SelectSingleNode("/response"));
                return XmlUtils.Int("likes");
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="postId"></param>
        /// <param name="sortOrder"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<EntryComment> GetComments(int? ownerId, int postId, SortOrder sortOrder, int? offset, int? count)
        {
            this.Manager.Method("wall.getComments");
            if (ownerId != null)
            {
                this.Manager.Params("owner_id", ownerId);//((type == MessageType.Outgoing) ? "1" : "0"));;
            }
            this.Manager.Params("post_id", postId);
            if (sortOrder != null)
            {
                this.Manager.Params("sort", ((sortOrder == SortOrder.Asc)?"asc":"desc"));
            }
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
                XmlNodeList nodes = x.SelectNodes("/response/comment");
                if (nodes.Count > 0)
                {
                    List<EntryComment> comments = new List<EntryComment>();
                    foreach (XmlNode node in nodes)
                    {
                        EntryComment c = CommentsFactory.GetEntryComment(node);
                        comments.Add(c);
                    }
                    return comments;
                }
                return null;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="postId"></param>
        /// <param name="msg"></param>
        /// <param name="replyToCid"></param>
        /// <returns></returns>
        public int AddComment(int? ownerId, int postId, string msg, int? replyToCid)
        {
            this.Manager.Method("wall.addComment");
            if (ownerId != null)
            {
                this.Manager.Params("owner_id", ownerId);//((type == MessageType.Outgoing) ? "1" : "0"));;
            }
            this.Manager.Params("post_id", postId);
            this.Manager.Params("text", msg);
            if (replyToCid != null)
            {
                this.Manager.Params("reply_to_cid", replyToCid);
            }
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                XmlUtils.UseNode(x.SelectSingleNode("/response"));
                return XmlUtils.Int("cid");
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public bool DeleteComment(int? ownerId, int commentId)
        {
            this.Manager.Method("wall.deleteComment");
            if (ownerId != null)
            {
                this.Manager.Params("owner_id", ownerId);//((type == MessageType.Outgoing) ? "1" : "0"));;
            }
            this.Manager.Params("cid", commentId);
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                XmlUtils.UseNode(x);
                return XmlUtils.Bool("response");
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public bool RestoreComment(int? ownerId, int commentId)
        {
            this.Manager.Method("wall.restoreComment");
            if (ownerId != null)
            {
                this.Manager.Params("owner_id", ownerId);//((type == MessageType.Outgoing) ? "1" : "0"));;
            }
            this.Manager.Params("cid", commentId);
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                XmlUtils.UseNode(x);
                return XmlUtils.Bool("response");
            }
            return false;
        }
    }
}
