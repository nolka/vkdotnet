using System;
using System.Collections.Generic;
using System.Text;
using ApiCore.AttachmentTypes;

namespace ApiCore
{
    public class Attachment
    {
        public AttachmentType Type;
        public int ItemId;
        public AttachmentData Data;
        //public int ApplicationId;
        //public string ThumbnailUrl;
        //public string ShareUrl;
        //public string ShareTitle;
    }

}
