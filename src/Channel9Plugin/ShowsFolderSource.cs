using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using Rogue.PlayOnPlugins;
using Rogue.PlayOnPlugins.Hierarchies;

namespace Rogue.PlayOn.Plugins.Channel9
{
    internal class ShowsFolderSource : IFolderSource
    {
        private readonly FolderStructure _folderStructure;
        private readonly string _url;
        private readonly IDownloader _downloader;

        public ShowsFolderSource(FolderStructure folderStructure, string url, IDownloader downloader)
        {
            _folderStructure = folderStructure;
            _url = url;
            _downloader = downloader;
        }

        public IEnumerable<FolderItem> FolderItems()
        {
            var html = _downloader.DownloadString(_url);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var shows = (from div in doc.DocumentNode.Descendants(_folderStructure.FolderContainerTag)
                         where div.GetAttributeValue("class", "").Equals(_folderStructure.FolderContainerClass)
                         from a in div.Descendants("a")
                         where a.GetAttributeValue("class", "") == _folderStructure.LinkClass
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
                                 let nextFolderSource = new ShowsFolderSource(_folderStructure, uri.ToString(), _downloader)
                                 from folderSource in nextFolderSource.FolderItems()
                                 select folderSource);
            
            return shows;
        }

    }
}