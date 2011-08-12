using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.Photos
{
    public class AlbumEntry: BaseEntry
    {
        /*
         <album>
            <aid>16178407</aid>
            <thumb_id>96509883</thumb_id>
            <owner_id>6492</owner_id>
            <title/>
            <description/>
            <created>1203967836</created>
            <updated>1238072451</updated>
            <size>3</size>
            <privacy>3</privacy>
            </album>
         */
        public int OwnerId;
        public int ThumbnailId;
        public string Title;
        public string Description;
        public DateTime DateCreated;
        public DateTime DateUpdated;
        public int Size;
        public int Privacy;

        public override string ToString()
        {
            return this.Title;
        }
    }
}
