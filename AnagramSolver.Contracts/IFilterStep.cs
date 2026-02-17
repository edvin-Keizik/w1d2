using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts
{
    public interface IFilterStep
    {
        bool Handle(FilterContext context, string signature, Func<bool> next);
    }
}
