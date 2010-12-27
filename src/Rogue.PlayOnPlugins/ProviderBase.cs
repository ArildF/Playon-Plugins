using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using MediaMallTechnologies.Plugin;

namespace Rogue.PlayOnPlugins
{
	public abstract class ProviderBase
	{
		private VirtualHierarchy _hierarchy;

		public Payload GetSharedMedia(string id, bool includeChildren, int startIndex, int requestCount)
		{
			_hierarchy = _hierarchy ?? CreateHierarchy();

			var node = _hierarchy.GetNode(id);

			IEnumerable<AbstractSharedMediaInfo> nodes = new []{node.ToMedia()};
			if (includeChildren)
			{
				nodes = _hierarchy.GetChildren(node).Select(n => n.ToMedia())
					.Skip(startIndex);
				if (requestCount != 0)
				{
					nodes = nodes.Take(requestCount);
				}

			}

			var list = nodes.ToArray();


			ApplySortIndexes(list);


			return new Payload(node.Id, node.ParentId, node.Title, 0, list, node.IsContainer);
            
		}

		private static void ApplySortIndexes(IEnumerable<AbstractSharedMediaInfo> nodes)
		{
			int idx = 0;
			nodes.OfType<SharedMediaFileInfo>().OrderByDescending(n => n.Date)
				.ForEach(n => n.MetadataProperties["SortIndex"] = (idx++).ToString("000"));
		}

		public string Resolve(SharedMediaFileInfo fileInfo)
		{
			return String.Format(@"
<media>
    <url type=""wmv"">{0}</url>
</media>", SecurityElement.Escape(fileInfo.Path));
		}

		public void SetPlayOnHost(IPlayOnHost host)
		{
		}

		protected abstract VirtualHierarchy CreateHierarchy();
	}
}