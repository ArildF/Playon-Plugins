using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.PlayOn.Plugins.Channel9
{
	public interface IDownloader
	{
		string DownloadString(string url);
	}
}
