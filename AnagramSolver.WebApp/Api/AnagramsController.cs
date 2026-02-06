using AnagramSolver.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnagramsController : ControllerBase
    {
        private readonly IWordProcessor _processor;

        public AnagramsController(IWordProcessor processor)
        {
            _processor = processor;
        }

        [HttpGet("{word}")]
        public async Task<ActionResult<IEnumerable<string>>> GetAnagrams(string word, CancellationToken ct)
        {
            var anagrams = await _processor.GetAnagramsAsync(word, 2, 3, ct);
            return Ok(anagrams.Select(a => a.Word));
        }
    }
}
