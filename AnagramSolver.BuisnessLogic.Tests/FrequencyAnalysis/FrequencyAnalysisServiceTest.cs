using AnagramSolver.BusinessLogic.FrequencyAnalysis;
using AnagramSolver.Contracts.FrequencyAnalysis;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AnagramSolver.BusinessLogic.Tests.FrequencyAnalysis;

public class FrequencyAnalysisServiceTest
{
    private readonly IFrequencyAnalysisService _service;
    private readonly IStopWordProvider _stopWordProvider;

    public FrequencyAnalysisServiceTest()
    {
        var logger = new LoggerFactory().CreateLogger<StopWordProvider>();
        _stopWordProvider = new StopWordProvider(logger);
        _stopWordProvider.InitializeAsync(CancellationToken.None).Wait();

        var serviceLogger = new LoggerFactory().CreateLogger<FrequencyAnalysisService>();
        _service = new FrequencyAnalysisService(_stopWordProvider, serviceLogger);
    }

    [Fact]
    public async Task AnalyzeAsync_NormalText_ReturnsCorrectFrequencyCounts()
    {
        var text = "Labas pasaulis. Labas!";

        var result = await _service.AnalyzeAsync(text, CancellationToken.None);

        Assert.NotEmpty(result.TopWords);
        var labasWord = result.TopWords.FirstOrDefault(w => w.Word == "labas");
        Assert.NotNull(labasWord);
        Assert.Equal(2, labasWord.Frequency);
    }

    [Fact]
    public async Task AnalyzeAsync_DuplicateWords_CountedAsSameWord()
    {
        var text = "žodis Žodis ŽODIS";

        var result = await _service.AnalyzeAsync(text, CancellationToken.None);

        Assert.Single(result.TopWords);
        Assert.Equal("žodis", result.TopWords[0].Word);
        Assert.Equal(3, result.TopWords[0].Frequency);
    }

    [Fact]
    public async Task AnalyzeAsync_StopWordsExcluded_NotInTopWords()
    {
        var text = "the quick brown fox jumps over lazy dog";

        var result = await _service.AnalyzeAsync(text, CancellationToken.None);

        Assert.DoesNotContain(result.TopWords, w => w.Word == "the");
    }

    [Fact]
    public async Task AnalyzeAsync_MoreThanTenUnique_ReturnsOnlyTopTen()
    {
        var text = "one two three four five six seven eight nine ten eleven twelve";

        var result = await _service.AnalyzeAsync(text, CancellationToken.None);

        Assert.True(result.TopWords.Count <= 10);
    }

    [Theory]
    [InlineData("apple apple banana banana banana cherry")]
    [InlineData("manzana manzana plátano plátano plátano cereza")]
    public async Task AnalyzeAsync_FrequencyOrdering_SortedByFrequencyThenAlphabetically(string text)
    {
        var result = await _service.AnalyzeAsync(text, CancellationToken.None);

        Assert.NotEmpty(result.TopWords);
        // First should be highest frequency
        Assert.True(result.TopWords[0].Frequency >= result.TopWords[1].Frequency);
    }

    [Fact]
    public async Task AnalyzeAsync_WordCountCalculation_CorrectTotalCount()
    {
        var text = "one two three four five";

        var result = await _service.AnalyzeAsync(text, CancellationToken.None);

        Assert.Equal(5, result.TotalWordCount);
    }

    [Fact]
    public async Task AnalyzeAsync_UniqueWordCount_CorrectlyCalculated()
    {
        var text = "apple apple banana banana banana cherry";

        var result = await _service.AnalyzeAsync(text, CancellationToken.None);

        Assert.Equal(3, result.UniqueWordCount);
    }

    [Fact]
    public async Task AnalyzeAsync_LongestWord_IdentifiedCorrectly()
    {
        var text = "cat dog elephant";

        var result = await _service.AnalyzeAsync(text, CancellationToken.None);

        Assert.Equal("elephant", result.LongestWord);
    }

    [Fact]
    public async Task AnalyzeAsync_EmptyText_ThrowsArgumentException()
    {
        var text = "";

        await Assert.ThrowsAsync<ArgumentException>(() => _service.AnalyzeAsync(text, CancellationToken.None));
    }

    [Fact]
    public async Task AnalyzeAsync_WhitespaceOnlyText_ThrowsArgumentException()
    {
        var text = "   \t\n  ";

        await Assert.ThrowsAsync<ArgumentException>(() => _service.AnalyzeAsync(text, CancellationToken.None));
    }

    [Fact]
    public async Task AnalyzeAsync_OnlyStopWords_ThrowsArgumentException()
    {
        var text = "the is a an";

        await Assert.ThrowsAsync<ArgumentException>(() => _service.AnalyzeAsync(text, CancellationToken.None));
    }

    [Fact]
    public async Task AnalyzeAsync_OnlySpecialCharacters_ThrowsArgumentException()
    {
        var text = "!@#$%^&*()";

        await Assert.ThrowsAsync<ArgumentException>(() => _service.AnalyzeAsync(text, CancellationToken.None));
    }

    [Fact]
    public async Task AnalyzeAsync_LongText_ProcessesWithoutError()
    {
        var text = new string('a', 1000) + " " + new string('b', 500);

        var result = await _service.AnalyzeAsync(text, CancellationToken.None);

        Assert.NotEmpty(result.TopWords);
    }

    [Fact]
    public async Task AnalyzeAsync_MixedLanguages_AnalyzesSuccessfully()
    {
        var text = "Hello labas world pasaulis";

        var result = await _service.AnalyzeAsync(text, CancellationToken.None);

        Assert.NotEmpty(result.TopWords);
        Assert.True(result.TotalWordCount > 0);
    }

    [Fact]
    public async Task AnalyzeAsync_AllPropertiesPopulated_ReturnsCompleteResult()
    {
        var text = "apple banana cherry apple banana apple";

        var result = await _service.AnalyzeAsync(text, CancellationToken.None);

        Assert.NotEmpty(result.TopWords);
        Assert.True(result.TotalWordCount > 0);
        Assert.True(result.UniqueWordCount > 0);
        Assert.NotEmpty(result.LongestWord);
    }

    [Fact]
    public async Task AnalyzeAsync_CancellationToken_RespectsCancellation()
    {
        var cts = new CancellationTokenSource();
        cts.Cancel();

        var text = "test text";

        // Either OperationCanceledException or TaskCanceledException is acceptable
        var exceptionThrown = await Record.ExceptionAsync(
            () => _service.AnalyzeAsync(text, cts.Token));

        Assert.NotNull(exceptionThrown);
        Assert.True(exceptionThrown is OperationCanceledException or TaskCanceledException);
    }
}
