using System;
using System.Net;

namespace Rogue.PlayOn.Plugins.Channel9
{
    internal class Downloader : IDownloader
    {
        private readonly WebClient _client = new WebClient();
        public string DownloadString(string url)
        {
            return _client.DownloadString(url);
        }
    }
}