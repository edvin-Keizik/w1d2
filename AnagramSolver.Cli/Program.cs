using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;
using System.Text.Json;

namespace AnagramSolver.Cli
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            using var cts = new CancellationTokenSource();
            var ct = cts.Token;

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            string jsonString = File.ReadAllText("appsettings.json");
            AnagramSettings settings = JsonSerializer.Deserialize<AnagramSettings>(jsonString)!;

            IUserInputOutput ui = new ConsoleUI();

            App myApp = new App(settings, ui);
            await myApp.Run(ct);
        }
    }
}
