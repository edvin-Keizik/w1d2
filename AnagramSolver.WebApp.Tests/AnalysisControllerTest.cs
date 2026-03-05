using AnagramSolver.Contracts.FrequencyAnalysis;
using AnagramSolver.WebApp.Controllers;
using AnagramSolver.WebApp.Models.Analysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AnagramSolver.WebApp.Tests.Controllers;

public class AnalysisControllerTest
{
    private readonly Mock<IFrequencyAnalysisService> _mockService;
    private readonly Mock<ILogger<AnalysisController>> _mockLogger;
    private readonly AnalysisController _controller;

    public AnalysisControllerTest()
    {
        _mockService = new Mock<IFrequencyAnalysisService>();
        _mockLogger = new Mock<ILogger<AnalysisController>>();
        _controller = new AnalysisController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task AnalyzeFrequency_ValidRequest_ReturnsOkWithResponse()
    {
        var request = new FrequencyAnalysisRequestModel { Text = "test text" };
        var analysisResult = new FrequencyAnalysisResult
        {
            TopWords = new List<WordFrequency>
            {
                new WordFrequency { Word = "test", Frequency = 1 }
            },
            TotalWordCount = 2,
            UniqueWordCount = 2,
            LongestWord = "test"
        };

        _mockService.Setup(s => s.AnalyzeAsync(request.Text, It.IsAny<CancellationToken>()))
            .ReturnsAsync(analysisResult);

        var result = await _controller.AnalyzeFrequency(request, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<FrequencyAnalysisResponseModel>(okResult.Value);
        Assert.NotNull(response.TopWords);
        Assert.Equal(2, response.TotalWordCount);
    }

    [Fact]
    public async Task AnalyzeFrequency_EmptyText_ReturnsBadRequest()
    {
        var request = new FrequencyAnalysisRequestModel { Text = "" };
        _controller.ModelState.AddModelError("Text", "Text must not be empty");

        _mockService.Setup(s => s.AnalyzeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ArgumentException("Text cannot be empty"));

        var result = await _controller.AnalyzeFrequency(request, CancellationToken.None);

        var badResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badResult.Value);
    }

    [Fact]
    public async Task AnalyzeFrequency_TextExceedsMaxLength_ReturnsBadRequest()
    {
        var request = new FrequencyAnalysisRequestModel
        {
            Text = new string('a', 100001)
        };

        _mockService.Setup(s => s.AnalyzeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ArgumentException("Text exceeds max length"));

        var result = await _controller.AnalyzeFrequency(request, CancellationToken.None);

        Assert.True(result is BadRequestObjectResult || !_controller.ModelState.IsValid);
    }

    [Fact]
    public async Task AnalyzeFrequency_OnlyStopWords_ReturnsBadRequest()
    {
        var request = new FrequencyAnalysisRequestModel { Text = "the is a" };

        _mockService.Setup(s => s.AnalyzeAsync(request.Text, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ArgumentException("Text must contain at least one valid word"));

        var result = await _controller.AnalyzeFrequency(request, CancellationToken.None);

        var badResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badResult.Value);
    }

    [Fact]
    public async Task AnalyzeFrequency_OnlySpecialCharacters_ReturnsBadRequest()
    {
        var request = new FrequencyAnalysisRequestModel { Text = "!!!##$%" };

        _mockService.Setup(s => s.AnalyzeAsync(request.Text, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ArgumentException("Text must contain at least one valid word"));

        var result = await _controller.AnalyzeFrequency(request, CancellationToken.None);

        var badResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badResult.Value);
    }

    [Fact]
    public async Task AnalyzeFrequency_ValidLithuanianText_ReturnsOkWithAnalysis()
    {
        var request = new FrequencyAnalysisRequestModel
        {
            Text = "Labas pasaulis, labas svete!"
        };
        var analysisResult = new FrequencyAnalysisResult
        {
            TopWords = new List<WordFrequency>
            {
                new WordFrequency { Word = "labas", Frequency = 2 },
                new WordFrequency { Word = "pasaulis", Frequency = 1 }
            },
            TotalWordCount = 3,
            UniqueWordCount = 2,
            LongestWord = "pasaulis"
        };

        _mockService.Setup(s => s.AnalyzeAsync(request.Text, It.IsAny<CancellationToken>()))
            .ReturnsAsync(analysisResult);

        var result = await _controller.AnalyzeFrequency(request, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<FrequencyAnalysisResponseModel>(okResult.Value);
        Assert.Equal(2, response.TopWords.Count);
    }

    [Fact]
    public async Task AnalyzeFrequency_ValidMixedText_ReturnsOkWithAnalysis()
    {
        var request = new FrequencyAnalysisRequestModel
        {
            Text = "Hello labas world pasaulis"
        };
        var analysisResult = new FrequencyAnalysisResult
        {
            TopWords = new List<WordFrequency>
            {
                new WordFrequency { Word = "hello", Frequency = 1 },
                new WordFrequency { Word = "world", Frequency = 1 },
                new WordFrequency { Word = "labas", Frequency = 1 },
                new WordFrequency { Word = "pasaulis", Frequency = 1 }
            },
            TotalWordCount = 4,
            UniqueWordCount = 4,
            LongestWord = "pasaulis"
        };

        _mockService.Setup(s => s.AnalyzeAsync(request.Text, It.IsAny<CancellationToken>()))
            .ReturnsAsync(analysisResult);

        var result = await _controller.AnalyzeFrequency(request, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<FrequencyAnalysisResponseModel>(okResult.Value);
        Assert.NotEmpty(response.TopWords);
    }

    [Fact]
    public async Task AnalyzeFrequency_ServiceThrowsUnexpectedException_Returns500()
    {
        var request = new FrequencyAnalysisRequestModel { Text = "test" };

        _mockService.Setup(s => s.AnalyzeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Unexpected error"));

        var result = await _controller.AnalyzeFrequency(request, CancellationToken.None);

        var statusResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, statusResult.StatusCode);
    }

    [Fact]
    public async Task AnalyzeFrequency_ResponseIncludesAnalyzedAtTimestamp()
    {
        var request = new FrequencyAnalysisRequestModel { Text = "test word" };
        var analysisResult = new FrequencyAnalysisResult
        {
            TopWords = new List<WordFrequency> { new WordFrequency { Word = "test", Frequency = 1 } },
            TotalWordCount = 2,
            UniqueWordCount = 2,
            LongestWord = "test"
        };

        _mockService.Setup(s => s.AnalyzeAsync(request.Text, It.IsAny<CancellationToken>()))
            .ReturnsAsync(analysisResult);

        var result = await _controller.AnalyzeFrequency(request, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<FrequencyAnalysisResponseModel>(okResult.Value);
        Assert.NotEqual(default, response.AnalyzedAt);
    }

    [Fact]
    public async Task AnalyzeFrequency_ConcurrentRequests_BothReturnCorrectResults()
    {
        var request1 = new FrequencyAnalysisRequestModel { Text = "apple banana" };
        var request2 = new FrequencyAnalysisRequestModel { Text = "cherry date" };

        var result1 = new FrequencyAnalysisResult
        {
            TopWords = new List<WordFrequency>
            {
                new WordFrequency { Word = "apple", Frequency = 1 },
                new WordFrequency { Word = "banana", Frequency = 1 }
            },
            TotalWordCount = 2,
            UniqueWordCount = 2,
            LongestWord = "banana"
        };

        var result2 = new FrequencyAnalysisResult
        {
            TopWords = new List<WordFrequency>
            {
                new WordFrequency { Word = "cherry", Frequency = 1 },
                new WordFrequency { Word = "date", Frequency = 1 }
            },
            TotalWordCount = 2,
            UniqueWordCount = 2,
            LongestWord = "cherry"
        };

        _mockService.Setup(s => s.AnalyzeAsync("apple banana", It.IsAny<CancellationToken>()))
            .ReturnsAsync(result1);
        _mockService.Setup(s => s.AnalyzeAsync("cherry date", It.IsAny<CancellationToken>()))
            .ReturnsAsync(result2);

        var task1 = _controller.AnalyzeFrequency(request1, CancellationToken.None);
        var task2 = _controller.AnalyzeFrequency(request2, CancellationToken.None);

        await Task.WhenAll(task1, task2);

        var result1Obj = Assert.IsType<OkObjectResult>(task1.Result);
        var result2Obj = Assert.IsType<OkObjectResult>(task2.Result);

        Assert.NotNull(result1Obj.Value);
        Assert.NotNull(result2Obj.Value);
    }
}
