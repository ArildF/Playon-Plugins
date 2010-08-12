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
        public Payload GetSharedMedia(string id, bool includeChildren, int startIndex, int requestCount)
        {
            throw new NotImplementedException();
        }

        public string Resolve(SharedMediaFileInfo fileInfo)
        {
            throw new NotImplementedException();
        }

        public void SetPlayOnHost(IPlayOnHost host)
        {
            throw new NotImplementedException();
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
