using System.Drawing;
using MediaMallTechnologies.Plugin;
using Rogue.PlayOn.Plugins.Channel9.Properties;
using Rogue.PlayOnPlugins;
using Rogue.PlayOnPlugins.Hierarchies;

namespace Rogue.PlayOn.Plugins.Channel9
{
    public class Channel9Provider : ProviderBase, IPlayOnProvider
    {
    	private IDownloader _downloader = new Downloader();

    	public void SetWebClient(IDownloader client)
        {
            _downloader = client;
        }

    	protected override VirtualHierarchy CreateHierarchy()
        {
            var hierarchy = new VirtualHierarchy(ID);

            new RssFeed("http://channel9.msdn.com/Feeds/RSS/", _downloader).AddToHierarchy(hierarchy);

            new Shows("http://channel9.msdn.com/Browse/Shows?sort=atoz&page=1", _downloader).AddToHierarchy(hierarchy);
            new Tags("http://channel9.msdn.com/Browse/Tags?page=1", _downloader).AddToHierarchy(hierarchy);
            new Series("http://channel9.msdn.com/Browse/Series?sort=atoz&page=1", _downloader).AddToHierarchy(hierarchy);

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
