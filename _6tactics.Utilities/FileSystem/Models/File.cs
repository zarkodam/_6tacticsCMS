using _6tactics.Utilities.FileSystem.Interfaces;

namespace _6tactics.Utilities.FileSystem.Models
{
    public class File : IFile
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
    }
}
