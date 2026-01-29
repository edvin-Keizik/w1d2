using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts
{
    public interface IWordProcessor
    {
        List<Anagram> GetAnagrams(string input, int maxAnagramsToShow);
        void AddWord(string word);
    }
}
