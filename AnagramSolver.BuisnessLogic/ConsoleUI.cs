using AnagramSolver.Contracts;

namespace AnagramSolver.Cli
{
    public class ConsoleUI : IUserInputOutput
    {
        public string? ReadLine() => Console.ReadLine();
        public void WriteLine(string message) => Console.WriteLine(message);
    }
}