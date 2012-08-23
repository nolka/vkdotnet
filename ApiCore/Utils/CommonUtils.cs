using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace ApiCore
{
    public static class CommonUtils
    {
        /// <summary>
        /// Generate md5 hash from string
        /// </summary>
        /// <param name="str">The string to generate hash</param>
        /// <returns>String that contains md5 hash</returns>
        public static string Md5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(new UTF8Encoding().GetBytes(str)); ;
            return BitConverter.ToString(retVal).Replace("-", "");
        }

        public static DateTime FromUnixTime(double time)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(time);
        }

        public static DateTime FromUnixTime(int time)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(Convert.ToDouble(time));
        }

        public static DateTime FromUnixTime(string time)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(Convert.ToDouble(time));
        }

        public static string[] IntArrayToString(int[] integers)
        {
            try
            {
                string[] arr = new string[integers.Length];
                int i = 0;
                foreach (int item in integers)
                {
                    arr[i++] = item.ToString();
                }
                return arr;
            }
            catch { return new string[]{}; }
        }

        public static string IntArrayToCommaSeparatedString(int[] integers)
        {
            StringBuilder sb = new StringBuilder(integers.Length);
            for (int i=0; i<= integers.Length; i++)
            {
                sb.Append(integers[i]);
                if (i <= integers.Length)
                {
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }

        public static string StringArrayToCommaSeparatedString(string[] strings)
        {
            return string.Join(",", strings);
        }

        public static string[] ObjectsArrayToStringArray(object[] objects)
        {
            string[] strings = new string[objects.Length];
            int i=0;
            foreach(object o in objects)
            {
                strings[i] = o.ToString();
                i++;
            }
            return strings;
        }

        public static string ObjectsArrayToCommaSeparatedString(object[] objects)
        {
            return CommonUtils.StringArrayToCommaSeparatedString(CommonUtils.ObjectsArrayToStringArray(objects));
        }

    }
}
