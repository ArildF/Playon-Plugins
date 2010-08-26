using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaMallTechnologies.Plugin;

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

        public abstract AbstractSharedMediaInfo ToMedia();
    }

    class VirtualHierarchy
    {
        private readonly Dictionary<string, HierarchyNode> _nodes = new Dictionary<string, HierarchyNode>();
        private readonly string _root;

        public VirtualHierarchy(string rootId)
        {
            _root = rootId;

            _nodes.Add(_root, new FolderNode(null, _root, ""));
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
            foreach (var child in folderNode.Children)
            {
                _nodes[child.Id] = child;
                yield return child;
            }
        }

       

        internal class FolderNode : HierarchyNode
        {
            private readonly List<HierarchyNode> _children = new List<HierarchyNode>();

            public FolderNode(string parentId, string id, string title) : base(parentId, id, title)
            {
            }

            public IEnumerable<HierarchyNode> Children
            {
                get { return _children; }
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

        public HierarchyNode CreateFolder(HierarchyNode parent, string name)
        {
            var node = new FolderNode(parent.Id, CreateId(), name);
            AddChild(parent, node);
            return node;
        }

        private string CreateId()
        {
            return _root + "-" + Guid.NewGuid();
        }
    }

    
}
