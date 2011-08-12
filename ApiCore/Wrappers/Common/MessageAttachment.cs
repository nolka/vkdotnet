using System;
using System.Collections.Generic;
using System.Text;
using ApiCore.AttachmentTypes;

namespace ApiCore
{
    public class MessageAttachment
    {
        public AttachmentType Type;
        public int UserId;
        public int MediaId;

        public MessageAttachment(AttachmentType type, int userId, int mediaId)
        {
            this.Type = type;
            this.UserId = userId;
            this.MediaId = mediaId;
        }

        public string AttachmentTypeToString()
        {
            switch (this.Type)
            {
                case AttachmentType.Application:
                {
                    return "app";

                }
                case AttachmentType.Audio:
                {
                    return "audio";

                }
                case AttachmentType.Checkin:
                {
                    return "checkin";

                }
                case AttachmentType.Document:
                {
                    return "doc";

                }
                case AttachmentType.Graffiti:
                {
                    return "graffiti";

                }
                case AttachmentType.Note:
                {
                    return "note";

                }
                case AttachmentType.Photo:
                {
                    return "photo";

                }
                case AttachmentType.Poll:
                {
                    return "poll";

                }
                case AttachmentType.PostedPhoto:
                {
                    return "posted_photo";

                }
                case AttachmentType.Share:
                {
                    return "share";

                }
                case AttachmentType.Url:
                {
                    return "link";

                }
                case AttachmentType.Video:
                {
                    return "video";

                }
            }
            return "";
        }

        public override string ToString()
        {
            return String.Format("{0}{1}_{2}", this.AttachmentTypeToString(), this.UserId.ToString(), this.MediaId.ToString());
        }
    }
}
