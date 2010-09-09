using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Rogue.PlayOn.Plugins.Channel9.Hierarchies
{
    internal class Shows
    {
        private readonly string _url;
        private readonly IDownloader _downloader;

        public Shows(string url, IDownloader downloader)
        {
            _url = url;
            _downloader = downloader;
        }

        public void AddToHierarchy(VirtualHierarchy hierarchy)
        {
            hierarchy.AddFolder(hierarchy.Root, "Shows", folderSource: new ShowsFolderSource(_url, _downloader));
        }

        internal class ShowsFolderSource : IFolderSource
        {
            private readonly string _url;
            private readonly IDownloader _downloader;

            public ShowsFolderSource(string url, IDownloader downloader)
            {
                _url = url;
                _downloader = downloader;
            }

            public IEnumerable<FolderItem> FolderItems()
            {
                var html = _downloader.DownloadString(_url);

                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var showLinkRegex = new Regex(@"/shows/([^/]+)/$");

                var shows = (from a in doc.DocumentNode.Descendants("a")
                             let href = a.GetAttributeValue("href", "")
                             let match = showLinkRegex.Match(href)
                             where match != Match.Empty
                             select new {Title = a.InnerText, Link = match.Groups[1].Value}).Distinct();

                return from show in shows
                       let rootUri = new Uri(_url)
                       let rssFeed = new Uri(rootUri, String.Format("/shows/{0}/feed/wmvhigh", show.Link))
                       let rssParser = new RssParser(rssFeed.ToString(), _downloader)
                       let folder = new FolderItem(show.Title, rssParser)
                       select folder;
            }

        }
    }
}
