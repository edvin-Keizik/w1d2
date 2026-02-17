using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts
{
    public class FilterContext
    {
        public string LetterBank { get; init; }
        public HashSet<char> AllowedChars { get; init; }
        public int BankLength { get; init; }
        public int MinWordLength { get; init; }
    }
}
