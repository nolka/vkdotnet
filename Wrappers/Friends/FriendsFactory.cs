using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ApiCore
{
    public class FriendsFactory: BaseFactory
    {

        public FriendsFactory(ApiManager manager)
            : base(manager)
        {
            this.Manager = manager;
        }

        private List<int> buildFriendsList(XmlNodeList node)
        {
            if (node != null)
            {
                List<int> lists = new List<int>();
                foreach (XmlNode n in node)
                {
                    lists.Add(Convert.ToInt32(n.InnerText));
                }
                return lists;
            }
            return null;
        }

        private List<Friend> buildFriendsEntryList(XmlDocument x)
        {
            XmlNodeList msgsNodes = x.SelectNodes("/response/user");
            if (msgsNodes.Count>0)
            {
                List<Friend> friendList = new List<Friend>();
                foreach (XmlNode msgNode in msgsNodes)
                {
                    Friend friend = new Friend();
                    XmlUtils.UseNode(msgNode);
                    friend.Id = XmlUtils.Int(FriendFields.Id);
                    friend.FirstName = XmlUtils.String(FriendFields.FirstName);
                    friend.LastName = XmlUtils.String(FriendFields.LastName);
                    friend.NickName = XmlUtils.String(FriendFields.NickName);
                    friend.Rating = XmlUtils.Int(FriendFields.Rating);
                    friend.Sex = (FriendSex)XmlUtils.Int(FriendFields.Sex);
                    friend.BirthDate = XmlUtils.String(FriendFields.BirthDate);
                    friend.City = XmlUtils.Int(FriendFields.City);
                    friend.Country = XmlUtils.Int(FriendFields.Country);
                    friend.Timezone = XmlUtils.String(FriendFields.Timezone);
                    friend.Photo = XmlUtils.String(FriendFields.Photo);
                    friend.PhotoMedium = XmlUtils.String(FriendFields.PhotoMedium);
                    friend.PhotoBig = XmlUtils.String(FriendFields.PhotoBig);
                    friend.Online = XmlUtils.Bool(FriendFields.Online);
                    friend.Lists = this.buildFriendsList(msgNode.SelectNodes("lists/list/item"));
                    friend.Domain = XmlUtils.String(FriendFields.Domain);
                    friend.HomePhone = XmlUtils.String(FriendFields.HomePhone);
                    friend.MobilePhone = XmlUtils.String(FriendFields.MobilePhone);
                    friend.HasMobile = XmlUtils.Bool(FriendFields.HasMobile);
                    friend.University = XmlUtils.Int(FriendFields.University);
                    friend.UniversityName = XmlUtils.String(FriendFields.UniversityName);
                    friend.Faculty = XmlUtils.Int(FriendFields.Faculty);
                    friend.FacultyName = XmlUtils.String(FriendFields.FacultyName);
                    friend.Graduation = XmlUtils.Int(FriendFields.Graduation);
                    friendList.Add(friend);
                }
                return friendList;
            }
            return null;
        }

        /// <summary>
        /// Gets the friend list
        /// </summary>
        /// <param name="nameCase">friends name case</param>
        /// <param name="fields">fields array to be fetched</param>
        /// <returns>friend list</returns>
        public List<Friend> Get(string nameCase, int? count, int? offset, int? listId,  string[] fields)
        {
            this.Manager.Method("friends.get");
            if (nameCase != null)
            {
                this.Manager.Params("name_case", nameCase);
            }
            if (count != null)
            {
                this.Manager.Params("count", count);
            }
            if (offset != null)
            {
                this.Manager.Params("offset", offset);
            }
            if (listId != null)
            {
                this.Manager.Params("lid", listId);
            }
            if (fields != null)
            {
                this.Manager.Params("fields", String.Join(",",fields));
            }
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                if (x.SelectSingleNode("/response").InnerText.Equals("0"))
                {
                    return null;
                }
                return this.buildFriendsEntryList(x);
            }
            return null;
        }

    }
}
