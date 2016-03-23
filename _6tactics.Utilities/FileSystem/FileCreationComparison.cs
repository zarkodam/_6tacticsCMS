using System;
using System.IO;

namespace _6tactics.Utilities.FileSystem
{
    public static class FileCreationComparison
    {
        public static bool IsFileOlderThan(DateTime dateTime, string path)
        {
            var file = new FileInfo(path);
            return dateTime <= file.CreationTimeUtc;
        }
    }
}
