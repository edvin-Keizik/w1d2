using AnagramSolver.BusinessLogic.FrequencyAnalysis;
using AnagramSolver.Contracts.FrequencyAnalysis;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AnagramSolver.BusinessLogic.Tests.FrequencyAnalysis;

public class WordTokenizerTest
{
    private readonly IStopWordProvider _stopWordProvider;

    public WordTokenizerTest()
    {
        var logger = new LoggerFactory().CreateLogger<StopWordProvider>();
        _stopWordProvider = new StopWordProvider(logger);
        _stopWordProvider.InitializeAsync(CancellationToken.None).Wait();
    }

    [Fact]
    public void Tokenize_SimpleTextWithPunctuation_RemovesPunctuation()
    {
        var text = "Labas pasaulis!";

        var result = WordTokenizer.Tokenize(text, _stopWordProvider);

        Assert.NotEmpty(result);
        Assert.DoesNotContain("!", string.Concat(result));
    }

    [Fact]
    public void Tokenize_MixedCaseText_ConvertsToLowercase()
    {
        var text = "LABAS Pasaulis";

        var result = WordTokenizer.Tokenize(text, _stopWordProvider);

        Assert.All(result, word => Assert.Equal(word, word.ToLowerInvariant()));
    }

    [Fact]
    public void Tokenize_MultipleSpaces_NormalizesToSingleWords()
    {
        var text = "Labas   pasaulis";

        var result = WordTokenizer.Tokenize(text, _stopWordProvider);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Tokenize_EmptyString_ReturnsEmptyList()
    {
        var text = "";

        var result = WordTokenizer.Tokenize(text, _stopWordProvider);

        Assert.Empty(result);
    }

    [Fact]
    public void Tokenize_OnlyStopWords_ReturnsEmptyList()
    {
        var text = "the is a";

        var result = WordTokenizer.Tokenize(text, _stopWordProvider);

        Assert.Empty(result);
    }

    [Fact]
    public void Tokenize_SpecialCharacters_Removed()
    {
        var text = "Labas@€#pasaulis";

        var result = WordTokenizer.Tokenize(text, _stopWordProvider);

        Assert.NotEmpty(result);
        Assert.DoesNotContain("@", string.Concat(result));
    }

    [Fact]
    public void Tokenize_LithuanianDiacritics_PreservedCorrectly()
    {
        var text = "ąčęėįšųūž";

        var result = WordTokenizer.Tokenize(text, _stopWordProvider);

        Assert.NotEmpty(result);
        Assert.Contains(result[0], c => "ąčęėįšųūž".Contains(c));
    }

    [Fact]
    public void Tokenize_WithStopWordsFiltering_ExcludesStopWords()
    {
        var text = "Labas ir ačiū";

        var result = WordTokenizer.Tokenize(text, _stopWordProvider);

        Assert.DoesNotContain("ir", result, StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public void FindLongestWord_ValidWordList_ReturnsLongestWord()
    {
        var words = new List<string> { "labas", "pasaulis", "ir" };

        var result = WordTokenizer.FindLongestWord(words);

        Assert.Equal("pasaulis", result);
    }

    [Fact]
    public void FindLongestWord_EmptyList_ReturnsEmptyString()
    {
        var words = new List<string>();

        var result = WordTokenizer.FindLongestWord(words);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void FindLongestWord_TieInLength_ReturnsMByDefault()
    {
        var words = new List<string> { "ab", "cd", "ef" };

        var result = WordTokenizer.FindLongestWord(words);

        Assert.Equal(2, result.Length);
    }

    [Fact]
    public void FindLongestWord_SingleWord_ReturnsThatWord()
    {
        var words = new List<string> { "pasaulis" };

        var result = WordTokenizer.FindLongestWord(words);

        Assert.Equal("pasaulis", result);
    }

    [Fact]
    public void Tokenize_WhitespaceOnly_ReturnsEmptyList()
    {
        var text = "   \t  \n  ";

        var result = WordTokenizer.Tokenize(text, _stopWordProvider);

        Assert.Empty(result);
    }

    [Fact]
    public void Tokenize_NumbersIncluded_ContainsNumbers()
    {
        var text = "Versija 2024 yra nauja";

        var result = WordTokenizer.Tokenize(text, _stopWordProvider);

        Assert.Contains("2024", result);
    }
}
