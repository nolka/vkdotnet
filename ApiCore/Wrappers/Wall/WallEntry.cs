using System;
using System.Collections.Generic;
using System.Text;
using ApiCore.Utils.Mapper;

namespace ApiCore.Wall
{
    [ClassMetadata("post")]
    public class WallEntry
    {
        /// <summary>
        /// Entry id
        /// </summary>
        [MapperInfo("id", CastTo = typeof (int))] public int Id;
        /// <summary>
        /// Entry posted from user
        /// </summary>
        [MapperInfo("from_id", CastTo = typeof(int))]
        public int FromUser;
        /// <summary>
        /// Entry posted to user
        /// </summary>
        [MapperInfo("to_id", CastTo = typeof(int))]
        public int ToUser;
        /// <summary>
        /// Entry text
        /// </summary>
        [MapperInfo("text")]
        public string Body;
        /// <summary>
        /// If some media attached to the wall entry, it will be here
        /// </summary>
        public MessageAttachment[] Attachments;
        /// <summary>
        /// Entry post date and time
        /// </summary>
        [MapperInfo("date")]
        public DateTime Date;
        /// <summary>
        /// 
        /// </summary>
        public GeoInfo GeoInfo;
        /// <summary>
        /// 
        /// </summary>
        [MapperInfo("copy_owner_id", CastTo = typeof(int))]
        public int CopyOwnerId;
        /// <summary>
        /// 
        /// </summary>
        [MapperInfo("copy_post_id", CastTo = typeof(int))]
        public int CopyPostId;
        /// <summary>
        /// 
        /// </summary>
        public CommentsInfo CommentsInfo;
        /// <summary>
        /// 
        /// </summary>
        public LikesInfo LikesInfo;

        public override string ToString()
        {
            return this.Body;
        }

    }
}
