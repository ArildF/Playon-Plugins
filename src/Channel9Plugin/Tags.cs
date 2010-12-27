using Rogue.PlayOnPlugins;
using Rogue.PlayOnPlugins.Hierarchies;

namespace Rogue.PlayOn.Plugins.Channel9
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
