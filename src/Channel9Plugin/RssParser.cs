using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Rogue.PlayOn.Plugins.Channel9
{
    public class RssParser : IMediaItemSource
    {
        private readonly string _url;
        private readonly IDownloader _downloader;

        public RssParser(string url, IDownloader downloader)
        {
            _url = url;
            _downloader = downloader;
        }

        public IEnumerable<MediaItem> MediaItems()
        {
            var rssString = _downloader.DownloadString(_url);

            var doc = XDocument.Parse(rssString);
            var items = doc.Element("rss").Element("channel").Elements("item");

            return from item in items 
                   let title = item.Element("title").Value 
                   let url = item.Element("enclosure").Attribute("url").Value 
                   let description = item.Element("description").Value 
                   select new MediaItem(title, url, description);
        }
    }
}