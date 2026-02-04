using AnagramSolver.Contracts;

namespace AnagramSolver.BusinessLogic
{
    public class FileSystemWrapper : IFileSystemWrapper
    {
        public async IAsyncEnumerable<string> ReadLinesAsync(string path)
        {
            using var reader = new StreamReader(path, System.Text.Encoding.UTF8);
            while (await reader.ReadLineAsync() is { } line)
            {
                yield return line;
            }
        }
    }
}
