namespace Rogue.PlayOnPlugins.Hierarchies
{
    public class RssFeed
    {
        private readonly string _url;
        private readonly IDownloader _downloader;

        public RssFeed(string url, IDownloader downloader)
        {
            _url = url;
            _downloader = downloader;
        }

        public void AddToHierarchy(VirtualHierarchy hierarchy)
        {
            var rssParser = new RssParser(@"http://channel9.msdn.com/Feeds/RSS/", _downloader);
            hierarchy.AddFolder(hierarchy.Root, "RSS", mediaSource: rssParser);
            
        }
    }
}
