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
            this.Manager.Method("photos.getAlbums", new object[]{"uid", userId, 
                                                                "aids", string.Join(",", CommonUtils.IntArrayToString(albums)) });
            XmlNode result = this.Manager.Execute().GetResponseXml();

            return this.buildAlbumsList(result);
        }

        public List<PhotoEntryFull> GetPhotos(int userId, int albumId, int[] photoIds, int? count, int? offset)
        {
            this.Manager.Method("photos.get", new object[] { "uid", userId, 
                                                            "aid", albumId, 
                                                            "pids", string.Join(",", CommonUtils.IntArrayToString(photoIds)),
                                                            "limit", count,
                                                            "offset", offset});
            XmlNode result = this.Manager.Execute().GetResponseXml();

            return this.buildPhotosListFull(result);
        }

        public List<PhotoEntryShort> GetAll(int? userId, int? count, int? offset)
        {
            this.Manager.Method("photos.getAll", new object[] { "owner_id", userId, "limit", count, "offset", offset });
            XmlNode result = this.Manager.Execute().GetResponseXml();

            return this.buildPhotosListShort(result);
        }

        public List<PhotoEntryShort> GetUserPhotos(int? userId, int? count, int? offset)
        {
            this.Manager.Method("photos.getUserPhotos", new object[] { "uid", userId, "limit", count, "offset", offset });
            XmlNode result = this.Manager.Execute().GetResponseXml();

            return this.buildPhotosListShort(result);
        }

        public List<PhotoEntryTag> GetTags(int photoId, int? ownerId)
        {
            this.Manager.Method("photos.getTags", new object[] { "pid", photoId, "owner_id", ownerId });
            XmlNode result = this.Manager.Execute().GetResponseXml();

            return this.buildPhotoTagsList(result);
        }

        public int PutTag(int? ownerId, int photoId, int userId, float x, float y, float x2, float y2)
        {
            this.Manager.Method("photos.putTag", new object[] { "pid", photoId, "uid", userId, "x", x, "y", y, "x2", x2, "y2", y2, "owner_id", ownerId });
            XmlNode result = this.Manager.Execute().GetResponseXml();
            XmlUtils.UseNode(result);

            return XmlUtils.IntVal();
        }

        public bool RemoveTag(int? ownerId, int photoId, int tagId)
        {
            this.Manager.Method("photos.removeTag", new object[] { "pid", photoId, "tag_id", tagId, "owner_id", ownerId });
            XmlNode result = this.Manager.Execute().GetResponseXml();
            XmlUtils.UseNode(result);

            return XmlUtils.BoolVal();
        }

        public List<PhotoEntryFull> GetPhotosById(string[] photos)
        {
            this.Manager.Method("photos.getById", new object[] { "photos", string.Join(",", photos) });
            XmlNode result = this.Manager.Execute().GetResponseXml();

            return this.buildPhotosListFull(result);
        }

        public AlbumEntry CreateAlbum(string title, AlbumAccessPrivacy access, AlbumCommentPrivacy comment, string description)
        {
            this.Manager.Method("photos.createAlbum", new object[] { "title", title, "privacy", access, "comment_privacy", comment, "description", description });

            return this.buildAlbumEntry(this.Manager.Execute().GetResponseXml().SelectSingleNode("album"));
        }

        public bool EditAlbum(int albumId, string title, AlbumAccessPrivacy access, AlbumCommentPrivacy comment, string description)
        {
            this.Manager.Method("photos.editAlbum", new object[] { "aid", albumId, "title", title, "privacy", access, "comment_privacy", comment, "description", description });
            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml());

            return XmlUtils.BoolVal();
        }

        public PhotoUploadInfo GetUploadServer(int albumId, int? groupId, bool saveBig)
        {
            this.Manager.Method("photos.getUploadServer", new object[] { "save_big", saveBig, "gid", groupId });
            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml());
            PhotoUploadInfo i = new PhotoUploadInfo();
            i.Url = XmlUtils.String("upload_url");
            i.AlbumId = XmlUtils.Int("aid");

            return i;
        }

        public string GetWallUploadServer(int? userId, int? groupId)
        {
            this.Manager.Method("photos.getWallUploadServer", new object[] { "uid", userId, "gid", groupId });
            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml());

            return XmlUtils.String("upload_url");
        }

        public PhotoEntryFull SaveWallPhoto(PhotoUploadedInfo photoInfo, int? userId, int? groupId)
        {
            this.Manager.Method("photos.saveWallPhoto", new object[] { "server", photoInfo.Server,
                                                                        "photo", photoInfo.Photo,
                                                                        "hash", photoInfo.Hash,
                                                                        "uid", userId, "gid", groupId});

            return this.buildPhotoEntryFull(this.Manager.Execute().GetResponseXml());
        }

        public bool EditPhoto(int userId, int photoid, string text)
        {
            this.Manager.Method("photos.edit", new object[] { "uid", userId, "pid", photoid, "caption", text });
            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml());

            return XmlUtils.BoolVal();
        }

        public bool MovePhoto(int photoId, int toAlbumId, int? ownerId)
        {
            this.Manager.Method("photos.move", new object[] { "pid", photoId, "target_aid", toAlbumId, "oid", ownerId });
            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml());

            return XmlUtils.BoolVal();
        }

        public bool UsePhotoAsCover(int photoId, int albumId, int? ownerId)
        {
            this.Manager.Method("photos.makeCover", new object[] { "pid", photoId, "aid", albumId, "oid", ownerId });
            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml();

            return XmlUtils.BoolVal();
        }

        public bool ReorderAlbums(int albumId, int before, int after, int? ownerId)
        {
            this.Manager.Method("photos.reorderAlbums", new object[] { "aid", albumId, "before", before, "after", after, "oid", ownerId });
            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml());

            return XmlUtils.BoolVal();
        }

        public bool ReorderPhotos(int photoId, int before, int after, int? ownerId)
        {
            this.Manager.Method("photos.reorderAlbums", new object[] { "pid", photoId, "before", before, "after", after, "oid", ownerId });
            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml());

            return XmlUtils.BoolVal();
        }

    }
}
