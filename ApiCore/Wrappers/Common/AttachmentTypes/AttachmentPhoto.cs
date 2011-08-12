using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.AttachmentTypes
{
    /// <summary>
    /// 
    /// </summary>
    public class AttachmentPhoto: AttachmentData
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id;
        /// <summary>
        /// 
        /// </summary>
        public int OwnerId;
        /// <summary>
        /// 
        /// </summary>
        public string ApplicationId;
        /// <summary>
        /// 
        /// </summary>
        public string ThumbnailUrl;
        /// <summary>
        /// 
        /// </summary>
        public string PictureUrl;
    }
}
