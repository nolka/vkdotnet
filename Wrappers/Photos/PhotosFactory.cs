using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ApiCore
{
    public class PhotosFactory: BaseFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        public PhotosFactory(ApiManager manager)
            : base(manager)
        {
            this.Manager = manager;
        }

        private AlbumEntry buildAlbumEntry(XmlNode node)
        {
            if (node != null)
            {
                XmlUtils.UseNode(node);
                AlbumEntry a = new AlbumEntry();
                a.Id = XmlUtils.Int("aid");
                a.ThumbnailId = XmlUtils.Int("thumb_id");
                a.OwnerId = XmlUtils.Int("owner_id");
                a.Title = XmlUtils.String("title");
                a.Description = XmlUtils.String("description");
                a.DateCreated = CommonUtils.FromUnixTime(XmlUtils.Int("created"));
                a.DateUpdated = CommonUtils.FromUnixTime(XmlUtils.Int("updated"));
                a.Size = XmlUtils.Int("size");
                a.Privacy = XmlUtils.Int("privacy");
                return a;
            }
            return null;
        }

        private List<AlbumEntry> buildAlbumsList(XmlNode data)
        {
            XmlNodeList nodes = data.SelectNodes("album");
            if (nodes.Count > 0)
            {
                List<AlbumEntry> albums = new List<AlbumEntry>();
                foreach (XmlNode n in nodes)
                {
                    albums.Add(this.buildAlbumEntry(n));
                }
                return albums;
            }
            return null;
        }

        private PhotoEntry buildPhotoEntry(XmlNode node)
        {
            if (node != null)
            {
                XmlUtils.UseNode(node);
                PhotoEntry photo = new PhotoEntry();
                photo.Id = XmlUtils.Int("aid");
                photo.OwnerId = XmlUtils.Int("owner_id");
                photo.Url = XmlUtils.String("src");
                photo.UrlBig = XmlUtils.String("src_big");
                photo.UrlSmall = XmlUtils.String("src_small");
                photo.UrlXBig = XmlUtils.String("src_xbig");
                photo.UrlXXBig = XmlUtils.String("src_xxbig");
                photo.Text = XmlUtils.String("text");
                photo.DateCreated = CommonUtils.FromUnixTime(XmlUtils.Int("created"));
                return photo;
            }
            return null;
        }

        private List<PhotoEntry> buildPhotosList(XmlNode data)
        {
            XmlNodeList nodes = data.SelectNodes("photo");
            if (nodes.Count > 0)
            {
                List<PhotoEntry> photos = new List<PhotoEntry>();
                foreach (XmlNode n in nodes)
                {
                    photos.Add(this.buildPhotoEntry(n));
                }
                return photos;
            }
            return null;
        }

        public List<AlbumEntry> GetAlbums(int? userId, int[] albums)
        {
            this.Manager.Method("photos.getAlbums");
            if (userId != null)
            {
                this.Manager.Params("uid", userId);
            }
            if (albums != null)
            {
                this.Manager.Params("aids", string.Join(",", CommonUtils.ArrayIntToString(albums)));
            }

            XmlNode result = this.Manager.Execute().GetResponseXml();
            if (this.Manager.MethodSuccessed)
            {
                return this.buildAlbumsList(result);
            }
            return null;
        }

        public List<PhotoEntry> GetPhotos(int userId, int albumId, int[] photoIds, int? count, int? offset)
        {
            this.Manager.Method("photos.get");
            this.Manager.Params("uid", userId);
            this.Manager.Params("aid", albumId);
            if (photoIds != null)
            {
                this.Manager.Params("pids", string.Join(",", CommonUtils.ArrayIntToString(photoIds)));
            }
            if (count != null)
            {
                this.Manager.Params("limit", count);
            }
            if (offset != null)
            {
                this.Manager.Params("offset", offset);
            }

            XmlNode result = this.Manager.Execute().GetResponseXml();
            if (this.Manager.MethodSuccessed)
            {
                return this.buildPhotosList(result);
            }
            return null;
        }

        public List<PhotoEntry> GetPhotosById(string[] photos)
        {
            this.Manager.Method("photos.getById");
            if (photos != null)
            {
                this.Manager.Params("photos", string.Join(",", photos));
            }
            XmlNode result = this.Manager.Execute().GetResponseXml();
            if (this.Manager.MethodSuccessed)
            {
                return this.buildPhotosList(result);
            }
            return null;
        }

        public AlbumEntry CreateAlbum(string title, AlbumAccessPrivacy access, AlbumCommentPrivacy comment, string description)
        {
            this.Manager.Method("photos.createAlbum");
            this.Manager.Params("title", title);
            this.Manager.Params("privacy", access);
            this.Manager.Params("comment_privacy", comment);
            if (description != null)
            {
                this.Manager.Params("description", description);
            }

            XmlNode result = this.Manager.Execute().GetResponseXml();
            if (this.Manager.MethodSuccessed)
            {
                return this.buildAlbumEntry(result.SelectSingleNode("album"));
            }
            return null;
        }

        public bool EditAlbum(int albumId, string title, AlbumAccessPrivacy access, AlbumCommentPrivacy comment, string description)
        {
            this.Manager.Method("photos.editAlbum");
            this.Manager.Params("aid", albumId);
            this.Manager.Params("title", title);
            this.Manager.Params("privacy", access);
            this.Manager.Params("comment_privacy", comment);
            if (description != null)
            {
                this.Manager.Params("description", description);
            }

            XmlNode result = this.Manager.Execute().GetResponseXml();
            if (this.Manager.MethodSuccessed)
            {
                XmlUtils.UseNode(result);
                return XmlUtils.BoolVal();
            }
            return false;
        }

        public PhotoUploadInfo GetUploadServer(int albumId, int? groupId, bool saveBig)
        {
            this.Manager.Method("photos.getUploadServer");
            this.Manager.Params("save_big", saveBig);
            if (groupId != null)
            {
                this.Manager.Params("gid", groupId);
            }

            XmlNode result = this.Manager.Execute().GetResponseXml();
            if (this.Manager.MethodSuccessed)
            {
                XmlUtils.UseNode(result);
                PhotoUploadInfo i = new PhotoUploadInfo();
                i.Url = XmlUtils.String("upload_url");
                i.AlbumId = XmlUtils.Int("aid");
                return i;
            }
            return null;
        }

        public bool EditPhoto(int userId, int photoid, string text)
        {
            this.Manager.Method("photos.edit");
            this.Manager.Params("uid", userId);
            this.Manager.Params("pid", photoid);
            this.Manager.Params("caption", text);
            XmlNode result = this.Manager.Execute().GetResponseXml();
            if (this.Manager.MethodSuccessed)
            {
                XmlUtils.UseNode(result);
                return XmlUtils.BoolVal();
            }
            return false;
        }

        public bool MovePhoto(int photoId, int toAlbumId, int? ownerId)
        {
            this.Manager.Method("photos.move");
            this.Manager.Params("pid", photoId);
            this.Manager.Params("target_aid", toAlbumId);
            if (ownerId != null)
            {
                this.Manager.Params("oid", ownerId);
            }
            XmlNode result = this.Manager.Execute().GetResponseXml();
            if (this.Manager.MethodSuccessed)
            {
                XmlUtils.UseNode(result);
                return XmlUtils.BoolVal();
            }
            return false;
        }

        public bool UsePhotoAsCover(int photoId, int albumId, int? ownerId)
        {
            this.Manager.Method("photos.makeCover");
            this.Manager.Params("pid", photoId);
            this.Manager.Params("aid", albumId);
            if (ownerId != null)
            {
                this.Manager.Params("oid", ownerId);
            }
            XmlNode result = this.Manager.Execute().GetResponseXml();
            if (this.Manager.MethodSuccessed)
            {
                XmlUtils.UseNode(result);
                return XmlUtils.BoolVal();
            }
            return false;
        }

        public bool ReorderAlbums(int albumId, int before, int after, int? ownerId)
        {
            this.Manager.Method("photos.reorderAlbums");
            this.Manager.Params("aid", albumId);
            this.Manager.Params("before", before);
            this.Manager.Params("after", after);
            if (ownerId != null)
            {
                this.Manager.Params("oid", ownerId);
            }
            XmlNode result = this.Manager.Execute().GetResponseXml();
            if (this.Manager.MethodSuccessed)
            {
                XmlUtils.UseNode(result);
                return XmlUtils.BoolVal();
            }
            return false;
        }

        public bool ReorderPhotos(int photoId, int before, int after, int? ownerId)
        {
            this.Manager.Method("photos.reorderAlbums");
            this.Manager.Params("pid", photoId);
            this.Manager.Params("before", before);
            this.Manager.Params("after", after);
            if (ownerId != null)
            {
                this.Manager.Params("oid", ownerId);
            }
            XmlNode result = this.Manager.Execute().GetResponseXml();
            if (this.Manager.MethodSuccessed)
            {
                XmlUtils.UseNode(result);
                return XmlUtils.BoolVal();
            }
            return false;
        }

    }
}
