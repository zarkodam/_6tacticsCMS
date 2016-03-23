using _6tactics.Utilities.FileSystem.Models;
using System.Collections.Generic;

namespace _6tactics.Utilities.FileSystem.Interfaces
{
    public interface IDirectoryTree : IDirectory
    {
        string ParentPath { get; set; }
        int NumberOfParents { get; set; }
        List<DirectoryTree> ChildDirectories { get; set; }
    }
}