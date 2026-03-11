using Microsoft.Extensions.AI;
using System.ComponentModel;
using System.Text.Json;

namespace AnagramSolver.MAF.Tools;

public sealed class AnagramToolFunctions
{
    private readonly IAnagramTools _tools;

    public AnagramToolFunctions(IAnagramTools tools)
    {
        _tools = tools ?? throw new ArgumentNullException(nameof(tools));
    }

    public IList<AIFunction> CreateAllFunctions()
    {
        return
        [
            CreateSearchAnagramsFunction(),
            CreateGetWordCountFunction(),
            CreateFilterByLengthFunction()
        ];
    }

    private AIFunction CreateSearchAnagramsFunction()
    {
        var tools = _tools;

        return AIFunctionFactory.Create(
            [Description("Searches for anagrams of the given input letters. " +
                        "An anagram is a word formed by rearranging the letters of another word. " +
                        "Can find single words or combinations of words that use the same letters.")]
            async (
                [Description("The letters or word to find anagrams for (e.g., 'katas', 'vilnius', 'roma')")] 
                string input,

                [Description("Maximum number of words in each anagram combination (default: 2, max: 5)")] 
                int maxAnagrams = 2,

                [Description("Minimum length of individual words to include (default: 2)")] 
                int minWordLength = 2,

                CancellationToken cancellationToken = default) =>
            {
                maxAnagrams = Math.Clamp(maxAnagrams, 1, 5);
                minWordLength = Math.Max(minWordLength, 1);

                var result = await tools.SearchAnagramsAsync(
                    input, 
                    maxAnagrams, 
                    minWordLength, 
                    cancellationToken);

                return JsonSerializer.Serialize(result, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
            },
            "SearchAnagrams");
    }

    private AIFunction CreateGetWordCountFunction()
    {
        var tools = _tools;

        return AIFunctionFactory.Create(
            [Description("Gets the total number of words in the anagram dictionary database. " +
                        "Use this when the user asks about the dictionary size or how many words are available.")]
            async (CancellationToken cancellationToken = default) =>
            {
                var result = await tools.GetWordCountAsync(cancellationToken);

                return JsonSerializer.Serialize(result, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
            },
            "GetWordCount");
    }

    private AIFunction CreateFilterByLengthFunction()
    {
        var tools = _tools;

        return AIFunctionFactory.Create(
            [Description("Filters words from the dictionary by their exact length. " +
                        "Use this when the user asks for words of a specific length or needs words for word games.")]
            async (
                [Description("The exact length of words to filter (e.g., 5 for five-letter words)")] 
                int length,

                [Description("Maximum number of words to return (default: 100, max: 500)")] 
                int maxResults = 100,

                CancellationToken cancellationToken = default) =>
            {
                // Clamp maxResults to reasonable bounds
                maxResults = Math.Clamp(maxResults, 1, 500);

                var result = await tools.FilterByLengthAsync(
                    length, 
                    maxResults, 
                    cancellationToken);

                return JsonSerializer.Serialize(result, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
            },
            "FilterByLength");
    }
}
