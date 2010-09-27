using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace Rogue.PlayOn.Plugins.Channel9.Hierarchies
{
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
        }

    }
}