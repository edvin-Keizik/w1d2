namespace AnagramSolver.Contracts
{
    public interface IDictionaryLoader
    {
        Task LoadWordsAsync(string path, IWordProcessor processor);
        Task<IEnumerable<string>> GetWordsAsync(IWordProcessor processor);
        Task AddWordAsync(string path, string word, IWordProcessor processor);
        Task<bool> DeleteWordAsync(string path, int lineIndex, IWordProcessor processor);
    }
}
