using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace _6tactics.Utilities.FileSystem
{
    // TODO: REMOVE AFTER IMPLEMENTATION OF FILE MANAGER 
    public static class DataManager
    {
        //static long GetDirectorySize(string p)
        //{
        //    // 1.
        //    // Get array of all file names.
        //    string[] a = Directory.GetFiles(p, "*.*");

        //    // 2.
        //    // Calculate total bytes of all files in a loop.
        //    long b = 0;
        //    foreach (string name in a)
        //    {
        //        // 3.
        //        // Use FileInfo to get length of each file.
        //        FileInfo info = new FileInfo(name);
        //        b += info.Length;
        //    }
        //    // 4.
        //    // Return total size
        //    return b;
        //}

        //public static void GetDirectorySize(string path)
        //{
        //    if (Directory.Exists(path))
        //    {
        //        foreach(var directory in )
        //    }
        //}

        public static void CopyFile(string fullSourcePath, string destination, string rootFolderName, string subFolderName, string destFileName)
        {
            var destFolder = Path.Combine(destination, rootFolderName, subFolderName);
            var destFile = Path.Combine(destFolder, string.Concat(destFileName, Path.GetExtension(fullSourcePath)));

            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            try
            {
                File.Copy(fullSourcePath, destFile, true);
            }
            catch (IOException e)
            {
                Trace.WriteLine(e.Message);
            }
        }

        public static void CopyFiles(string source, string destination, string rootFolderName, string subFolderName)
        {
            var destFolder = Path.Combine(destination, rootFolderName, subFolderName);

            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            var files = Directory.GetFiles(source);
            foreach (var file in files)
            {
                File.Copy(file, Path.Combine(destFolder, file), true);
                Trace.WriteLine(file);
            }
        }

        public static void MoveFile(string source, string destionation)
        {
            try
            {
                File.Move(source, destionation);
            }
            catch (IOException e)
            {
                Trace.WriteLine(e.Message);
            }
        }

        public static void MoveDirectory(string source, string destionation)
        {
            try
            {
                Directory.Move(source, destionation);
            }
            catch (IOException e)
            {
                Trace.WriteLine(e.Message);
            }
        }

        public static void DeleteFile(string filePath)
        {
            if (!File.Exists(filePath)) return;

            try
            {
                File.Delete(filePath);
            }
            catch (IOException e)
            {
                Trace.WriteLine(e.Message);
            }
        }

        private static void Empty(this DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles()) file.Delete();
            foreach (var subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }

        public static void EmptyDirectory(string path)
        {
            var directory = new DirectoryInfo(path);
            directory.Empty();
        }

        public static void DeleteDirectory(string destination)
        {
            if (!Directory.Exists(destination)) return;

            try
            {
                Directory.Delete(destination, true);
            }
            catch (IOException e)
            {
                Trace.WriteLine("DeleteDirectory: " + e.Message);
            }
        }

        public static string GetFileNameFromString(string currentFileName)
        {
            if (string.IsNullOrEmpty(currentFileName)) return "";

            var fileName = Path.GetFileName(currentFileName);
            return fileName.Replace(Path.GetExtension(fileName), "");
        }

        public static string RemoveIllegalCharacters(string fileName)
        {
            var defaultIllegals = Path.GetInvalidFileNameChars();
            var customIllegals = new[] { "!", "#", "$", "%", "&", "=", "*", "/", ";", " " };

            var withoutCustomIllegals = customIllegals.Aggregate(fileName,
                (current, c) => current.Replace(c.ToString(CultureInfo.InvariantCulture), string.Empty));

            var withoutDefaultIllegals = defaultIllegals.Aggregate(withoutCustomIllegals,
                (current, c) => current.Replace(c.ToString(CultureInfo.InvariantCulture), string.Empty));

            return withoutDefaultIllegals;
        }
    }
}