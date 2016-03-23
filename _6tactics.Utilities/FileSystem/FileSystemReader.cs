using _6tactics.Utilities.FileSystem.Interfaces;
using _6tactics.Utilities.FileSystem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Directory = _6tactics.Utilities.FileSystem.Models.Directory;
using File = _6tactics.Utilities.FileSystem.Models.File;

namespace _6tactics.Utilities.FileSystem
{
    public class FileSystemReader
    {
        #region Fields

        private readonly List<IDirectory> _directories = new List<IDirectory>();
        private IDirectoryTree _directoryTree = new DirectoryTree();
        private readonly string _basePath;
        private readonly string _sourcePath;

        #endregion


        #region Constructors

        public FileSystemReader(string basePath, string concatWithBasePath)
        {
            _basePath = basePath;
            _sourcePath = Path.Combine(basePath, concatWithBasePath);
        }

        public FileSystemReader(string sourcePath)
        {
            _basePath = sourcePath;
            _sourcePath = sourcePath;
        }

        #endregion


        #region Files

        private List<IFile> CollectFiles(string path, string fileExtensionFilter = "*.*")
        {
            var files = new List<IFile>();

            try
            {
                foreach (var filePath in System.IO.Directory.EnumerateFiles(path, fileExtensionFilter))
                {
                    files.Add(new File
                    {
                        Name = Path.GetFileName(filePath),
                        Extension = Path.GetExtension(filePath),
                        Path = filePath,
                        Size = new FileInfo(filePath).Length
                    });
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

            return files;
        }

        public List<IFile> GetFiles(string fileExtensionFilter = "*.*")
        {
            return CollectFiles(_sourcePath, fileExtensionFilter);
        }

        #endregion


        #region DirectoriesTree

        private void AddDirectoryToProperPlaceInList(string parentPath, DirectoryTree elementToAdd, IDirectoryTree directoryLevel,
            bool addFiles = false, string fileExtensionFilter = "*.*")
        {
            if (!_directoryTree.ChildDirectories.Contains(elementToAdd) && _directoryTree.Path.Equals(elementToAdd.ParentPath))
            {
                if (addFiles)
                {
                    var elementToAddWithFiles = elementToAdd;
                    elementToAddWithFiles.Files = CollectFiles(elementToAdd.Path, fileExtensionFilter);
                    _directoryTree.ChildDirectories.Add(elementToAddWithFiles);
                }
                else
                    _directoryTree.ChildDirectories.Add(elementToAdd);
            }

            foreach (IDirectoryTree directory in directoryLevel.ChildDirectories)
            {
                if (directory.Path.Equals(parentPath))
                {
                    directory.ChildDirectories.Add(elementToAdd);
                }
                AddDirectoryToProperPlaceInList(parentPath, elementToAdd, directory);
            }
        }

        private int _counter;
        private int _counterGroup = 1;
        private void CollectDirectoriesAsTree(string path, bool addFiles = false, string fileExtensionFilter = "*.*")
        {
            _counter += 1;

            foreach (string directoryPath in System.IO.Directory.EnumerateDirectories(path))
            {
                var directoryToAdd = new DirectoryTree
                {
                    BasePath = _basePath,
                    Name = directoryPath.GetLastDirectoryNode(),
                    NumberOfParents = _counter,
                    ParentPath = path,
                    Path = directoryPath
                };

                AddDirectoryToProperPlaceInList(path, directoryToAdd, _directoryTree, addFiles, fileExtensionFilter);

                CollectDirectoriesAsTree(directoryPath);
            }

            _counterGroup = _counter - 1;
            _counter = _counterGroup;
        }

        public IDirectoryTree GetDirectoriesTree(bool addFiles = false, string fileExtensionFilter = "*.*")
        {
            try
            {
                // Add root folder
                IDirectoryTree rootElement = addFiles
                    ? new DirectoryTree
                    {
                        BasePath = _basePath,
                        Name = _sourcePath.GetLastDirectoryNode(),
                        Path = _sourcePath
                    }
                    : new DirectoryTree
                    {
                        BasePath = _basePath,
                        Name = _sourcePath.GetLastDirectoryNode(),
                        Path = _sourcePath,
                        Files = CollectFiles(_sourcePath, fileExtensionFilter)
                    };

                _directoryTree = rootElement;

                CollectDirectoriesAsTree(_sourcePath, addFiles, fileExtensionFilter);
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return _directoryTree;
        }

        #endregion


        #region Directories

        private void CollectDirectories(string path, bool addFiles = false, string fileExtensionFilter = "*.*")
        {
            foreach (var directoryPath in System.IO.Directory.EnumerateDirectories(path))
            {
                IDirectory elementToAdd;

                if (addFiles)
                {
                    elementToAdd = new Directory
                    {
                        BasePath = _basePath,
                        Name = directoryPath.GetLastDirectoryNode(),
                        Path = directoryPath,
                        Files = CollectFiles(directoryPath, fileExtensionFilter)
                    };
                }
                else
                    elementToAdd = new Directory { BasePath = _basePath, Name = directoryPath.GetLastDirectoryNode(), Path = directoryPath };

                _directories.Add(elementToAdd);

                CollectDirectories(directoryPath, addFiles, fileExtensionFilter);
            }
        }

        public List<IDirectory> GetDirectories(bool addFiles = false, string fileExtensionFilter = "*.*")
        {
            try
            {
                // Add root folder
                IDirectory rootElement = addFiles
                    ? new Directory
                    {
                        BasePath = _basePath,
                        Name = _sourcePath.GetLastDirectoryNode(),
                        Path = _sourcePath,
                        Files = CollectFiles(_sourcePath, fileExtensionFilter)
                    }
                    : new Directory
                    {
                        BasePath = _basePath,
                        Name = _sourcePath.GetLastDirectoryNode(),
                        Path = _sourcePath,
                    };

                _directories.Add(rootElement);

                CollectDirectories(_sourcePath, addFiles, fileExtensionFilter);
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return _directories;
        }


        #endregion
    }
}
