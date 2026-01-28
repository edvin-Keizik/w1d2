using AnagramSolver.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnagramSolver.BusinessLogic
{
    public class WordProcessor : IWordProcessor
    {
        private Dictionary<string, List<string>> _wordGroups = new Dictionary<string, List<string>>();

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

        public List<Anagram> GetAnagrams(string input)
        {
            string[] words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            List<Anagram> result = new List<Anagram>();

            foreach (string word in words)
            {
                string signature = GetSignature(word.ToLower());

                if (_wordGroups.ContainsKey(signature))
                {
                    var firstMatch = _wordGroups[signature].First();
                    result.Add(new Anagram { Word = firstMatch});
                }
                else
                {
                    result.Add(new Anagram { Word = $"[Nera anagramos zodziui {word}"});
                }
            }
            return result;

        }
    }
}
