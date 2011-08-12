using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.Photos
{
    public class PhotoEntryShort: BaseEntry   
    {
        public int AlbumId;
        public int OwnerId;
        public string Url;
        public string UrlBig;
        public string UrlSmall;
        public DateTime DateCreated;

        public override string ToString()
        {
            return this.AlbumId.ToString()+"_"+this.Id;
        }
    }
}
