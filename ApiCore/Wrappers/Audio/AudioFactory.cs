using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Web;


namespace ApiCore.Audio
{
    public class AudioFactory: BaseFactory
    {
        private int searchSongsCount = 0;

        private bool isSearchRequest = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        public AudioFactory(ApiManager manager)
            : base(manager)
        {
            this.Manager = manager;
        }

        private AudioEntry buildAudioEntry(XmlNode node)
        {
            if (node != null)
            {
                XmlUtils.UseNode(node);
                AudioEntry a = new AudioEntry();
                a.Id = XmlUtils.Int("aid");
                a.OwnerId = XmlUtils.Int("owner_id");
                a.Duration = XmlUtils.Int("duration");
                a.Artist = XmlUtils.String("artist");
                a.Title = XmlUtils.String("title");
                a.Url = XmlUtils.String("url");
                return a;
            }
            return null;
        }

        private List<AudioEntry> buildList(XmlNode data)
        {
            XmlNodeList nodes = data.SelectNodes("audio");
            List<AudioEntry> audios = new List<AudioEntry>();
            foreach (XmlNode n in nodes)
            {
                audios.Add(this.buildAudioEntry(n));
            }
            return audios;
        }

        public List<AudioEntry> Get(int? userId, int? groupId, int[] audioIds)
        {
            this.Manager.Method("audio.get");
            if (userId != null)
            {
                this.Manager.Params("uid", userId);
            }
            if (groupId != null)
            {
                this.Manager.Params("gid", groupId);
            }
            if (audioIds != null)
            {
                this.Manager.Params("aids", string.Join(",", CommonUtils.IntArrayToString(audioIds)));
            }

            
            XmlNode result = this.Manager.Execute().GetResponseXml();
            if (this.Manager.MethodSuccessed)
            {
                return this.buildList(result);
            }
            return null;
        }

        public List<AudioEntry> Search(string query, AudioSortOrder order, bool withLyrics, int? count, int? offset)
        {
            this.Manager.Method("audio.search");
            this.Manager.Params("q", query);
            this.Manager.Params("sort", order);
            this.Manager.Params("lyrics", ((withLyrics) ? 1 : 0));
            if (count != null)
            {
                this.Manager.Params("count", count);
            }
            if (offset != null)
            {
                this.Manager.Params("offset", offset);
            }

            XmlNode result = this.Manager.Execute().GetResponseXml();

            XmlUtils.UseNode(result);
            this.searchSongsCount = XmlUtils.Int("count");
            return this.buildList(result);

        }

        public string GetUploadServer()
        {
            this.Manager.Method("audio.getUploadServer");

            XmlNode result = this.Manager.Execute().GetResponseXml();

                XmlUtils.UseNode(result);
                return XmlUtils.String("response/upload_url");

        }

        public AudioEntry Save(AudioUploadedInfo info)
        {
            this.Manager.Method("audio.save", new string[] {
                                            "server", info.Server,
                                            "audio", info.Audio,
                                            "hash", info.Hash,
                                            "artist", info.Artist,
                                            "title", info.Title});
            return this.buildAudioEntry(this.Manager.Execute().GetResponseXml());
        }

        public bool Reorder(int audioId, int before, int after, int? ownerId)
        {
            this.Manager.Method("audio.reorder");
            this.Manager.Params("aid", audioId);
            this.Manager.Params("before", before);
            this.Manager.Params("after", after);
            if (ownerId != null)
            {
                this.Manager.Params("oid", ownerId);
            }
            XmlNode result = this.Manager.Execute().GetResponseXml();

            XmlUtils.UseNode(result);
            return XmlUtils.BoolVal();

        }
    }
}
