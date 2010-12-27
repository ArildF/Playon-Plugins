//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Rogue.PlayOn.Plugins.Channel9
//{
//    class HierarchyBuilder
//    {
//        private readonly VirtualHierarchy _hierarchy;

//        private HierarchyBuilder(VirtualHierarchy hierarchy)
//        {
//            _hierarchy = hierarchy;
//        }

//        public static HierarchyBuilder For(VirtualHierarchy hierarchy)
//        {
//            return new HierarchyBuilder(hierarchy);
//        }

//        public HierarchyBuilder WithChildren(params Child[] children)
//        {
//            foreach (var child in children)
//            {
//                child.AddToHierarchy(_hierarchy.Root, _hierarchy);

//            }
//        }
//    }

    

//    internal class FolderBuilder
//    {
//        private readonly string _name;

//        public FolderBuilder(string name)
//        {
//            _name = name;
//        }


//        public static implicit operator Child(FolderBuilder builder)
//        {
//            return builder.CreateChild();
//        }

//    }

//    internal abstract class Child
//    {

//        public abstract void AddToHierarchy(HierarchyNode parent, VirtualHierarchy virtualHierarchy);
//    }

//    internal class Folder : Child
//    {

//        public static FolderBuilder WithName(string name)
//        {
//            return new FolderBuilder(name);
//        }

//        public override void AddToHierarchy(HierarchyNode parent, VirtualHierarchy hierarchy)
//        {
//            hierarchy.Add
//        }
//    }
//}
