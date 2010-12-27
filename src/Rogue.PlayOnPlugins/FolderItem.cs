using System.Collections.Generic;

namespace Rogue.PlayOnPlugins
{
    public class FolderItem : IMediaItemSource, IFolderSource
    {
        private readonly IMediaItemSource _mediaItemSource;
        public string Name { get; private set; }

        public FolderItem(string name, IMediaItemSource mediaItemSource = null)
        {
            _mediaItemSource = mediaItemSource;
            Name = name;
        }

        public IEnumerable<MediaItem> MediaItems()
        {
            return _mediaItemSource.MediaItems();
        }

        public IEnumerable<FolderItem> FolderItems()
        {
            yield break;
        }
    }
}