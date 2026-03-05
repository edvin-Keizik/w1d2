using AnagramSolver.Contracts.FrequencyAnalysis;
using AnagramSolver.WebApp.Models;
using AnagramSolver.WebApp.Models.Analysis;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.WebApp.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
public class AnalysisController : ControllerBase
{
    private readonly IFrequencyAnalysisService _frequencyAnalysisService;
    private readonly ILogger<AnalysisController> _logger;

    public AnalysisController(
        IFrequencyAnalysisService frequencyAnalysisService,
        ILogger<AnalysisController> logger)
    {
        _frequencyAnalysisService = frequencyAnalysisService;
        _logger = logger;
    }

    [HttpPost("frequency")]
    [ProducesResponseType(typeof(FrequencyAnalysisResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AnalyzeFrequency(
        [FromBody] FrequencyAnalysisRequestModel request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new ErrorResponse
                {
                    Message = "Validation failed",
                    Errors = errors
                });
            }

            if (request.Text.Length > 100_000)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Text exceeds maximum allowed length of 100,000 characters"
                });
            }

            var result = await _frequencyAnalysisService.AnalyzeAsync(
                request.Text,
                cancellationToken);

            var response = new FrequencyAnalysisResponseModel
            {
                TopWords = result.TopWords
                    .Select(w => new WordFrequencyModel
                    {
                        Word = w.Word,
                        Frequency = w.Frequency
                    })
                    .ToList(),
                TotalWordCount = result.TotalWordCount,
                UniqueWordCount = result.UniqueWordCount,
                LongestWord = result.LongestWord,
                AnalyzedAt = DateTime.UtcNow
            };

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error in frequency analysis");
            return BadRequest(new ErrorResponse
            {
                Message = ex.Message,
                Errors = null
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in frequency analysis");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ErrorResponse
                {
                    Message = "An unexpected error occurred",
                    Errors = null
                });
        }
    }
}
