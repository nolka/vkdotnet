using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ApiCore.Storage
{
    public class StorageFactory: BaseFactory
    {

        public StorageFactory(ApiManager manager)
            : base(manager)
        {
            this.Manager = manager;
        }

        public bool Set(string key, object value, bool isGlobal, int? uid)
        {
            this.Manager.Method("storage.set", new object[] {
                                            "key", key,
                                            "value", value,
                                            "global", isGlobal,
                                            "uid", uid
            });
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return ((x.SelectSingleNode("/response").InnerText.Equals("1")) ? true : false);
            }
            return false;
        }

        private StorageEntry buildEntry(XmlNode item, bool isGlobal)
        {
            if (item != null)
            {
                XmlUtils.UseNode(item);
                StorageEntry s = new StorageEntry();
                s.Key = XmlUtils.String("key");
                s.Value = XmlUtils.String("value");
                s.IsGlobal = isGlobal;
                return s;
            }
            return null;
        }

        private XmlDocument get(string key, string[] keys, bool isGlobal, int? userId)
        {
            this.Manager.Method("storage.get", new object[] {
                                            "key", key,
                                            "keys", keys,
                                            "global", isGlobal,
                                            "uid", userId
            });
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                return this.Manager.GetXmlDocument(resp);
            }
            return null;
        }

        public List<StorageEntry> GetList(string[] keys, bool isGlobal, int? userId)
        {
            XmlDocument x = this.get(null, keys, isGlobal, userId);
            XmlNodeList items = x.SelectNodes("/response/item");
            if (items.Count != 0)
            {
                List<StorageEntry> sl = new List<StorageEntry>();
                foreach (XmlNode node in items)
                {
                    sl.Add(this.buildEntry(node, isGlobal));
                }
                return sl;
            }
            throw new Exception("Query set is empty");
        }

        public bool GetBool(string key, bool isGlobal, int? userId)
        {
            XmlDocument x = this.get(key, null, isGlobal, userId);
            XmlUtils.UseNode(x.SelectSingleNode("/response"));
            return XmlUtils.BoolVal();
        }

        public int GetInt(string key, bool isGlobal, int? userId)
        {
            XmlDocument x = this.get(key, null, isGlobal, userId);
            XmlUtils.UseNode(x.SelectSingleNode("/response"));
            return XmlUtils.IntVal();
        }

        public float GetFloat(string key, bool isGlobal, int? userId)
        {
            XmlDocument x = this.get(key, null, isGlobal, userId);
            XmlUtils.UseNode(x.SelectSingleNode("/response"));
            return XmlUtils.FloatVal();
        }

        public string Get(string key, bool isGlobal, int? userId)
        {
            XmlDocument x = this.get(key, null, isGlobal, userId);
            return x.SelectSingleNode("/response").InnerText;
        }






    }
}
