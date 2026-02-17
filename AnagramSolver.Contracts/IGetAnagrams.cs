using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts
{
    public interface IGetAnagrams
    {
        Task<IEnumerable<Anagram>> GetAnagramsAsync(string input, int maxAnagramsToShow, int minWordLength, Func<string, bool> filter, CancellationToken ct = default);
    }
}
