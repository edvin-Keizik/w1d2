namespace AnagramSolver.Contracts
{
    public interface IWordProcessor
    {
        List<Anagram> GetAnagrams(string input, int maxAnagramsToShow, int minWordLength);
        void AddWord(string word);
    }
}
