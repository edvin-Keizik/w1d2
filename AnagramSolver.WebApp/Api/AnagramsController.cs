using AnagramSolver.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AnagramSolver.WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnagramsController : ControllerBase
    {
        private readonly IWordProcessor _processor;
        private readonly AnagramSettings _settings;

        public AnagramsController(IWordProcessor processor, AnagramSettings settings)
        {
            _processor = processor;
            _settings = settings;
        }

        [HttpGet("{word}")]
        public async Task<ActionResult<IEnumerable<string>>> GetAnagrams(string word, CancellationToken ct)
        {
            var watch = Stopwatch.StartNew();

            var anagrams = await _processor.GetAnagramsAsync(word, _settings.MaxAnagramsToShow, w => w.Length > 3, ct);

            watch.Stop();

            Response.Headers.Append("X-Anagram-Count", anagrams.Count().ToString());
            Response.Headers.Append("X-Search-Duration-Ms", watch.ElapsedMilliseconds.ToString());

            return Ok(anagrams.Select(a => a.Word));
        }
    }
}
