namespace _6tactics.Utilities.FileSystem.Interfaces
{
    public interface IFile
    {
        string Name { get; set; }
        string Path { get; set; }
        string Extension { get; set; }
        long Size { get; set; }
    }
}