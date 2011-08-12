using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.Wall
{
    public class WallEntry
    {
        /// <summary>
        /// Entry id
        /// </summary>
        public int Id;
        /// <summary>
        /// Entry posted from user
        /// </summary>
        public int FromUser;
        /// <summary>
        /// Entry posted to user
        /// </summary>
        public int ToUser;
        /// <summary>
        /// Poster is online
        /// </summary>
        public bool Online;
        /// <summary>
        /// Entry text
        /// </summary>
        public string Body;
        /// <summary>
        /// If some media attached to the wall entry, it will be here
        /// </summary>
        public Attachment Attachment;
        /// <summary>
        /// Entry post date and time
        /// </summary>
        public DateTime Date;
        /// <summary>
        /// 
        /// </summary>
        public GeoInfo GeoInfo;
        /// <summary>
        /// 
        /// </summary>
        public int CopyOwnerId;
        /// <summary>
        /// 
        /// </summary>
        public int CopyPostId;
        /// <summary>
        /// 
        /// </summary>
        public CommentsInfo CommentsInfo;
        /// <summary>
        /// 
        /// </summary>
        public LikesInfo LikesInfo;
        /// <summary>
        /// 
        /// </summary>
        public int RepliesCount;

        public override string ToString()
        {
            return this.Body;
        }

    }
}
