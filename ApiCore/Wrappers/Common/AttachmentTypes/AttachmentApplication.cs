using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.AttachmentTypes
{
    class AttachmentApplication: AttachmentData
    {
        public int Id;
        public string Name;
        public string ThumbnailUrl;
        public string PictureUrl;
    }
}
