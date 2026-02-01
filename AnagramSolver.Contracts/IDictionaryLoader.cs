namespace AnagramSolver.Contracts
{
    public interface IDictionaryLoader
    {
        void LoadWords(string path, IWordProcessor processor);
    }
}
