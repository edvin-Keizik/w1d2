using AnagramSolver.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BuisnessLogic
{
    public class AnagramSearchEngine : IAnagramSearchEngine
    {
        public bool CanSubstract(string letterBank, string signature, out string leftover)
        {
            leftover = "";
            StringBuilder remainig = new StringBuilder();
            int pointerLetterBank = 0;
            int pointerSignature = 0;

            while (pointerLetterBank < letterBank.Length && pointerSignature < signature.Length)
            {
                if (letterBank[pointerLetterBank] == signature[pointerSignature])
                {
                    pointerLetterBank++;
                    pointerSignature++;
                }
                else if (letterBank[pointerLetterBank] < signature[pointerSignature])
                {
                    remainig.Append(letterBank[pointerLetterBank]);
                    pointerLetterBank++;
                }
                else return false;
            }

            if (pointerSignature != signature.Length) return false;

            while (pointerLetterBank < letterBank.Length) remainig.Append(letterBank[pointerLetterBank++]);

            leftover = remainig.ToString();
            return true;
        }

        public void FindAllCombinations(
            string remainingLetters,
            int wordsNeeded,
            List<string> currentPath,
            List<string> candidates,
            Dictionary<string, List<string>> wordGroups,
            List<List<string>> allResults,
            IEnumerable<string> originalWords)
        {
            if (wordsNeeded == 0)
            {
                if(remainingLetters.Length == 0)
                {
                    allResults.Add(new List<string>(currentPath));
                }
                return;
            }

            foreach (var signature in candidates)
            {
                if (CanSubstract(remainingLetters, signature, out string leftovers))
                {
                    foreach (var word in wordGroups[signature])
                    {
                        if (originalWords.Contains(word.ToLower())) continue;

                        currentPath.Add(word);

                        FindAllCombinations(leftovers, wordsNeeded - 1, currentPath, candidates, wordGroups, allResults, originalWords);

                        currentPath.RemoveAt(currentPath.Count - 1);
                    }
                }
            }
        }
    }
}
