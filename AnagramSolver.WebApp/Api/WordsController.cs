using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnagramSolver.Contracts;

namespace AnagramSolver.WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordsController : ControllerBase
    {
        private readonly IWordProcessor _processor;

        public WordsController(IWordProcessor processor)
        {
            _processor = processor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var words = await _processor.GetDictionary();
            return Ok(words);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var words = await _processor.GetDictionary();

            if(id < 0 || id > words.Count)
            {
                return NotFound($"Zodis su ID {id} nerastas.");
            }
            return Ok(words[id]);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return BadRequest("Word cannot be empty.");

            bool success = await _processor.AddWordAsync(word);
            return success ? Ok("Zodis pridetas") : BadRequest("Zodis jau egzistuoja.");
        }
    }

}
