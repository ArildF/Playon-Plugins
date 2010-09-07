using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using MediaMallTechnologies.Plugin;
using System.Linq;

namespace Rogue.PlayOn.Plugins.Channel9
{
    internal abstract class HierarchyNode
    {
        public string ParentId { get; private set; }
        public string Title { get; private set; }

        protected HierarchyNode(string parentId, string id, string title)
        {
            ParentId = parentId;
            Title = title;
            Id = id;
        }

        public string Id { get; private set; }
        public abstract bool IsContainer { get; }

        public abstract AbstractSharedMediaInfo ToMedia();
    }

    class VirtualHierarchy
    {
        private readonly Dictionary<string, HierarchyNode> _nodes = new Dictionary<string, HierarchyNode>();
        private readonly string _root;

        public VirtualHierarchy(string rootId)
        {
            _root = rootId;

            _nodes.Add(_root, new FolderNode(null, _root, "", null));
        }

        public HierarchyNode Root
        {
            get{ return _nodes[_root];}
        }


        public HierarchyNode GetNode(string id)
        {
            return _nodes[id];
        }

        public void AddChild(HierarchyNode node, HierarchyNode child)
        {
            var folderNode = (FolderNode) node;
            folderNode.AddChild(child);
            _nodes[child.Id] = child;
        }

        public IEnumerable<HierarchyNode> GetChildren(HierarchyNode node)
        {
            var folderNode = (FolderNode) node;
            foreach (var child in folderNode.GetChildren(this))
            {
                _nodes[child.Id] = child;
                yield return child;
            }
        }


        private class FolderNode : HierarchyNode
        {
            private readonly IMediaItemSource _source;
            private readonly List<HierarchyNode> _children = new List<HierarchyNode>();

            public FolderNode(string parentId, string id, string title, IMediaItemSource source) : base(parentId, id, title)
            {
                _source = source;
            }

            public override bool IsContainer
            {
                get { return false; }
            }

            public IEnumerable<HierarchyNode> GetChildren(VirtualHierarchy virtualHierarchy)
            {
                foreach (var hierarchyNode in _children)
                {
                    yield return hierarchyNode;
                }

                if (_source != null)
                {
                    foreach (var node in _source.MediaItems().Select(mi => virtualHierarchy.CreateMediaItemNode(this, mi)))
                    {
                        yield return node;
                    } 
                }
            }

            public void AddChild(HierarchyNode hierarchyNode)
            {
                _children.Add(hierarchyNode);
            }

            public override AbstractSharedMediaInfo ToMedia()
            {
                return new SharedMediaFolderInfo(Id, ParentId, Title, 0);
            }
        }

        private MediaItemNode CreateMediaItemNode(FolderNode parent, MediaItem item)
        {
            var itemNode = new MediaItemNode(item, parent.Id, CreateId());
            _nodes[itemNode.Id] = itemNode;

            return itemNode;
        }

        private class MediaItemNode : HierarchyNode
        {
            private readonly MediaItem _item;

            public MediaItemNode(MediaItem item, string parentId, string id) : base(parentId, id, item.Title)
            {
                _item = item;
            }

            public override bool IsContainer
            {
                get { return false; }
            }

            public override AbstractSharedMediaInfo ToMedia()
            {
                return new VideoResource(Id, ParentId, Title, _item.Url, _item.Description, null, DateTime.MinValue, null,
                    new NameValueCollection(), _item.Duration, 0, genre: "", resolution:"");

            }
        }

        public HierarchyNode CreateFolder(HierarchyNode parent, string name, IMediaItemSource source = null)
        {
            var node = new FolderNode(parent.Id, CreateId(), name, source);
            AddChild(parent, node);
            return node;
        }

        private string CreateId()
        {
            return _root + "-" + Guid.NewGuid();
        }
    }

    
}
