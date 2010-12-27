using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Rogue.PlayOnPlugins
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
            IEnumerable<XElement> items; 
            try
            { 
                items = doc.Element("rss").Element("channel").Elements("item");
            }
            catch (NullReferenceException)
            {
                throw new InvalidFeedException();
            }

            return from item in items 
                   let title = item.Element("title").ValueOrEmpty()
                   let enclosure = item.Element("enclosure")
                   let mediaContent = 
                        (from content in item.Descendants(Media + "content")
                         let def = content.Attribute("isDefault")
                         let isDefault = def != null && def.Value == "true"
                         let fileSize = content.Attribute("fileSize").ValueOrZero()
                         where content.Attribute("url").ValueOrEmpty().EndsWith(".wmv", StringComparison.InvariantCultureIgnoreCase)
                         orderby isDefault, fileSize descending
                         select content
                         ).FirstOrDefault()
                   let url = mediaContent != null ? 
                        mediaContent.Attribute("url").ValueOrNull() : 
                        enclosure.IfNotNull(e => e.Attribute("url").ValueOrNull()).Else(null)
                   where url != null
                   let durationString = mediaContent != null ? 
                       mediaContent.Attribute("duration").ValueOrNull() :
                       enclosure.Attribute("length").ValueOrNull()
                   let duration = durationString != null ? long.Parse(durationString) * 1000 : 0
                   let thumbNail = item.Element(Media + "thumbnail")
                   let thumbNailUrl = thumbNail != null ? thumbNail.Attribute("url").ValueOrNull() : null
                   let pubDate = ParsePubDate(item)
                   let description = item.Element("description").ValueOrEmpty()
                   select new MediaItem(title, url, description, duration, thumbNail: thumbNailUrl, publicationDate: pubDate);
        }

        private static DateTime? ParsePubDate(XElement item)
        {
            var date = item.Element("pubDate");
            if (date == null)
            {
                return null;
            }

            return DateTime.ParseExact(date.Value, "R", CultureInfo.InvariantCulture);
        }
    }
}