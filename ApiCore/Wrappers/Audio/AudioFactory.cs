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
                a.Genre = XmlUtils.Int("genre");
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
        
        /// <summary>
        /// Возвращает список аудиозаписей в соответствии с заданным критерием поиска
        /// </summary>
        /// <param name="query">текст поискового запроса</param>
        /// <param name="order">Вид сортировки</param>
        /// <param name="autoComplete">Если этот параметр равен 1, возможные ошибки в поисковом запросе будут исправлены</param>
        /// <param name="withLyrics">Если этот параметр равен 1, поиск будет производиться только по тем аудиозаписям, которые содержат тексты</param>
        /// <param name="performerOnly">Если этот параметр равен 1, поиск будет осуществляться только по названию исполнителя</param>
        /// <param name="count">количество аудиозаписей, информацию о которых необходимо вернуть</param>
        /// <param name="offset">смещение, необходимое для выборки определенного подмножетсва аудиозаписей</param>
        /// <returns></returns>
        public List<AudioEntry> Search(string query, AudioSortOrder order, bool withLyrics, int? count, int? offset, bool autoComplete = false, bool performerOnly = false)
        {
            this.Manager.Method("audio.search");
            this.Manager.Params("q", query);
            this.Manager.Params("auto_complete", ((autoComplete) ? 1 : 0));
            this.Manager.Params("lyrics", ((withLyrics) ? 1 : 0));
            this.Manager.Params("performer_only", ((performerOnly) ? 1 : 0));
            this.Manager.Params("sort", (Int32)order);

            if (offset != null)
            {
                this.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                this.Manager.Params("count", count);
            }


            XmlNode result = this.Manager.Execute().GetResponseXml().FirstChild;

            XmlUtils.UseNode(result);
            this.searchSongsCount = XmlUtils.Int("count");
            return this.buildList(result);

        }

        public List<AudioEntry> GetPopular(bool onlyEng, AudioGenre genre, int? offset, int? count)
        {
            this.Manager.Method("audio.getPopular");
            this.Manager.Params("only_eng", ((onlyEng) ? 1 : 0));
            this.Manager.Params("genre_id", (Int32)genre);

            if (count != null)
            {
                this.Manager.Params("count", count);
            }
            if (offset != null)
            {
                this.Manager.Params("offset", offset);
            }

            XmlNode result = this.Manager.Execute().GetResponseXml().FirstChild;

            XmlUtils.UseNode(result);
            return this.buildList(result);
        }

        /// <summary>
        /// Транслирует аудиозапись в статус пользователю или сообществу
        /// </summary>
        /// <param name="audio">идентификатор аудиозаписи, которая будет отображаться в статусе, в формате owner_id+audio_id</param>
        /// <param name="target_ids">перечисленные через запятую идентификаторы сообществ и пользователя, которым будет транслироваться аудиозапись. Идентификаторы сообществ должны быть заданы в формате "-gid"</param>
        /// <returns></returns>
        public bool SetBroadcast(string audio, string target_ids = null)
        {
            this.Manager.Method("audio.setBroadcast", new object[] { "audio", audio, "target_ids", target_ids });

            this.Manager.Params("audio", audio);

            if (target_ids != null)
            {
                this.Manager.Params("target_ids", target_ids);
            }

            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml());

            return XmlUtils.BoolVal();
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
