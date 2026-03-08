using AnagramSolver.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.WebApp.Api;

[Route("api/[controller]")]
[ApiController]
public class SettingsController : ControllerBase
{
    private readonly AnagramSettings _settings;

    public SettingsController(AnagramSettings settings)
    {
        _settings = settings;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            minWordLength = _settings.MinWordLength,
            maxAnagramsToShow = _settings.MaxAnagramsToShow
        });
    }
}
