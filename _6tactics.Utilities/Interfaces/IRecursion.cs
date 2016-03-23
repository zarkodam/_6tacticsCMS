namespace _6tactics.Utilities.Interfaces
{
    public interface IRecursion<out T>
    {
        int Depth { get; }
        T Item { get; }
    }
}
