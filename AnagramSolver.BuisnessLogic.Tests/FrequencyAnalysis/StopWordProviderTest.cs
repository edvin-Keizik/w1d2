using AnagramSolver.BusinessLogic.FrequencyAnalysis;
using AnagramSolver.Contracts.FrequencyAnalysis;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AnagramSolver.BusinessLogic.Tests.FrequencyAnalysis;

public class StopWordProviderTest
{
    private readonly ILogger<StopWordProvider> _mockLogger;
    private readonly StopWordProvider _provider;

    public StopWordProviderTest()
    {
        _mockLogger = new LoggerFactory().CreateLogger<StopWordProvider>();
        _provider = new StopWordProvider(_mockLogger);
        _provider.InitializeAsync(CancellationToken.None).GetAwaiter().GetResult();
    }

    [Fact]
    public void IsStopWord_Lithuanian_ReturnsTrueForStopWords()
    {
        Assert.True(_provider.IsStopWord("ir"));
        Assert.True(_provider.IsStopWord("o"));
        Assert.True(_provider.IsStopWord("bet"));
    }

    [Fact]
    public void IsStopWord_English_ReturnsTrueForStopWords()
    {
        Assert.True(_provider.IsStopWord("the"));
        Assert.True(_provider.IsStopWord("is"));
        Assert.True(_provider.IsStopWord("a"));
    }

    [Fact]
    public void IsStopWord_NonStopWords_ReturnsFalse()
    {
        Assert.False(_provider.IsStopWord("žodis"));
        Assert.False(_provider.IsStopWord("word"));
        Assert.False(_provider.IsStopWord("labas"));
    }

    [Theory]
    [InlineData("THE")]
    [InlineData("The")]
    [InlineData("ThE")]
    public void IsStopWord_CaseInsensitive_ReturnsTrueForStopWord(string word)
    {
        var result = _provider.IsStopWord(word);
        Assert.True(result);
    }

    [Fact]
    public void IsStopWord_EmptyString_ReturnsFalse()
    {
        var result = _provider.IsStopWord("");
        Assert.False(result);
    }

    [Theory]
    [InlineData("ir")]
    [InlineData("the")]
    public void IsStopWord_WithReadOnlySpan_WorksCorrectly(string word)
    {
        ReadOnlySpan<char> wordSpan = word.AsSpan();
        var result = _provider.IsStopWord(wordSpan);
        Assert.True(result);
    }

    [Fact]
    public async Task InitializeAsync_CanBeCalledMultipleTimes()
    {
        var provider = new StopWordProvider(_mockLogger);
        await provider.InitializeAsync(CancellationToken.None);
        await provider.InitializeAsync(CancellationToken.None);
        
        Assert.True(provider.IsStopWord("the"));
    }
}
