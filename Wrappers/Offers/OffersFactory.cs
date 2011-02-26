using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ApiCore
{
    /// <summary>
    /// Represent Offers wrapper
    /// </summary>
    public class OffersFactory: BaseFactory
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="manager">ApiManager instance</param>
        public OffersFactory(ApiManager manager)
            : base(manager)
        {
            this.Manager = manager;
        }

        /// <summary>
        /// Edit your own offer message
        /// </summary>
        /// <param name="message">offer message</param>
        /// <returns>true if ok, else false</returns>
        public bool Edit(string message)
        {
            this.Manager.Method("offers.edit");
            this.Manager.Params("message", message);
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return ((x.SelectSingleNode("/response").InnerText == "1") ? true : false);
            }
            return false;
        }

        /// <summary>
        /// Open to public your friends
        /// </summary>
        /// <returns>true if ok, else false</returns>
        public bool Open()
        {
            this.Manager.Method("offers.open");
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return ((x.SelectSingleNode("/response").InnerText == "1") ? true : false);
            }
            return false;
        }

        /// <summary>
        /// Close your offer from friends
        /// </summary>
        /// <returns>true if ok, else false</returns>
        public bool Close()
        {
            this.Manager.Method("offers.close");
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return ((x.SelectSingleNode("/response").InnerText == "1") ? true : false);
            }
            return false;
        }

        private List<OfferEntry> buildFriendsEntryList(XmlDocument x)
        {
            XmlNodeList msgsNodes = x.SelectNodes("/response/offer");
            if (msgsNodes.Count >0)
            {
                List<OfferEntry> offerList = new List<OfferEntry>();
                foreach (XmlNode offerNode in msgsNodes)
                {
                    OfferEntry offer = new OfferEntry();
                    XmlUtils.UseNode(offerNode);
                    offer.UserId = XmlUtils.Int("uid");
                    offer.Message = XmlUtils.String("message");
                    offer.Active = XmlUtils.Bool("active");
                    offerList.Add(offer);
                }
                return offerList;
            }
            return null;
        }

        private List<OfferInboundEntry> buildFriendsInboundResponsesEntryList(XmlDocument x)
        {
            XmlNodeList offerNodes = x.SelectNodes("/response/user");
            if (offerNodes.Count > 0)
            {
                List<OfferInboundEntry> offerList = new List<OfferInboundEntry>();
                foreach (XmlNode offerNode in offerNodes)
                {
                    OfferInboundEntry offer = new OfferInboundEntry();
                    XmlUtils.UseNode(offerNode);
                    offer.UserId = XmlUtils.Int("uid");
                    offer.UserName = XmlUtils.String("name");
                    offer.Photo = XmlUtils.String("photo");
                    offer.Viewed = XmlUtils.Bool("viewed");
                    offer.Age = XmlUtils.Int("age");
                    offer.Message = XmlUtils.String("message");
                    offer.CityId = XmlUtils.Int("city_id");
                    offer.CityName = XmlUtils.String("city_name");

                    offerList.Add(offer);
                }
                return offerList;
            }
            return null;
        }

        private List<OfferOutboundEntry> buildFriendsOutboundResponsesEntryList(XmlDocument x)
        {
            XmlNodeList offerNodes = x.SelectNodes("/response/user");
            if (offerNodes.Count > 0)
            {
                List<OfferOutboundEntry> offerList = new List<OfferOutboundEntry>();
                foreach (XmlNode offerNode in offerNodes)
                {
                        OfferOutboundEntry offer = new OfferOutboundEntry();
                        XmlUtils.UseNode(offerNode);
                        offer.UserId = XmlUtils.Int("uid");
                        offer.UserName = XmlUtils.String("name");
                        offer.Photo = XmlUtils.String("photo");
                        offer.Viewed = XmlUtils.Bool("viewed");
                        offer.Age = XmlUtils.Int("age");
                        offer.CityId = XmlUtils.Int("city_id");
                        offer.CityName = XmlUtils.String("city_name");

                        offerList.Add(offer);
                }
                return offerList;
            }
            return null;
        }

        /// <summary>
        /// Get offer of current user
        /// </summary>
        /// <param name="userIds">is set, returns user friends offers. specify user ids as comma-separated</param>
        /// <returns>OffetsList</returns>
        public List<OfferEntry> Get(string userIds)
        {
            this.Manager.Method("offers.get");
            if (userIds != null)
            {
                this.Manager.Params("uids", userIds);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public List<OfferInboundEntry> GetInboundResponses(int? count, int? offset)
        {
            this.Manager.Method("offers.getInboundResponses");
            if (count != null)
            {
                this.Manager.Params("count", count);
            }
            if (offset != null)
            {
                this.Manager.Params("offset", offset);
            }
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                if (x.SelectSingleNode("/response").InnerText.Equals("0"))
                {
                    return null;
                }
                return this.buildFriendsInboundResponsesEntryList(x);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public List<OfferOutboundEntry> GetOutboundResponses(int? count, int? offset)
        {
            this.Manager.Method("offers.getOutboundResponses");
            if (count != null)
            {
                this.Manager.Params("count", count);
            }
            if (offset != null)
            {
                this.Manager.Params("offset", offset);
            }
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                if (x.SelectSingleNode("/response").InnerText.Equals("0"))
                {
                    return null;
                }
                return this.buildFriendsOutboundResponsesEntryList(x);
            }
            return null;
        }

        /// <summary>
        /// Accept friend offer
        /// </summary>
        /// <param name="offer_uid">friend id to accept offer</param>
        /// <returns>true if ok, else false</returns>
        public bool Accept(int offer_uid)
        {
            this.Manager.Method("offers.accept");
            this.Manager.Params("uid", offer_uid);
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return ((x.SelectSingleNode("/response").InnerText == "1") ? true : false);
            }
            return false;
        }

        /// <summary>
        /// Refuse friend offer
        /// </summary>
        /// <param name="offer_uid">friend id to refuse</param>
        /// <returns>true if ok, else false</returns>
        public bool Refuse(int offer_uid)
        {
            this.Manager.Method("offers.refuse");
            this.Manager.Params("uid", offer_uid);
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return ((x.SelectSingleNode("/response").InnerText == "1") ? true : false);
            }
            return false;
        }

        /// <summary>
        /// Sets responses to your offer as viewed
        /// </summary>
        /// <param name="offer_uids">comma-separated user id's, thats answers are viewed</param>
        /// <returns>true if ok, else false</returns>
        public bool SetResponseViewed(string offer_uids)
        {
            this.Manager.Method("offers.setResponseViewed");
            this.Manager.Params("uids", offer_uids);
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return ((x.SelectSingleNode("/response").InnerText == "1") ? true : false);
            }
            return false;
        }

        /// <summary>
        /// Delete offer responses from friends
        /// </summary>
        /// <param name="offer_uids">friends ids to delete responses</param>
        /// <returns>true if ok, else false</returns>
        public bool DeleteResponses(string offer_uids)
        {
            this.Manager.Method("offers.deleteResponses");
            this.Manager.Params("uids", offer_uids);
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return ((x.SelectSingleNode("/response").InnerText == "1") ? true : false);
            }
            return false;
        }
        
    }
}
