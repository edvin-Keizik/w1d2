using AnagramSolver.Contracts;
using Microsoft.AspNetCore.Mvc;
using AnagramSolver.WebApp.ViewModels;
using System.Text.Json;

namespace AnagramSolver.WebApp.Controllers
{
    public class AnagramController : Controller
    {
        private readonly IGetAnagrams _anagrams;
        private readonly IWordProcessor _processor;
        private readonly AnagramSettings _settings;

        public AnagramController(IWordProcessor processor, AnagramSettings settings, IGetAnagrams anagrams)
        {
            _processor = processor;
            _settings = settings;
            _anagrams = anagrams;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? InputWord, CancellationToken ct)
        {
            var model = new AnagramViewModel();

            if (!string.IsNullOrEmpty(InputWord))
            {
                UpdateSearchHistory(InputWord);
                Response.Cookies.Append("lastSearch", InputWord, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(30),
                    HttpOnly = true
                });


                model.InputWord = InputWord;

                var anagrams = await _anagrams.GetAnagramsAsync(InputWord, _settings.MaxAnagramsToShow, _settings.MinWordLength, w => w.Length > 3, ct);
                model.Result = anagrams.Select(a => a.Word).ToList();
            }
            LoadHistoryToViewBag();

            var lastSearch = Request.Cookies["lastSearch"];
            ViewBag.LastSearch = lastSearch;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(AnagramViewModel model, CancellationToken ct)
        {
            if (model.InputWord != null && model.InputWord.Length >= _settings.MinWordLength)
            {
                UpdateSearchHistory(model.InputWord);

                Response.Cookies.Append("lastSearch", model.InputWord, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(30),
                    HttpOnly = true
                });

                ViewBag.LastSearch = model.InputWord;

                var anagrams = await _anagrams.GetAnagramsAsync(model.InputWord, _settings.MaxAnagramsToShow, _settings.MinWordLength, w => w.Length > 3, ct);
                model.Result = anagrams.Select(a => a.Word).ToList();
            }
            
            ViewBag.LastSearch = Request.Cookies["lastSearch"] ?? model.InputWord;
            LoadHistoryToViewBag();
            

            return View(model);
        }

        public IActionResult Dictionary(int page = 1)
        {
            int pageSize = 90;

            var allWords = _processor.GetDictionary();

            var peginateWords = allWords
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)allWords.Count / pageSize);

            return View(peginateWords);
        }

        [HttpGet]
        public IActionResult AddWord()
        {
            return View(new AnagramViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddWord(AnagramViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.InputWord)) return View(model);

            if (_processor.AddWord(model.InputWord))
            {
                await System.IO.File.AppendAllLinesAsync(_settings.FilePath, new[] { model.InputWord });
                TempData["SuccessMessage"] = $"Zodis '{model.InputWord}' sekmingai pridetas!";
                return RedirectToAction("Dictionary");
            }
            else
            {
                ViewBag.ErrorMessage = "Sis zodis jau yra zodyne!";
                return View(model);
            }                

        }
        private void LoadHistoryToViewBag()
        {
            var historyJson = HttpContext.Session.GetString("SearchHistory");
            ViewBag.History = string.IsNullOrEmpty(historyJson)
                ? new List<SearchHistoryItem>()
                : JsonSerializer.Deserialize<List<SearchHistoryItem>>(historyJson);
        }

        private void UpdateSearchHistory(string word)
        {
            if(string.IsNullOrEmpty(word)) return;

            var sessionData = HttpContext.Session.GetString("SearchHistory");
            var history = string.IsNullOrEmpty(sessionData)
                ? new List<SearchHistoryItem>()
                : JsonSerializer.Deserialize<List<SearchHistoryItem>>(sessionData) ?? new List<SearchHistoryItem>();

            history.Add(new SearchHistoryItem { Word = word, SearchedAt = DateTime.Now});
            System.Diagnostics.Debug.WriteLine($"History now has {history.Count} items.");

            HttpContext.Session.SetString("SearchHistory", JsonSerializer.Serialize(history));
        }
    }
}
