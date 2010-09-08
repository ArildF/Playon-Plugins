using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.PlayOn.Plugins.Channel9
{
    public class MediaItem
    {
        public DateTime? PublicationDate { get; private set; }
        public string Title { get; private set; }
        public string Url { get; private set; }
        public string Description { get; private set; }
        public long Duration { get; private set; }
        public string ThumbNail { get; set; }

        public MediaItem(string title, string url, string description = "", long duration =  0, string thumbNail = null, 
            DateTime? publicationDate = null)
        {
            Title = title;
            Url = url;
            Description = description;
            Duration = duration;
            ThumbNail = thumbNail;
            PublicationDate = publicationDate;
        }
    }
}
