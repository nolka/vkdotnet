using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.Photos
{
    public class PhotoEntryFull: BaseEntry
    {
        /*
         * <photo>
          <pid>146771291</pid>
          <aid>100001227</aid>
          <owner_id>6492</owner_id>
          <src>http://cs9231.vkontakte.ru/u06492/100001227/m_7875d2fb.jpg</src>
          <src_big>http://cs9231.vkontakte.ru/u06492/100001227/x_cd563004.jpg</src_big>
          <src_small>http://cs9231.vkontakte.ru/u06492/100001227/s_c3bba2a8.jpg</src_small>
          <src_xbig>http://cs9231.vkontakte.ru/u06492/100001227/y_62a74569.jpg</src_xbig>
          <src_xxbig>http://cs9231.vkontakte.ru/u06492/100001227/z_793e9682.jpg</src_xxbig>
          <text>test</text>
          <created>1260885741</created>
         </photo>
         */
        public int AlbumId;
        public int OwnerId;
        public string Url;
        public string UrlBig;
        public string UrlSmall;
        public string UrlXBig;
        public string UrlXXBig;
        public string Text;
        public DateTime DateCreated;

        public override string ToString()
        {
            return this.Text;
        }
    }
}
