using System.Text.Json;
using AnagramSolver.Contracts;
using AnagramSolver.Cli;

namespace AnagramSolver.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\e.keizik\Tasks\AnagramSolver\AnagramSolver.Cli\Data\zodynas.txt";

            string jsonString = File.ReadAllText("appsettings.json");
            AnagramSettings settings = JsonSerializer.Deserialize<AnagramSettings>(jsonString);

            App myApp = new App(path, settings);
            myApp.Run();
        }
    }
}
