namespace Rogue.PlayOnPlugins.Hierarchies
{
    public class FolderStructure
    {
        private readonly string _folderContainerTag;
        private readonly string _folderContainerClass;
        private readonly string _linkClass;

        public FolderStructure(string folderContainerTag, string folderContainerClass, string linkClass)
        {
            _folderContainerTag = folderContainerTag;
            _linkClass = linkClass;
            _folderContainerClass = folderContainerClass;
        }

        public string FolderContainerTag
        {
            get { return _folderContainerTag; }
        }

        public string FolderContainerClass
        {
            get { return _folderContainerClass; }
        }

        public string LinkClass
        {
            get {
                return _linkClass;
            }
        }
    }
}