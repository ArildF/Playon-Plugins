using System.Text;
using System.Text.RegularExpressions;

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
    }
}
