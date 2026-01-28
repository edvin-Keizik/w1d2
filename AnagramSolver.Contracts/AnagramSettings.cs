using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts
{
    public class AnagramSettings
    {
        public int MinWordLength {  get; set; }
        public int MaxAnagramsToShow { get; set; }
    }
}
