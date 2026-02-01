namespace AnagramSolver.Contracts
{
    public interface IFileSystemWrapper
    {
        IEnumerable<string> ReadLines(string path);
    }
}
