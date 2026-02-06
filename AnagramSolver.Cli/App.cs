using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;
using System.Net.Http.Json;

namespace AnagramSolver.Cli
{
    public class App
    {
        private readonly IUserInputOutput _ui;
        private readonly AnagramSettings _settings;

        public App(AnagramSettings settings, IUserInputOutput ui)
        {
            _settings = settings;
            _ui = ui;
        }

        public async Task Run(CancellationToken ct)
        {
            _ui.WriteLine("Prisijungta prie API. Ivesti 0 kad baigti.");

            using var client = new HttpClient();

            while (true)
            {
                string input = "";
                bool lengthCheck = false;

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

                try
                {
                    string url = $"http://localhost:8080/api/anagrams/{input}";

                    var anagrams = await client.GetFromJsonAsync<List<string>>(url, ct);

                    if (anagrams != null && anagrams.Any())
                    {
                        _ui.WriteLine($"Zodzio {input} anagramos: ");
                        foreach (var word in anagrams)
                        {
                            _ui.WriteLine(word);
                        }
                    }
                    else
                    {
                        _ui.WriteLine($"Anagramu zodziui {input} nera.");
                    }
                }
                catch (Exception ex)
                {
                    _ui.WriteLine($"Klaida: Nepavyko pasiekti API ({ex.Message})");
                }
            }
        }
    }
}
