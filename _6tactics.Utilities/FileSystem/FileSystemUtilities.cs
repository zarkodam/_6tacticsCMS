using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace _6tactics.Utilities.FileSystem
{
    public class FileSystemUtilities
    {
        public static bool IsFileOlderThan(DateTime dateTime, string path)
        {
            var file = new FileInfo(path);
            return dateTime <= file.CreationTimeUtc;
        }

        public static string GetFileNameFromString(string currentFileName)
        {
            if (string.IsNullOrEmpty(currentFileName)) return "";

            var fileName = Path.GetFileName(currentFileName);
            return fileName.Replace(Path.GetExtension(fileName), "");
        }

        public static string ChangePathBackSlashesToForward(string path)
        {
            return string.IsNullOrWhiteSpace(path) ? null : path.Replace('\\', '/');
        }

        public static string ChangePathForwardSlashesToBack(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;

            string replacedPath = path.Replace('/', '\\');

            return replacedPath.IndexOf('\\') == 0 ? replacedPath.Remove(0, 1) : replacedPath;
        }

        public static string ChangePathForwardSlashesToMinuses(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;

            string replacedPath = path.Replace('/', '-');

            return replacedPath.IndexOf('-') == 0 ? replacedPath.Remove(0, 1) : replacedPath;
        }

        public static string ChangePathBackSlashesToMinuses(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;

            string replacedPath = path.Replace('\\', '-');

            return replacedPath.IndexOf('-') == 0 ? replacedPath.Remove(0, 1) : replacedPath;
        }

        public static string ChangePathMinusesToBackSlashes(string path)
        {
            return string.IsNullOrWhiteSpace(path) ? null : path.Replace('-', '\\');
        }

        public static string ChangePathMinusesToForwardSlashes(string path)
        {
            return string.IsNullOrWhiteSpace(path) ? null : path.Replace('-', '/');
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
