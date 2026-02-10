using AnagramSolver.Contracts;

namespace AnagramSolver.BusinessLogic
{
    public class WordProcessor : IWordProcessor
    {
        private readonly Dictionary<string, List<string>> _wordGroups = new();
        private readonly IAnagramSearchEngine _searchEngine;
        private readonly MemoryCache<IEnumerable<Anagram>> _cache = new();

        public WordProcessor(IAnagramSearchEngine searchEngine)
        {
            _searchEngine = searchEngine;
        }
        public bool AddWord(string word)
        {
            string signature = GetSignature(word.ToLower());
            if (!_wordGroups.ContainsKey(signature))
            {
                _wordGroups[signature] = new List<string>();
            }
            if (_wordGroups[signature].Contains(word))
            {
                return false;
            }

            _wordGroups[signature].Add(word);      
            return true;
        }

        public List<string> GetDictionary()
        {
            List<string> words = _wordGroups.Values.SelectMany(list => list).ToList();
            return words; 
        }

        public void LoadDictionary(List<string> dictionary)
        {
            _wordGroups.Clear();
            foreach(string word in dictionary)
            {
                AddWord(word);
            }
        }

        private string GetSignature(string word)
        {
            char[] wordArray = word.ToCharArray();

            Array.Sort(wordArray);

            return new string(wordArray);
        }
        private async Task<List<string>> GetCandidatesKeysAsync(string letterBank)
        {
            return await Task.Run(() =>
            {
                var candidates = new List<string>();

                foreach (var signature in _wordGroups.Keys)
                {
                    if (signature.Length > letterBank.Length) continue;

                    if (_searchEngine.CanSubstract(letterBank, signature, out _))
                    {
                        candidates.Add(signature);
                    }
                }
                return candidates.OrderByDescending(s => s.Length).ToList();
            });
        }


        public async Task<IEnumerable<Anagram>> GetAnagramsAsync(string input, int maxAnagramsToShow, int minWordLength, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            if(_cache.TryGetCache(input, out var cachedAnagrams))
            {
                return cachedAnagrams;
            }

            string letterBank = GetSignature(input.Replace(" ", "").ToLower());
            var originalWords = input.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var candidates = await GetCandidatesKeysAsync(letterBank);

            var searchTasks = Enumerable.Range(1, maxAnagramsToShow).Select(i =>
            {
                return Task.Run(() =>
                {
                    var resultsForThisCount = new List<List<string>>();
                    _searchEngine.FindAllCombinations(
                        letterBank,
                        i,
                        new List<string>(),
                        candidates,
                        _wordGroups,
                        resultsForThisCount,
                        originalWords);
                    return resultsForThisCount;
                }, ct);
            });

            var allTaskResults = await Task.WhenAll(searchTasks);

            var finalResults = allTaskResults
                .SelectMany(list => list)
                .Where(combination => combination.All(word => word.Length >= minWordLength))
                .Select(combination => new Anagram { Word = string.Join(" ", combination) })
                .ToList();

            _cache.AddCache(input, finalResults);

            return finalResults;
        }
    }
}
