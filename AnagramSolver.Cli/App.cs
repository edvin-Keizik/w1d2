using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;

namespace AnagramSolver.Cli
{
    public class App
    {
        private readonly string _filePath;
        private readonly IWordProcessor _processor;
        private readonly IDictionaryLoader _loader;
        private readonly AnagramSettings _settings;
        private readonly IUserInputOutput _ui;


        public App(string filePath,
            AnagramSettings settings,
            IWordProcessor processor,
            IDictionaryLoader loader,
            IUserInputOutput ui)
        {
            _filePath = filePath;
            _settings = settings;
            _processor = processor;
            _loader = loader;
            _ui = ui;
        }

        public void Run()
        {
            _ui.WriteLine("Atsisiunciamas zodynas");
            _loader.LoadWords(_filePath, _processor);
            _ui.WriteLine("Zodynas atsiustas. Ivesti 0 kad baigti.");

            while (true)
            {
                string input = "";
                bool lengthCheck = false;
                int anagramCount = 0;

                while (!lengthCheck)
                {
                    _ui.WriteLine("\nIveskite zodi kurio anagramas norite suzinot(bent 3 raides): ");
                    input = _ui.ReadLine()?.Trim() ?? "";
                    if (input == "0") return;

                    if (input.Length >= _settings.MinWordLength)
                    {
                        lengthCheck = true;
                    }
                    else
                    {
                        _ui.WriteLine($"Klaida: Zodis per trumpas!");
                    }
                }           

                List<Anagram> anagrams = _processor.GetAnagrams(input, _settings.MaxAnagramsToShow, _settings.MinWordLength);
                if (anagrams.Any())
                {
                    _ui.WriteLine($"Zodzio {input} anagramos: ");
                    foreach (var anagram in anagrams)
                    {
                            _ui.WriteLine(anagram.Word);
                            anagramCount++;
                    }
                }
                else
                {
                    _ui.WriteLine($"Anagramu zodziui {input} nera.");
                }
            }
        }
    }
}
