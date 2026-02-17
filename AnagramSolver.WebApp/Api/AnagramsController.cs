using AnagramSolver.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AnagramSolver.WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnagramsController : ControllerBase
    {
        private readonly IGetAnagrams _anagrams;
        private readonly AnagramSettings _settings;

        public AnagramsController(IGetAnagrams anagrams, AnagramSettings settings)
        {
            _anagrams = anagrams;
            _settings = settings;
        }

        [HttpGet("{word}")]
        public async Task<ActionResult<IEnumerable<string>>> GetAnagrams(string word, CancellationToken ct)
        {
            var watch = Stopwatch.StartNew();

            var anagrams = await _anagrams.GetAnagramsAsync(word, _settings.MaxAnagramsToShow, _settings.MinWordLength, w => w.Length > 3, ct);

            watch.Stop();

            Response.Headers.Append("X-Anagram-Count", anagrams.Count().ToString());
            Response.Headers.Append("X-Search-Duration-Ms", watch.ElapsedMilliseconds.ToString());

            return Ok(anagrams.Select(a => a.Word));
        }
    }
}
