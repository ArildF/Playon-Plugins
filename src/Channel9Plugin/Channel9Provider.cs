using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            var children = _hierarchy.GetChildren(node).Select(n => n.ToMedia())
                .Skip(startIndex);
            if (requestCount != 0)
            {
                children = children.Take(requestCount);
            }

            return new Payload(node.Id, node.ParentId, node.Title, 0, children.ToArray());
            
        }

        public string Resolve(SharedMediaFileInfo fileInfo)
        {
            throw new NotImplementedException();
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
