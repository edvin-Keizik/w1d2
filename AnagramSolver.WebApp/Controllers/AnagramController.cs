using AnagramSolver.Contracts;
using Microsoft.AspNetCore.Mvc;
using AnagramSolver.WebApp.ViewModels;

namespace AnagramSolver.WebApp.Controllers
{
    public class AnagramController : Controller
    {
        private readonly IWordProcessor _processor;
        private readonly AnagramSettings _settings;
        private readonly IDictionaryLoader _loader;

        public AnagramController(IWordProcessor processor, AnagramSettings settings, IDictionaryLoader loader)
        {
            _processor = processor;
            _settings = settings;
            _loader = loader;
        }

        [HttpGet]
        public IActionResult Index(string? InputWord)
        {
            var model = new AnagramViewModel();

            if (!string.IsNullOrEmpty(InputWord))
            {
                model.InputWord = InputWord;

                var anagrams = _processor.GetAnagrams(InputWord, _settings.MaxAnagramsToShow, _settings.MinWordLength);
                model.Result = anagrams.Select(a => a.Word).ToList();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(AnagramViewModel model)
        {
            if (model.InputWord != null && model.InputWord.Length >= _settings.MinWordLength)
            {
                var anagrams = _processor.GetAnagrams(model.InputWord, _settings.MaxAnagramsToShow, _settings.MinWordLength);
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
        public IActionResult AddWord(AnagramViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.InputWord)) return View(model);

            if (_processor.AddWord(model.InputWord))
            {
                System.IO.File.AppendAllLines(_settings.FilePath, new[] { model.InputWord });
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
