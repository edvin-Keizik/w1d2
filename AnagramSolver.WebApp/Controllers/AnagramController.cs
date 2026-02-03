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
        public IActionResult Index()
        {
            return View(new AnagramViewModel());
        }

        [HttpPost]
        public IActionResult Index(AnagramViewModel model)
        {
            if (model.InputWord != null && model.InputWord.Length >= _settings.MinWordLength)
            {
                _loader.LoadWords(_settings.FilePath, _processor);

                var anagrams = _processor.GetAnagrams(model.InputWord, _settings.MaxAnagramsToShow, _settings.MinWordLength);
                model.Result = anagrams.Select(a => a.Word).ToList();
            }
            if (model.InputWord.Length > _settings.MinWordLength)
            {
                var anagrams = _processor.GetAnagrams(model.InputWord, _settings.MaxAnagramsToShow, _settings.MinWordLength);
                model.Result = anagrams.Select(a => a.Word).ToList();
            }
            return View(model);
        }
    }
}
