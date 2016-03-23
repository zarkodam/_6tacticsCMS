using FtpLib;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace _6tactics.Utilities.Network
{
    public class FtpHandler
    {
        private readonly string _serverAdress;
        private readonly string _ftpUserName;
        private readonly string _ftpPass;

        public FtpHandler(string serverAdress, string ftpUsername, string ftpPass)
        {
            _serverAdress = serverAdress;
            _ftpUserName = ftpUsername;
            _ftpPass = ftpPass;
        }

        private string StringToUtf8(string stringFromConvert)
        {
            byte[] bytes = Encoding.Default.GetBytes(stringFromConvert);
            return Encoding.UTF8.GetString(bytes);
        }

        public void UploadFile(string localFilePathForUpload, string destinationPath = "/")
        {
            using (var ftp = new FtpConnection(_serverAdress, _ftpUserName, _ftpPass))
            {
                ftp.Open();
                ftp.Login();
                try
                {
                    ftp.SetCurrentDirectory(destinationPath);
                    ftp.PutFile(localFilePathForUpload);
                }
                catch (FtpException e)
                {
                    Trace.WriteLine(String.Format("FTP Error: {0} {1}", e.ErrorCode, e.Message));
                }
            }
        }

        public void DownloadFile(string fileName, string localPath, string remotePath = "/")
        {
            try
            {
                using (var ftp = new FtpConnection(_serverAdress, _ftpUserName, _ftpPass))
                {
                    ftp.Open();
                    ftp.Login();


                    if (ftp.DirectoryExists(remotePath))
                        ftp.SetCurrentDirectory(remotePath);

                    if (!ftp.FileExists(string.Concat(remotePath, fileName))) return;

                    if (!Directory.Exists(localPath))
                        Directory.CreateDirectory(localPath);

                    ftp.GetFile(string.Concat(remotePath, fileName), Path.Combine(localPath, fileName), false);
                    Trace.WriteLine("Ftp get file passed!");
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
        }

        public void GetContentList(string ftpDirectoryPath = "/")
        {
            using (var ftp = new FtpConnection(_serverAdress, _ftpUserName, _ftpPass))
            {
                ftp.Open();
                ftp.Login();

            }
        }

    }
}
