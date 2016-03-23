using _6tactics.Utilities.FileSystem;
using System;
using System.Diagnostics;
using System.IO;
using System.Web;

namespace _6tactics.Utilities.Web
{
    public class FileUploader
    {
        public string FileName { get; private set; }
        public string CategoryAndFilePath { get; private set; }
        public string FileNameWithExtension { get; private set; }
        public int? FileId { get; private set; }
        public string FileExtension { get; private set; }
        public string FullFilePath { get; private set; }
        public string ServerUploadFolder { get; set; }
        public string FolderPath { get; private set; }
        public bool IsFileExist { get; private set; }

        private readonly HttpPostedFileBase _file;
        private readonly string _tempFileName;

        private static string WithRemovedDuplicatedExtension(string fileName, string extension)
        {
            return string.IsNullOrWhiteSpace(extension) ? fileName : fileName.Replace(extension, "");
        }

        public FileUploader() { }

        public FileUploader(HttpPostedFileBase file) { }

        public FileUploader(HttpPostedFileBase file, string serverUploadFolder, int? fileId, string fileName)
        {
            if (file != null)
            {
                _file = file;
                FileExtension = Path.GetExtension(_file.FileName);
                _tempFileName = DataManager.RemoveIllegalCharacters(_file.FileName);
            }

            string fileNameWithoutIllegals = DataManager.RemoveIllegalCharacters(fileName);

            if (fileNameWithoutIllegals != null)
            {
                if (!string.IsNullOrEmpty(Path.GetExtension(fileNameWithoutIllegals)))
                {
                    FileExtension = Path.GetExtension(fileNameWithoutIllegals);
                    _tempFileName = fileNameWithoutIllegals;
                }
                else
                    _tempFileName = string.Concat(fileNameWithoutIllegals, FileExtension);
            }

            FileName = WithRemovedDuplicatedExtension(_tempFileName, FileExtension);

            FileId = fileId;

            FileNameWithExtension = string.Concat(FileName, FileExtension);

            ServerUploadFolder = serverUploadFolder;

            FolderPath = Path.Combine(ServerUploadFolder, FileId.ToString());

            CategoryAndFilePath = Path.Combine(fileId.ToString(), FileNameWithExtension);

            FullFilePath = Path.Combine(serverUploadFolder, CategoryAndFilePath);

            IsFileExist = File.Exists(FileNameWithExtension);
        }

        public void UploadFile()
        {
            if (string.IsNullOrWhiteSpace(FullFilePath) || string.IsNullOrWhiteSpace(FolderPath)) return;

            if (Directory.Exists(FolderPath)) return;
            try
            {
                Directory.CreateDirectory(FolderPath);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            finally
            {
                _file.SaveAs(FullFilePath);
            }
        }

        public void RenameUploadedFile(string oldFileName, string newName, string extension)
        {
            if (string.IsNullOrWhiteSpace(oldFileName) || string.IsNullOrWhiteSpace(newName) || string.IsNullOrWhiteSpace(extension))
            {
                FileNameWithExtension = null;
                return;
            }

            var oldFileNameFullPath = Path.Combine(FolderPath, oldFileName);
            if (!File.Exists(oldFileNameFullPath)) return;

            FileNameWithExtension = string.Concat(WithRemovedDuplicatedExtension(newName, extension), extension);

            if (File.Exists(oldFileNameFullPath))
                File.Move(oldFileNameFullPath, Path.Combine(FolderPath, FileNameWithExtension));
        }

        public static void IsFileNameExistDeleteIt(string folderPath, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return;

            var filePath = Path.Combine(folderPath, fileName);

            if (!File.Exists(filePath)) return;

            DataManager.DeleteFile(filePath);
        }

        public static void IsFolderExistDeleteIt(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath)) return;

            if (!Directory.Exists(folderPath)) return;

            DataManager.DeleteDirectory(folderPath);
        }

        public void UploadFile(Action<int?, string> uploadFileNameInDb)
        {
            if (string.IsNullOrWhiteSpace(FileName) || _file == null) return;

            if (FileName != FileNameWithExtension)
                uploadFileNameInDb(FileId, FileNameWithExtension);
            UploadFile();
        }

        public void EditUploadedFile(string oldFileName, out string fileNameWithExtension)
        {
            if (_file != null || string.IsNullOrWhiteSpace(FileNameWithExtension))
                IsFolderExistDeleteIt(FolderPath);

            if (_file == null && !string.IsNullOrWhiteSpace(FileNameWithExtension))
                RenameUploadedFile(oldFileName, FileNameWithExtension, Path.GetExtension(oldFileName));

            if (_file != null)
                UploadFile();

            fileNameWithExtension = FileNameWithExtension;
        }

        public static string IfIsNewFileNameWithoutExtensionGetIt(HttpPostedFileBase file, string newFileName, string oldFileName)
        {
            string fileName = "";

            if (!string.IsNullOrWhiteSpace(Path.GetFileName(newFileName)))
                fileName = Path.GetFileName(newFileName);
            else if (string.IsNullOrWhiteSpace(Path.GetFileName(newFileName)) && file != null)
                fileName = Path.GetFileName(file.FileName);
            else if (string.IsNullOrWhiteSpace(Path.GetFileName(newFileName)) && file == null)
                fileName = Path.GetFileName(oldFileName);

            string extension = "";

            if (!string.IsNullOrWhiteSpace(Path.GetExtension(newFileName)))
                extension = Path.GetExtension(newFileName);
            else if (string.IsNullOrWhiteSpace(Path.GetExtension(newFileName)) && file != null)
                extension = Path.GetExtension(file.FileName);
            else if (string.IsNullOrWhiteSpace(Path.GetExtension(newFileName)) && file == null)
                extension = Path.GetExtension(oldFileName);

            return Path.GetExtension(DataManager.RemoveIllegalCharacters(string.Concat(fileName, extension)));
        }
    }
}

