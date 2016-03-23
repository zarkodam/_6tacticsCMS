using Newtonsoft.Json;

namespace _6tactics.Cms.Core.ViewModels.FileManager
{
    public class FileManagerFileForPreviewViewModel
    {
        [JsonProperty("path")]
        public string WebPath { get; set; }
        [JsonProperty("name")]
        public string FileName { get; set; }
        [JsonProperty("filenamewithpath")]
        public string FileNameWithPath => string.Concat(WebPath, "/", FileName);
        [JsonProperty("type")]
        public string MimeType { get; set; }
        [JsonProperty("extension")]
        public string Extension { get; set; }
        [JsonProperty("size")]
        public long Size { get; set; }
    }
}
