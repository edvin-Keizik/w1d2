using AnagramSolver.Contracts;
using Microsoft.AspNetCore.Mvc;
using AnagramSolver.WebApp.ViewModels;

namespace AnagramSolver.WebApp.Controllers
{
    public class AnagramController : Controller
    {
        private readonly IWordProcessor _processor;
        private readonly AnagramSettings _settings;

        public AnagramController(IWordProcessor processor, AnagramSettings settings)
        {
            _processor = processor;
            _settings = settings;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? InputWord, CancellationToken ct)
        {
            var model = new AnagramViewModel();

            if (!string.IsNullOrEmpty(InputWord))
            {
                model.InputWord = InputWord;

                var anagrams =await _processor.GetAnagramsAsync(InputWord, _settings.MaxAnagramsToShow, _settings.MinWordLength, ct);
                model.Result = anagrams.Select(a => a.Word).ToList();
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(AnagramViewModel model, CancellationToken ct)
        {
            if (model.InputWord != null && model.InputWord.Length >= _settings.MinWordLength)
            {
                var anagrams = await _processor.GetAnagramsAsync(model.InputWord, _settings.MaxAnagramsToShow, _settings.MinWordLength, ct);
                model.Result = anagrams.Select(a => a.Word).ToList();
            }
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
    }
}
