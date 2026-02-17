using AnagramSolver.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.ChainOfResponsibility.Steps
{
    public class AllowedCharactersStep : IFilterStep
    {
        public bool Handle(FilterContext context, string signature, Func<bool> next)
        {
            foreach (char letter in signature)
            {
                if (!context.AllowedChars.Contains(letter)) return false;
            }

            return next();
        }
    }
}
