using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

using Procurios.Public;

namespace ApiCore.Audio
{
    public class AudioUploadedInfo
    {
        public string Server;
        public string Audio;
        public string Hash;
        public string Artist = null;
        public string Title = null;

        public AudioUploadedInfo(string jsonstr, string artist, string title)
        {
            object jsonObj = JSON.JsonDecode(jsonstr);
            Hashtable h = (Hashtable)JSON.JsonDecode(jsonstr);

            // присваиваем значения
            this.Server = (string)h["server"];
            this.Audio = (string)h["audio"];
            this.Hash = (string)h["hash"];
            this.Artist = artist;
            this.Title = title;
        }

    }
}
