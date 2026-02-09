using AnagramSolver.Contracts;

namespace AnagramSolver.WebApp.GraphQL
{
    public class Query
    {
        public async Task<IEnumerable<string>> GetAnagrams([Service] IWordProcessor processor, string word)
        {
            var result = await processor.GetAnagramsAsync(word, 2, 2);
            return result.Select(a => a.Word);
        }

        public List<string> GetWords([Service] IWordProcessor processor)
        {
            return processor.GetDictionary();
        }
    }
}
