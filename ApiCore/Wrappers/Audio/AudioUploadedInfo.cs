using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

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
            // отрезаем скобки, убираем кавычки
            jsonstr = jsonstr.Replace("{", "").Replace("}", "").Replace("\"", "");

            // разделяем строку на кусочки по  запятой с пробелами
            Regex r = new Regex(@"\,[\s]*");
            string[] json = r.Split(jsonstr);

            // далее каждый кусок обрабатываем как ключ: значение
            // и складываем их в таблицу
            Hashtable h = new Hashtable();
            r = new Regex(@"(\:[\s]+)");
            foreach (string str in json)
            {
                string[] kv = r.Split(str);
                h[kv[0]] = kv[2];
            }

            // присваиваем значения
            this.Server = (string)h["server"];
            this.Audio = (string)h["audio"];
            this.Hash = (string)h["hash"];
            this.Artist = artist;
            this.Title = title;
        }

    }
}
