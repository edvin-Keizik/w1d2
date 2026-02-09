using AnagramSolver.Contracts;
using System.Net.Http.Json;

namespace AnagramSolver.Cli
{
    public class App
    {
        private readonly IUserInputOutput _ui;
        private readonly AnagramSettings _settings;
        private readonly HttpClient _httpClient;

        public App(AnagramSettings settings, IUserInputOutput ui, HttpClient httpClient)
        {
            _settings = settings;
            _ui = ui;
            _httpClient = httpClient;
        }

        public async Task Run(CancellationToken ct)
        {
            _ui.WriteLine("Prisijungta prie API. Ivesti 0 kad baigti.");

            while (!ct.IsCancellationRequested)
            {
                _ui.WriteLine("\nIveskite zodi (bent 3 raides): ");
                string input = _ui.ReadLine()?.Trim() ?? "";

                if (input == "0") break;
                if (input.Length < _settings.MinWordLength)
                {
                    _ui.WriteLine("Klaida: Zodis per trumpas!");
                    continue;
                }

                try
                {
                    var response = await _httpClient.GetFromJsonAsync<List<string>>($"api/anagrams/{input}", ct);

                    if (response != null && response.Any())
                    {
                        _ui.WriteLine($"Anagramos: {string.Join(", ", response)}");
                    }
                    else
                    {
                        _ui.WriteLine("Anagramu nerasta.");
                    }
                }
                catch (Exception ex)
                {
                    _ui.WriteLine($"Klaida: {ex.Message}");
                }
            }
        }
    }
}
