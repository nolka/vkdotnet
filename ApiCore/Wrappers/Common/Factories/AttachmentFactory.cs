using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ApiCore.AttachmentTypes;

namespace ApiCore
{
    public static class AttachmentFactory
    {
        public static AttachmentData GetAttachment(AttachmentType type, XmlNode attachmentData)
        {
            XmlUtils.UseNode(attachmentData);
            switch (type)
            {
                case AttachmentType.Application:
                    {
                        AttachmentApplication a = new AttachmentApplication();
                        a.Id = XmlUtils.Int("app_id");
                        a.Name = XmlUtils.String("app_name");
                        a.PictureUrl = XmlUtils.String("src_big");
                        a.ThumbnailUrl = XmlUtils.String("src");
                        return a;
                    }

                case AttachmentType.Audio:
                    {
                        AttachmentAudio a = new AttachmentAudio();
                        a.Id = XmlUtils.Int("aid");
                        a.OwnerId = XmlUtils.Int("owner_id");
                        a.Performer = XmlUtils.String("performer");
                        a.Title = XmlUtils.String("title");
                        a.Duration = XmlUtils.Int("duration");
                        return a;
                    }

                case AttachmentType.Checkin:
                    {
                        break;
                    }
                    
                case AttachmentType.Graffiti:
                    {
                        AttachmentGraffiti a = new AttachmentGraffiti();
                        a.Id= XmlUtils.Int("gid");
                        a.OwnerId= XmlUtils.Int("owner_id");
                        a.PictureUrl = XmlUtils.String("src_big");
                        a.ThumbnailUrl = XmlUtils.String("src");
                        return a;
                    }

                case AttachmentType.Note: 
                    {
                        AttachmentNote a = new AttachmentNote();
                        a.Id = XmlUtils.Int("nid");
                        a.OwnerId = XmlUtils.Int("owner_id");
                        a.Title = XmlUtils.String("title");
                        a.CommentsCount = XmlUtils.Int("ncom");
                        return a;
                    }

                case AttachmentType.Photo:
                    {
                        AttachmentPhoto a = new AttachmentPhoto();
                        a.Id = XmlUtils.Int("pid");
                        a.OwnerId = XmlUtils.Int("owner_id");
                        a.PictureUrl = XmlUtils.String("src_big");
                        a.ThumbnailUrl = XmlUtils.String("src");
                        return a;
                    }

                case AttachmentType.PostedPhoto:
                    {
                        AttachmentPhoto a = new AttachmentPhoto();
                        a.Id = XmlUtils.Int("pid");
                        a.OwnerId = XmlUtils.Int("owner_id");
                        a.PictureUrl = XmlUtils.String("src_big");
                        a.ThumbnailUrl = XmlUtils.String("src");
                        return a;
                    }

                case AttachmentType.Poll:
                    {
                        AttachmentPoll a = new AttachmentPoll();
                        a.Question = XmlUtils.String("question");
                        return a;
                    }

                case AttachmentType.Share:
                    {
                        
                        break;
                    }

                case AttachmentType.Video:
                    {
                        AttachmentVideo a = new AttachmentVideo();
                        a.Id = XmlUtils.Int("vid");
                        a.OwnerId = XmlUtils.Int("owner_id");
                        a.Title = XmlUtils.String("title");
                        a.Duration = XmlUtils.Int("duration");
                        return a;
                    }

                case AttachmentType.Url:
                    {
                        AttachmentUrl a = new AttachmentUrl();
                        a.Url = XmlUtils.String("url");
                        a.Title = XmlUtils.String("title");
                        a.Description = XmlUtils.String("description");
                        a.ThumbnailUrl = XmlUtils.String("image_src");
                        return a;
                    }
            }
            return null;
        }
    }
}
