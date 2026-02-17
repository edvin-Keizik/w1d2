namespace AnagramSolver.Contracts
{
    public interface IWordProcessor
    {
        bool AddWord(string word);
        List<string> GetDictionary();
        void LoadDictionary(List<string> newWords);
    }
}
