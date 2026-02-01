using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts
{
    public interface IUserInputOutput
    {
        string? ReadLine();
        void WriteLine(string message);
    }
}
