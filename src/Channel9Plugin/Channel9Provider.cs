using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using MediaMallTechnologies.Plugin;

namespace Rogue.PlayOn.Plugins.Channel9
{
    public class Channel9Provider : IPlayOnProvider
    {
        private VirtualHierarchy _hierarchy;

        public Payload GetSharedMedia(string id, bool includeChildren, int startIndex, int requestCount)
        {
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
            _hierarchy = new VirtualHierarchy(ID);

            var folder = _hierarchy.CreateFolder(_hierarchy.Root, "RSS");


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
            get { throw new NotImplementedException(); }
        }
    }
}
