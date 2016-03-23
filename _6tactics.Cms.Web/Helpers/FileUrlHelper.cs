using _6tactics.Utilities.Common;
using _6tactics.Utilities.FileSystem;
using System.IO;

namespace _6tactics.Cms.Web.Helpers
{
    public class FileUrlHelper
    {
        public bool IsUrlValid { get; private set; }
        public string RouteFolderPath { get; private set; }
        public string Filename { get; private set; }
        public string ExtensionName { get; private set; }
        public FileUrlHelper(string webFilePath)
        {
            IsUrlValid = CheckExistence.IsUrlExist(webFilePath);

            if (!IsUrlValid) return;

            string fullFilePath = FileSystemUtilities.ChangePathForwardSlashesToBack(webFilePath);

            string localFolderPath = fullFilePath.Replace(string.Concat("\\", Path.GetFileName(fullFilePath)), "");

            RouteFolderPath = FileSystemUtilities.ChangePathBackSlashesToMinuses(localFolderPath);

            Filename = FileSystemUtilities.GetFileNameFromString(fullFilePath);

            ExtensionName = Path.GetExtension(fullFilePath).Replace(".", "");
        }
    }
}