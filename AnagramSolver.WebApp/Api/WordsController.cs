using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnagramSolver.Contracts;

namespace AnagramSolver.WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordsController : ControllerBase
    {
        private readonly IDictionaryLoader _loader;
        private readonly IWordProcessor _processor;
        private readonly string _path = "Data/zodynas.txt";

        public WordsController(IDictionaryLoader loader, IWordProcessor processor)
        {
            _loader = loader;
            _processor = processor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _loader.GetWordsAsync(_processor));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var words = _processor.GetDictionary();

            if(id < 0 || id > words.Count)
            {
                return NotFound($"Zodis su ID {id} nerastas.");
            }
            return Ok(words[id]);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] string word)
        {
            await _loader.AddWordAsync(_path, word, _processor);
            return Ok("Zodis pridetas");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _loader.DeleteWordAsync(_path, id, _processor);
            return success ? NoContent() : NotFound();
        }
    }

}
