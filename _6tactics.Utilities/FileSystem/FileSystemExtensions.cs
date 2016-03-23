using _6tactics.Utilities.FileSystem.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _6tactics.Utilities.FileSystem
{
    public static class FileSystemExtensions
    {
        public static string GetFileNameFromString(this string source, char splitBy, bool withExtension = false)
        {
            string[] pathParts = source.Split(splitBy);
            string filename = pathParts.Last();
            return withExtension ? filename : filename.Replace(Path.GetExtension(filename), "");
        }

        public static string GetFileExtensionFromString(this string source)
        {
            string[] pathParts = source.Split('.');
            string extension = pathParts.Last();
            return string.IsNullOrEmpty(extension) ? "" : extension;
        }

        public static string RemovePart(this string path, string toRemove)
        {
            return path.Replace(toRemove, "");
        }

        public static string GetLastDirectoryNode(this string directoryPath)
        {
            var directoryNodes = directoryPath.Split('\\');
            return directoryNodes.LastOrDefault();
        }

        public static void DeleteDirectoryWithContent(this List<IDirectory> source)
        {
            source.Reverse();

            foreach (var file in source.SelectMany(directory => directory.Files))
                File.Delete(file.Path);

            foreach (IDirectory directory in source)
                Directory.Delete(directory.Path);
        }

        public static void DeleteFilesFromDirectory(this DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles()) file.Delete();
            foreach (var subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }

        public static long GetSizeInBytes(this List<IFile> files)
        {
            return files.Sum(i => i.Size);
        }

        public static long GetSizeInMegabytes(this List<IFile> files)
        {
            return GetSizeInBytes(files) / 1024 / 1024;
        }

        public static long GetSizeInBytes(this List<IDirectory> directories)
        {
            return directories.SelectMany(i => i.Files).Sum(i => i.Size);
        }

        public static long GetSizeInMegabytes(this List<IDirectory> directories)
        {
            return GetSizeInBytes(directories) / 1024 / 1024;
        }
    }
}
