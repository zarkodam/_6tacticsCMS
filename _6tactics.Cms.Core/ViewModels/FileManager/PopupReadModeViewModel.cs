using _6tactics.Utilities.FileSystem.Interfaces;

namespace _6tactics.Cms.Core.ViewModels.FileManager
{
    public class PopupReadModeViewModel
    {
        public IDirectoryTree DirectoryTree { get; set; }
        public string InputIdToShowFileUrl { get; set; }
    }
}
