using System;
using System.Diagnostics;
using System.IO;

namespace _6tactics.Utilities.FileSystem
{
    public static class FileSystemWriter
    {
        public static void CopyFile(string source, string destination, bool copyOverride = true)
        {
            try
            {
                if (!Directory.Exists(destination))
                    Directory.CreateDirectory(destination);

                File.Copy(source, destination, copyOverride);
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static void CopyFiles(string source, string destination, bool copyOverride = true)
        {
            try
            {
                if (!Directory.Exists(destination))
                    Directory.CreateDirectory(destination);

                var files = Directory.GetFiles(source);

                foreach (var file in files)
                {
                    File.Copy(file, Path.Combine(destination, file), copyOverride);
                    Trace.WriteLine(file);
                }
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static void MoveFile(string source, string destionation)
        {
            try
            {
                if (File.Exists(source))
                    File.Move(source, destionation);
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static void CreateDirectory(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static void MoveDirectory(string source, string destionation)
        {
            try
            {
                if (Directory.Exists(source))
                    Directory.Move(source, destionation);
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static void DeleteFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath)) return;

                File.Delete(filePath);
            }
            catch (IOException e)
            {
                Trace.WriteLine(e.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static void EmptyDirectory(string path)
        {
            try
            {
                var directory = new DirectoryInfo(path);
                directory.DeleteFilesFromDirectory();
            }
            catch (IOException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static void DeleteDirectoryWithFiles(string destination)
        {
            Debug.WriteLine("Delete destination: " + destination);

            try
            {
                if (!Directory.Exists(destination)) return;

                var fileReader = new FileSystemReader(destination);
                fileReader.GetDirectories(true).DeleteDirectoryWithContent();
            }
            catch (IOException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}