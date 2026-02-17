using AnagramSolver.Contracts;

namespace AnagramSolver.BusinessLogic.ChainOfResponsibility.Steps
{
    public class WordLengthStep : IFilterStep
    {
        public bool Handle(FilterContext context, string signature, Func<bool> next)
        {
            if (signature.Length > context.BankLength) return false;

            if(signature.Length < context.MinWordLength) return false;

            return next();
        }
    }
}
