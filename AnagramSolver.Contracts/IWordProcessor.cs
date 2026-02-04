namespace AnagramSolver.Contracts
{
    public interface IWordProcessor
    {
        Task<IEnumerable<Anagram>> GetAnagramsAsync(string input, int maxAnagramsToShow, int minWordLength, CancellationToken ct = default);
        bool AddWord(string word);

        List<string> GetDictionary();
    }
}
