using System.Collections.Generic;

namespace Rogue.PlayOnPlugins
{
    public interface IMediaItemSource
    {
        IEnumerable<MediaItem> MediaItems();
    }

    public interface IFolderSource
    {
        IEnumerable<FolderItem> FolderItems();
    }
}
