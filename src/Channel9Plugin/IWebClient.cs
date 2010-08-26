using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.PlayOn.Plugins.Channel9
{
	public interface IWebClient
	{
		string DownloadString(string url);
	}
}
