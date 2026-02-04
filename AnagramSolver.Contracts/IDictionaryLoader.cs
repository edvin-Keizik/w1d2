namespace AnagramSolver.Contracts
{
    public interface IDictionaryLoader
    {
        Task LoadWordsAsync(string path, IWordProcessor processor);
    }
}
