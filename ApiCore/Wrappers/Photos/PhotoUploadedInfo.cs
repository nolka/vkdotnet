using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace ApiCore.Photos
{
    public class PhotoUploadedInfo
    {
        public string Server;
        public string Photo;
        public string Hash;

        public PhotoUploadedInfo(string jsonstr)
        {
            // отрезаем скобки, убираем кавычки
            System.Windows.Forms.MessageBox.Show(jsonstr);
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
            this.Photo = (string)h["photo"];
            this.Hash = (string)h["hash"];
        }
    }
}
