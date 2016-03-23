using System.Collections.Generic;
using System.IO;
using Directory = _6tactics.Utilities.FileSystem.Models.Directory;
using File = _6tactics.Utilities.FileSystem.Models.File;

namespace _6tactics.Utilities.FileSystem
{
    public class FilesAndFolderReader
    {
        private readonly List<Directory> _directories = new List<Directory>();
        private Directory _directory = new Directory();
        private readonly List<File> _files = new List<File>();
        private readonly string _sourcePath;

        public FilesAndFolderReader(string sourcePath)
        {
            _sourcePath = sourcePath;
        }



        private void CollectDirectoriesAsTree(string path)
        {
            //_directory.Name = Path.GetFileName(path);
            //_directory.Path = path;

            foreach (var directoryPath in System.IO.Directory.EnumerateDirectories(path))
            {
                _directory.ChildDirectories.Add(new Directory
                {
                    Name = Path.GetFileName(directoryPath),
                    Path = directoryPath
                });

                CollectDirectoriesAsTree(directoryPath);
            }
        }

        public Directory GetDirectoriesAsTree()
        {
            // Add root folder
            _directory = new Directory { Name = Path.GetDirectoryName(_sourcePath), Path = _sourcePath };
            CollectDirectoriesAsTree(_sourcePath);
            return _directory;
        }

        private void CollectDirectories(string path)
        {
            foreach (var directoryPath in System.IO.Directory.EnumerateDirectories(path))
            {
                _directories.Add(new Directory { Name = Path.GetDirectoryName(directoryPath), Path = directoryPath });
                CollectDirectories(directoryPath);
            }
        }

        public List<Directory> GetDirectories()
        {
            // Add root folder
            _directories.Add(new Directory { Name = Path.GetDirectoryName(_sourcePath), Path = _sourcePath });
            CollectDirectories(_sourcePath);
            return _directories;
        }

        public List<File> GetFiles(string fileExtensionFilter = "*.*")
        {
            foreach (var directory in _directories)
                foreach (var filePath in System.IO.Directory.EnumerateFiles(directory.Path, fileExtensionFilter))
                {
                    _files.Add(new File
                    {
                        Name = Path.GetFileName(filePath),
                        Extension = Path.GetExtension(filePath),
                        Path = filePath,
                        Size = new FileInfo(filePath).Length
                    });
                }

            return _files;
        }
    }
}
