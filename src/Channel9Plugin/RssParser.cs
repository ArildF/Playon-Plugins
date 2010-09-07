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

        private static readonly XNamespace Media = "http://search.yahoo.com/mrss/";


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
                   let enclosure = item.Element("enclosure")
                   let mediaContent = 
                        (from content in item.Descendants(Media + "content")
                         let def = content.Attribute("isDefault")
                         where def != null && def.Value == "true"
                         where content.Attribute("url").Value.EndsWith(".wmv", StringComparison.InvariantCultureIgnoreCase)
                         select content
                         ).FirstOrDefault()
                   let url = mediaContent != null ? 
                        mediaContent.Attribute("url").Value : 
                        enclosure.Attribute("url").Value 
                   let durationString = mediaContent != null ? 
                       mediaContent.Attribute("duration").Value :
                       enclosure.Attribute("length").Value
                   let duration = long.Parse(durationString) * 1000
                   let description = item.Element("description").Value 
                   select new MediaItem(title, url, description, duration);
        }
    }
}