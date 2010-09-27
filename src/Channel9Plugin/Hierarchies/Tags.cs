using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.PlayOn.Plugins.Channel9.Hierarchies
{
    class Tags
    {
        private readonly string _url;
        private readonly IDownloader _downloader;

        public Tags(string url, IDownloader downloader)
        {
            _url = url;
            _downloader = downloader;
        }

        public void AddToHierarchy(VirtualHierarchy hierarchy)
        {
            hierarchy.AddFolder(hierarchy.Root, "Tags", folderSource: new ShowsFolderSource(
                                                            new FolderStructure("ul", "default", ""), _url,
                                                            _downloader));

        }
    }
}
