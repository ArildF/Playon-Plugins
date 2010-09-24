using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
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

                var shows = (from div in doc.DocumentNode.Descendants("div")
                             where div.GetAttributeValue("class", "").Equals("entry-meta")
                             from a in div.Descendants("a")
                             where a.GetAttributeValue("class", "") == "title"
                             let title = HttpUtility.HtmlDecode(a.InnerText).Replace('\u00A0', ' ')
                             let href = HttpUtility.HtmlDecode(a.GetAttributeValue("href", ""))
                             let root = new Uri(_url, UriKind.Absolute)
                                 .GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped)
                             let rootUri = new Uri(root)
                             let rss = new Uri(rootUri, href + "/RSS")
                             let parser = new RssParser(rss.ToString(), _downloader)
                             select new FolderItem(title, parser)).Concat(
                                from paging in doc.DocumentNode.Descendants("ul")
                                where paging.GetAttributeValue("class", "").Equals("paging")
                                from li in paging.Descendants("li")
                                where li.GetAttributeValue("class", "").Equals("next")
                                from a in li.Descendants("a")
                                let href = HttpUtility.HtmlDecode(a.GetAttributeValue("href", ""))
                                let root = new Uri(_url, UriKind.Absolute)
                                    .GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped)
                                let rootUri = new Uri(root)
                                let uri = new Uri(rootUri, href)
                                let nextFolderSource = new ShowsFolderSource(uri.ToString(), _downloader)
                                from folderSource in nextFolderSource.FolderItems()
                                select folderSource);

                return shows;


                //var showLinkRegex = new Regex(@"/shows/([^/]+)/$");

                //var shows = (from a in doc.DocumentNode.Descendants("a")
                //             let href = a.GetAttributeValue("href", "")
                //             let match = showLinkRegex.Match(href)
                //             where match != Match.Empty
                //             select new {Title = a.InnerText, Link = match.Groups[1].Value}).Distinct();

                //return from show in shows
                //       let rootUri = new Uri(_url)
                //       let rssFeed = new Uri(rootUri, String.Format("/shows/{0}/feed/wmvhigh", show.Link))
                //       let rssParser = new RssParser(rssFeed.ToString(), _downloader)
                //       let folder = new FolderItem(show.Title, rssParser)
                //       select folder;
            }

        }
    }
}
