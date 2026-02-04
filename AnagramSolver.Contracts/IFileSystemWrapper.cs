namespace AnagramSolver.Contracts
{
    public interface IFileSystemWrapper
    {
        IAsyncEnumerable<string> ReadLinesAsync(string path);
    }
}
