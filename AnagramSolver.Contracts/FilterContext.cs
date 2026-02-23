using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts
{
    public class FilterContext
    {
        public string LetterBank { get; init; } = string.Empty;
        public HashSet<char> AllowedChars { get; init; } = null!;
        public int BankLength { get; init; }
        public int MinWordLength { get; init; }
    }
}
