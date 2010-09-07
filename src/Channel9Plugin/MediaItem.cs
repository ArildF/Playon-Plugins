using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.PlayOn.Plugins.Channel9
{
    public class MediaItem
    {
        public string Title { get; private set; }
        public string Url { get; private set; }
        public string Description { get; private set; }
        public long Duration { get; private set; }

        public MediaItem(string title, string url, string description = "", long duration =  0)
        {
            Title = title;
            Url = url;
            Description = description;
            Duration = duration;
        }
    }
}
