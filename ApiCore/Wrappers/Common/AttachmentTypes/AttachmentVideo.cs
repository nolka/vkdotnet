using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.AttachmentTypes
{
    public class AttachmentVideo: AttachmentData
    {
        public int Id;
        public int OwnerId;
        public string Title;
        public float Duration;
    }
}
