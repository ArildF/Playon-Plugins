namespace Rogue.PlayOn.Plugins.Channel9.Hierarchies
{
    class Series
    {
        private readonly string _url;
        private readonly IDownloader _downloader;

        public Series(string url, IDownloader downloader)
        {
            _url = url;
            _downloader = downloader;
        }

        public void AddToHierarchy(VirtualHierarchy hierarchy)
        {
            hierarchy.AddFolder(hierarchy.Root, "Series", folderSource: new ShowsFolderSource(
                                                              new FolderStructure("div", "entry-meta", "title"), _url,
                                                              _downloader));
        }
    }
}
