namespace AnagramSolver.Contracts
{
    public interface IWordProcessor
    {
        Task<List<string>> GetDictionary();
        Task<bool> AddWordAsync(string word);
    }
}
