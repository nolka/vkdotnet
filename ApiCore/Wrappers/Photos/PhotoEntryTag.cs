using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore.Photos
{
    public class PhotoEntryTag: BaseEntry
    {
        public int UserId;
        public int PlacerId;
        public string TaggedName;
        public DateTime Date;
        public float X;
        public float Y;
        public float X2;
        public float Y2;
        public int Viewed;
    }
}
