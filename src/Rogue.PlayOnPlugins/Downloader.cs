using System.Net;

namespace Rogue.PlayOnPlugins
{
	public class Downloader : IDownloader
    {
        private readonly WebClient _client = new WebClient();
        public string DownloadString(string url)
        {
            return _client.DownloadString(url);
        }
    }
}