using _6tactics.Utilities.StringUtilities;
using System.Web;

namespace _6tactics.Utilities.Web
{
    public static class FileUrlBuilder
    {
        public static readonly string ServerProtocol = HttpContext.Current.Request.IsSecureConnection ? "https://" : "http://";
        public static readonly string ServerAddress = HttpContext.Current.Request.Url.Authority;

        public static string CreateLocalUrl(this string path)
        {
            return string.Concat(ServerProtocol, ServerAddress, path);
        }

        public static string AddProtocolToUrl(this string url, string protocolToAdd)
        {
            return url.CustomContains("http") ? url : string.Concat(protocolToAdd, "://", url);
        }
    }
}

