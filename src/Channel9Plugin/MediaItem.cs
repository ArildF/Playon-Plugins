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

        public MediaItem(string title, string url, string description = "")
        {
            Title = title;
            Url = url;
            Description = description;
        }
    }
}
