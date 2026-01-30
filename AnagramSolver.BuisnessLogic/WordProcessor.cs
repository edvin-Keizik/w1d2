using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using System.Text;

namespace AnagramSolver.BusinessLogic
{
    public class WordProcessor : IWordProcessor
    {
        private readonly Dictionary<string, List<string>> _wordGroups = new();
        private readonly IAnagramSearchEngine _searchEngine;

        public WordProcessor(IAnagramSearchEngine searchEngine)
        {
            _searchEngine = searchEngine;
        }
        public void AddWord(string word)
        {
            string signature = GetSignature(word.ToLower());

            if (_wordGroups.ContainsKey(signature))
            {
                _wordGroups[signature].Add(word);
            }

            else
            {
                List<string> newWordList = new List<string>();
                newWordList.Add(word);

                _wordGroups.Add(signature, newWordList);
            }
        }

        private string GetSignature(string word)
        {
            char[] wordArray = word.ToCharArray();

            Array.Sort(wordArray);

            return new string(wordArray);
        }
        private List<string> GetCandidatesKeys(string letterBank)
        {
            var candidates = new List<string>();

            foreach(var signature in _wordGroups.Keys)
            {
                if (signature.Length > letterBank.Length) continue;

                if(_searchEngine.CanSubstract(letterBank, signature, out _))
                {
                    candidates.Add(signature);
                }
            }
            return candidates.OrderByDescending(s => s.Length).ToList();
        }


        public List<Anagram> GetAnagrams(string input, int maxAnagramsToShow)
        {
            string letterBank = GetSignature(input.Replace(" ", "").ToLower());
            var originalWords = input.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            List<string> candidates = GetCandidatesKeys(letterBank);
            List<List<string>> allResults = new List<List<string>>();

            for(int i = maxAnagramsToShow; i>= 1; i--)
            {
                _searchEngine.FindAllCombinations(letterBank, i, new List<string>(), candidates, _wordGroups, allResults, originalWords);

                if (allResults.Any())
                {
                    return allResults
                        .Select(combination => new Anagram { Word = string.Join(" ", combination) })
                        .ToList();
                }
                
            }

            return new List<Anagram>();
        }
    }
}
