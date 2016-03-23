using System.Collections.Generic;

namespace _6tactics.Utilities.FileSystem.Interfaces
{
    public interface IDirectory
    {
        string Name { get; set; }
        string BasePath { get; set; }
        string Path { get; set; }
        string WebPath { get; }
        string RoutePath { get; }
        List<IFile> Files { get; set; }
    }
}