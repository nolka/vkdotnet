using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ApiCore.Subscriptions
{
    public class SubscriptionsFactory : BaseFactory
    {
        public SubscriptionsFactory(ApiManager manager)
            : base(manager)
        {
            this.Manager = manager;
        }

        private int[] buildUsersList(XmlNodeList data)
        {
            if (data != null && data.Count > 0)
            {
                int[] users = new int[data.Count];
                int i=0;
                foreach (XmlNode n in data)
                {
                    XmlUtils.UseNode(n);
                    users[i] = XmlUtils.IntVal();
                }
                return users;
            }
            return null;
        }

        public SubscriptionsInfo Get(int? userId, int? offset, int? count)
        {
            this.Manager.Method("subscriptions.get", new object[] { "uid", userId, "offset", offset, "count", count });

            XmlNode result = this.Manager.Execute().GetResponseXml();
            XmlUtils.UseNode(result);
            SubscriptionsInfo si = new SubscriptionsInfo();
            si.Count = XmlUtils.Int("count");
            si.Users = this.buildUsersList(result.SelectNodes("users/uid"));

            return si;
        }

        public SubscriptionsInfo GetFollowers(int? userId, int? offset, int? count)
        {
            this.Manager.Method("subscriptions.getFollowers", new object[] { "uid", userId, "offset", offset, "count", count });

            XmlNode result = this.Manager.Execute().GetResponseXml();
            XmlUtils.UseNode(result);
            SubscriptionsInfo si = new SubscriptionsInfo();
            si.Count = XmlUtils.Int("count");
            si.Users = this.buildUsersList(result.SelectNodes("users/uid"));

            return si;
        }

        public bool Follow(int userId)
        {
            this.Manager.Method("subscriptions.follow");
            this.Manager.Params("uid", userId);
            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml());

            return XmlUtils.BoolVal();
        }

        public bool Unfollow(int userId)
        {
            this.Manager.Method("subscriptions.follow");
            this.Manager.Params("uid", userId);
            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml());

            return XmlUtils.BoolVal();
        }
    }
}
