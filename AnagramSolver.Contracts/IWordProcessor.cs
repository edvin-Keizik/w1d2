namespace AnagramSolver.Contracts
{
    public interface IWordProcessor
    {
        Task<IEnumerable<Anagram>> GetAnagramsAsync(string input, int maxAnagramsToShow, Func<string, bool> filter, CancellationToken ct = default);
        bool AddWord(string word);
        List<string> GetDictionary();
        void LoadDictionary(List<string> newWords);
    }
}
