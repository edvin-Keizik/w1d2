namespace AnagramSolver.Contracts
{
    public interface IWordProcessor
    {
        List<Anagram> GetAnagrams(string input, int maxAnagramsToShow, int minWordLength);
        bool AddWord(string word);

        List<string> GetDictionary();
    }
}
