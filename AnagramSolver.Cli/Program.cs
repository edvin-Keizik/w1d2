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

            string path = @"C:\Users\e.keizik\Tasks\AnagramSolver\AnagramSolver.Cli\Data\zodynas.txt";

            string jsonString = File.ReadAllText("appsettings.json");
            AnagramSettings settings = JsonSerializer.Deserialize<AnagramSettings>(jsonString)!;

            IAnagramSearchEngine engine = new AnagramSearchEngine();
            IWordProcessor processor = new WordProcessor(engine);
            IFileSystemWrapper fileSystem = new FileSystemWrapper();
            IDictionaryLoader loader = new DictionaryLoader(fileSystem);
            IUserInputOutput ui = new ConsoleUI();

            App myApp = new App(path, settings, processor, loader, ui);
            await myApp.Run(ct);
        }
    }
}
