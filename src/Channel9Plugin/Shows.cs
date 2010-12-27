using Rogue.PlayOnPlugins;
using Rogue.PlayOnPlugins.Hierarchies;

namespace Rogue.PlayOn.Plugins.Channel9
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
            hierarchy.AddFolder(hierarchy.Root, "Shows", folderSource: new ShowsFolderSource(
                new FolderStructure("div", "entry-meta", "title"), _url, _downloader));
        }
    }
}
