using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ApiCore.Photos
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

        private PhotoEntryFull buildPhotoEntryFull(XmlNode node)
        {
            if (node != null)
            {
                XmlUtils.UseNode(node);
                PhotoEntryFull photo = new PhotoEntryFull();
                photo.Id = XmlUtils.Int("pid");
                photo.AlbumId = XmlUtils.Int("aid");
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

        private PhotoEntryShort buildPhotoEntryShort(XmlNode node)
        {
            if (node != null)
            {
                XmlUtils.UseNode(node);
                PhotoEntryShort photo = new PhotoEntryShort();
                photo.Id = XmlUtils.Int("pid");
                photo.AlbumId = XmlUtils.Int("aid");
                photo.OwnerId = XmlUtils.Int("owner_id");
                photo.Url = XmlUtils.String("src");
                photo.UrlBig = XmlUtils.String("src_big");
                photo.UrlSmall = XmlUtils.String("src_small");
                photo.DateCreated = CommonUtils.FromUnixTime(XmlUtils.Int("created"));
                return photo;
            }
            return null;
        }

        private List<PhotoEntryFull> buildPhotosListFull(XmlNode data)
        {
            XmlNodeList nodes = data.SelectNodes("photo");
            if (nodes.Count > 0)
            {
                List<PhotoEntryFull> photos = new List<PhotoEntryFull>();
                foreach (XmlNode n in nodes)
                {
                    photos.Add(this.buildPhotoEntryFull(n));
                }
                return photos;
            }
            return null;
        }

        private List<PhotoEntryShort> buildPhotosListShort(XmlNode data)
        {
            XmlNodeList nodes = data.SelectNodes("photo");
            if (nodes.Count > 0)
            {
                List<PhotoEntryShort> photos = new List<PhotoEntryShort>();
                foreach (XmlNode n in nodes)
                {
                    photos.Add(this.buildPhotoEntryShort(n));
                }
                return photos;
            }
            return null;
        }

        private List<PhotoEntryTag> buildPhotoTagsList(XmlNode result)
        {
            XmlNodeList nodes = result.SelectNodes("tag");
            if (nodes.Count > 0)
            {
                List<PhotoEntryTag> tags = new List<PhotoEntryTag>();
                foreach (XmlNode n in nodes)
                {
                    XmlUtils.UseNode(n);
                    PhotoEntryTag tag = new PhotoEntryTag();
                    /**
                     * <tag>
                      <uid>5005272</uid>
                      <tag_id>2859378</tag_id>
                      <placer_id>5005272</placer_id>
                      <tagged_name>Алексей Харьков</tagged_name>
                      <date>1214309859</date>
                      <x>8.98</x>
                      <y>6.65</y>
                      <x2>39.01</x2>
                      <y2>64.45</y2>
                      <viewed>1</viewed>
                     </tag>
                     */
                    tag.Id = XmlUtils.Int("tag_id");
                    tag.UserId = XmlUtils.Int("uid");
                    tag.PlacerId = XmlUtils.Int("placer_id");
                    tag.Date = CommonUtils.FromUnixTime(XmlUtils.Int("date"));
                    tag.X = XmlUtils.Int("x");
                    tag.Y = XmlUtils.Int("y");
                    tag.X2 = XmlUtils.Int("x2");
                    tag.Y2 = XmlUtils.Int("y2");
                    tag.Viewed = XmlUtils.Int("viewed");
                    tags.Add(tag);
                }
                return tags;
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

        public List<PhotoEntryFull> GetPhotos(int userId, int albumId, int[] photoIds, int? count, int? offset)
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
                return this.buildPhotosListFull(result);
            }
            return null;
        }

        public List<PhotoEntryShort> GetAll(int? userId, int? count, int? offset)
        {
            this.Manager.Method("photos.getAll");
            if (userId != null)
            {
                this.Manager.Params("owner_id", userId);
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
                return this.buildPhotosListShort(result);
            }
            return null;
        }

        public List<PhotoEntryShort> GetUserPhotos(int? userId, int? count, int? offset)
        {
            this.Manager.Method("photos.getUserPhotos");
            if (userId != null)
            {
                this.Manager.Params("uid", userId);
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
                return this.buildPhotosListShort(result);
            }
            return null;
        }

        public List<PhotoEntryTag> GetTags(int photoId, int? ownerId)
        {
            this.Manager.Method("photos.getTags");
            this.Manager.Params("pid", photoId);
            if (ownerId != null)
            {
                this.Manager.Params("owner_id", ownerId);
            }

            XmlNode result = this.Manager.Execute().GetResponseXml();
            if (this.Manager.MethodSuccessed)
            {
                return this.buildPhotoTagsList(result);
            }
            return null;
        }

        public int PutTag(int? ownerId, int photoId, int userId, float x, float y, float x2, float y2)
        {
            this.Manager.Method("photos.putTag");
            this.Manager.Params("pid", photoId);
            this.Manager.Params("uid", userId);
            this.Manager.Params("x", x);
            this.Manager.Params("y", y);
            this.Manager.Params("x2", x2);
            this.Manager.Params("y2", y2);
            if (ownerId != null)
            {
                this.Manager.Params("owner_id", ownerId);
            }

            XmlNode result = this.Manager.Execute().GetResponseXml();
            if (this.Manager.MethodSuccessed)
            {
                XmlUtils.UseNode(result);
                return XmlUtils.IntVal();
            }
            return -1;
        }

        public bool RemoveTag(int? ownerId, int photoId, int tagId)
        {
            this.Manager.Method("photos.removeTag");
            this.Manager.Params("pid", photoId);
            this.Manager.Params("tag_id", tagId);
            if (ownerId != null)
            {
                this.Manager.Params("owner_id", ownerId);
            }

            XmlNode result = this.Manager.Execute().GetResponseXml();
            if (this.Manager.MethodSuccessed)
            {
                XmlUtils.UseNode(result);
                return XmlUtils.BoolVal();
            }
            return false;
        }

        public List<PhotoEntryFull> GetPhotosById(string[] photos)
        {
            this.Manager.Method("photos.getById");
            if (photos != null)
            {
                this.Manager.Params("photos", string.Join(",", photos));
            }
            XmlNode result = this.Manager.Execute().GetResponseXml();
            if (this.Manager.MethodSuccessed)
            {
                return this.buildPhotosListFull(result);
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

        public string GetWallUploadServer(int? userId, int? groupId)
        {
            this.Manager.Method("photos.getWallUploadServer", new object[] { "uid", userId, "gid", groupId });
            XmlNode result = this.Manager.Execute().GetResponseXml();
            if (this.Manager.MethodSuccessed)
            {
                XmlUtils.UseNode(result);
                return XmlUtils.String("upload_url");
            }
            return null;
        }

        public PhotoEntryFull SaveWallPhoto(PhotoUploadedInfo photoInfo, int? userId, int? groupId)
        {
            this.Manager.Method("photos.saveWallPhoto", new object[] { "server", photoInfo.Server,
                                                                        "photo", photoInfo.Photo,
                                                                        "hash", photoInfo.Hash,
                                                                        "uid", userId, "gid", groupId});
            XmlNode result = this.Manager.Execute().GetResponseXml();
            if (this.Manager.MethodSuccessed)
            {
                return this.buildPhotoEntryFull(result);
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
