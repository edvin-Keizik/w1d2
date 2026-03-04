using AnagramSolver.Contracts;
using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AnagramSolver.WebApp.Plugins
{
    public class AnagramPlugin
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AnagramPlugin> _logger;

        public AnagramPlugin(IServiceProvider serviceProvider, ILogger<AnagramPlugin> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [KernelFunction("FindAnagrams")]
        [Description("Finds all anagrams and word combinations for the given input letters. Can handle multiple words separated by spaces.")]
        public async Task<AnagramResult> FindAnagrams(
            [Description("The letters to find anagrams for (e.g., 'katas', 'vilnius')")] string input,
            [Description("Maximum number of words in each combination (default: 2)")] int maxAnagrams = 2,
            [Description("Minimum length of individual words (default: 2)")] int minWordLength = 2,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                _logger.LogWarning("FindAnagrams called with empty input");
                return new AnagramResult
                {
                    Success = false,
                    Message = "Input cannot be empty",
                    Anagrams = new List<string>()
                };
            }

            try
            {
                _logger.LogInformation("Finding anagrams for input: {Input}, maxAnagrams: {MaxAnagrams}, minWordLength: {MinWordLength}",
                    input, maxAnagrams, minWordLength);

                using var scope = _serviceProvider.CreateScope();
                var anagramService = scope.ServiceProvider.GetRequiredService<IGetAnagrams>();

                Func<string, bool> defaultFilter = _ => true;

                var results = await anagramService.GetAnagramsAsync(
                    input,
                    maxAnagrams,
                    minWordLength,
                    defaultFilter,
                    cancellationToken);

                var anagrams = results.Select(a => a.Word).ToList();

                _logger.LogInformation("Found {Count} anagrams for input: {Input}", anagrams.Count, input);

                return new AnagramResult
                {
                    Success = true,
                    Message = $"Found {anagrams.Count} anagrams for '{input}'",
                    Anagrams = anagrams,
                    InputWord = input
                };
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("FindAnagrams operation was cancelled for input: {Input}", input);
                return new AnagramResult
                {
                    Success = false,
                    Message = "Operation was cancelled",
                    Anagrams = new List<string>()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding anagrams for input: {Input}", input);
                return new AnagramResult
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Anagrams = new List<string>()
                };
            }
        }

       
        [KernelFunction("GetDictionaryInfo")]
        [Description("Gets information about the word dictionary used for anagram searching")]
        public async Task<DictionaryInfo> GetDictionaryInfo(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving dictionary information");

                using var scope = _serviceProvider.CreateScope();
                var anagramService = scope.ServiceProvider.GetRequiredService<IGetAnagrams>();

                if (anagramService is IWordProcessor wordProcessor)
                {
                    var dictionary = await wordProcessor.GetDictionary();

                    _logger.LogInformation("Dictionary contains {Count} words", dictionary.Count);

                    return new DictionaryInfo
                    {
                        Success = true,
                        WordCount = dictionary.Count,
                        Message = $"Dictionary contains {dictionary.Count} words"
                    };
                }

                return new DictionaryInfo
                {
                    Success = false,
                    WordCount = 0,
                    Message = "Unable to retrieve dictionary information"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dictionary information");
                return new DictionaryInfo
                {
                    Success = false,
                    WordCount = 0,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }

    public class AnagramResult
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("inputWord")]
        public string? InputWord { get; set; }

        [JsonPropertyName("anagrams")]
        public List<string> Anagrams { get; set; } = new();

        [JsonPropertyName("count")]
        public int Count => Anagrams.Count;
    }

    public class DictionaryInfo
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("wordCount")]
        public int WordCount { get; set; }
    }
}
