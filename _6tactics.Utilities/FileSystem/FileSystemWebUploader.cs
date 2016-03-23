using System;
using System.Diagnostics;
using System.IO;
using System.Web;

namespace _6tactics.Utilities.FileSystem
{
    public class FileSystemWebUploader
    {
        private readonly string _folderPath;
        private readonly HttpPostedFileBase _file;

        public FileSystemWebUploader(string folderPath, HttpPostedFileBase file)
        {
            _folderPath = folderPath;
            _file = file;
        }

        public void Upload()
        {
            _file.SaveAs(Path.Combine(_folderPath, _file.FileName));
        }

        public void Upload(string newFilename)
        {
            try
            {
                Upload();

                string source = Path.Combine(_folderPath, _file.FileName);
                string destination = Path.Combine(_folderPath, newFilename);

                FileSystemWriter.MoveFile(source, destination);
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
    }
}
