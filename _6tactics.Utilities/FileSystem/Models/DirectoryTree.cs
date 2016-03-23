using _6tactics.Utilities.FileSystem.Interfaces;
using System.Collections.Generic;

namespace _6tactics.Utilities.FileSystem.Models
{
    public class DirectoryTree : IDirectoryTree
    {
        public string ParentPath { get; set; }
        public int NumberOfParents { get; set; }
        public string Name { get; set; }
        public string BasePath { get; set; }
        public string Path { get; set; }
        public string WebPath => FileSystemUtilities.ChangePathBackSlashesToForward(Path.RemovePart(BasePath));
        public string RoutePath => FileSystemUtilities.ChangePathBackSlashesToMinuses(Path.RemovePart(BasePath));
        public List<IFile> Files { get; set; }
        public List<DirectoryTree> ChildDirectories { get; set; }

        public DirectoryTree()
        {
            Files = new List<IFile>();
            ChildDirectories = new List<DirectoryTree>();
        }
    }
}
