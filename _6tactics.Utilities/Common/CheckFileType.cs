using _6tactics.Utilities.Network;
using System.IO;

namespace _6tactics.Utilities.Common
{
    public static class CheckFileType
    {
        public static bool IsFileImage(string fileName)
        {
            return MimeTypeMap.GetMimeType(Path.GetExtension(fileName)).Contains("image");
        }

        public static bool IsFileCompressed(string fileName)
        {
            return MimeTypeMap.GetMimeType(Path.GetExtension(fileName)).Contains("compressed") ||
                MimeTypeMap.GetMimeType(Path.GetExtension(fileName)).Contains("zip");
        }

        public static bool IsFileDocument(string fileName)
        {
            var ext = Path.GetExtension(fileName);
            if (ext == null) return false;
            var extLower = ext.ToLower();

            return extLower.Equals(".doc") || extLower.Equals(".docx") || extLower.Equals(".xls")
                || extLower.Equals(".xlsx") || extLower.Equals(".ppt") || extLower.Equals(".pptx")
                || extLower.Equals(".pdf") || extLower.Equals(".txt") || extLower.Equals(".epub");
        }

        // TODO: DEPRECATED!!!!
        public static bool IsFileExtensionApproved(string fileName)
        {
            return !string.IsNullOrWhiteSpace(Path.GetExtension(fileName)) && (IsFileCompressed(fileName) || IsFileImage(fileName) || IsFileDocument(fileName));
        }
    }
}
