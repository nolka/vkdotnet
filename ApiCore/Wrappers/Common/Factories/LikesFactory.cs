using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ApiCore
{
    public static class LikesFactory
    {
        public static LikesInfo GetLikesInfo(XmlNode node)
        {
            if (node != null)
            {
                XmlUtils.UseNode(node);
                LikesInfo l = new LikesInfo();
                l.CanLike = XmlUtils.Bool("can_like");
                l.CanPublish = XmlUtils.Bool("can_publish");
                l.Count = XmlUtils.Int("count");
                l.UserLikes = XmlUtils.Int("user_likes");
                return l;
            }
            return null;
        }
    }
}
