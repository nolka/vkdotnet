using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.Offers
{
    public class OfferOutboundEntry
    {
        /*
         * <user>
            <uid>51173856</uid>
            <name>Yekaterina Volkovich</name>
            <photo>httр://cs671.vkоntakte.ru/u51173856/b_3a60674d.jpg</photo>
            <date>2009-09-30 23:03:24</date>
            <viewed>1</viewed>
            <age>16</age>
            <city_id>281</city_id>
            <city_name>Брест</city_name>
            </user>
         */
        public int UserId;
        public string UserName;
        public string Photo;
        public bool Viewed;
        public int Age;
        public int CityId;
        public string CityName;
    }
}
