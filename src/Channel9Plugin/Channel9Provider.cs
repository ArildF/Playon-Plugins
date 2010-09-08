using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using MediaMallTechnologies.Plugin;
using Rogue.PlayOn.Plugins.Channel9.Properties;

namespace Rogue.PlayOn.Plugins.Channel9
{
    public class Channel9Provider : IPlayOnProvider
    {
        private VirtualHierarchy _hierarchy;
        private IDownloader _downloader = new Downloader();

        public Payload GetSharedMedia(string id, bool includeChildren, int startIndex, int requestCount)
        {
            _hierarchy = _hierarchy ?? CreateHierarchy();

            var node = _hierarchy.GetNode(id);

            IEnumerable<AbstractSharedMediaInfo> nodes = new []{node.ToMedia()};
            if (includeChildren)
            {
                nodes = _hierarchy.GetChildren(node).Select(n => n.ToMedia())
                    .Skip(startIndex);
                if (requestCount != 0)
                {
                    nodes = nodes.Take(requestCount);
                }

            }

            var list = nodes.ToArray();


            ApplySortIndexes(list);


            return new Payload(node.Id, node.ParentId, node.Title, 0, list, node.IsContainer);
            
        }

        private static void ApplySortIndexes(IEnumerable<AbstractSharedMediaInfo> nodes)
        {
            int idx = 0;
            nodes.OfType<SharedMediaFileInfo>().OrderByDescending(n => n.Date)
                .ForEach(n => n.MetadataProperties["SortIndex"] = (idx++).ToString("000"));
        }

        public string Resolve(SharedMediaFileInfo fileInfo)
        {
            return String.Format(@"
<media>
    <url type=""wmv"">{0}</url>
</media>", SecurityElement.Escape(fileInfo.Path));
        }

        public void SetPlayOnHost(IPlayOnHost host)
        {
        }

        public void SetWebClient(IDownloader client)
        {
            _downloader = client;
        }

        private VirtualHierarchy CreateHierarchy()
        {
            var rssParser = new RssParser(@"http://channel9.msdn.com/Feeds/RSS/", _downloader);
            var hierarchy = new VirtualHierarchy(ID);

            hierarchy.CreateFolder(hierarchy.Root, "RSS", rssParser);
            return hierarchy;
        }

        public string Name
        {
            get { return "Channel 9 (MSDN)"; }
        }

        public string ID
        {
            get { return "Channel9Plugin"; }
        }

        public Image Image
        {
            get { return Resources.logo; }
        }
    }
}
