using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.AttachmentTypes
{
    public class AttachmentNote: AttachmentData
    {
        public int Id;
        public int OwnerId;
        public string Title;
        public int CommentsCount;
    }
}
