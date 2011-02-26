using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCore
{
    public class AudioEntry: BaseEntry
    {
        public int OwnerId;
        public int Duration;
        public string Artist;
        public string Title;
        public string Url;

        public override string ToString()
        {
            return this.Artist + " - " + this.Title;
        }
    }
}
