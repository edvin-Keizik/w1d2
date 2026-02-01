using AnagramSolver.Contracts;

namespace AnagramSolver.BusinessLogic
{
    public class FileSystemWrapper : IFileSystemWrapper
    {
        public IEnumerable<string> ReadLines(string path)
        {
            using (var reader = new StreamReader(path, System.Text.Encoding.UTF8))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
