using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.AttachmentTypes
{
    public  class AttachmentAudio: AttachmentData
    {
        public int Id;
        public int OwnerId;
        public string Performer;
        public string Title;
        public float Duration;
    }
}
