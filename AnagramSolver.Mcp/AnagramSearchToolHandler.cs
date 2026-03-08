namespace AnagramSolver.Mcp
{
    /// <summary>
    /// Represents the result of an anagram search operation.
    /// </summary>
    public class AnagramSearchResult
    {
        public string Word { get; set; } = string.Empty;
        public List<string> Anagrams { get; set; } = new();
        public long SearchDurationMs { get; set; }
        public int AnagramCount { get; set; }
    }

    /// <summary>
    /// MCP Server handler that exposes anagram search as a callable tool.
    /// </summary>
    public class AnagramSearchToolHandler
    {
        private readonly McpAnagramSearchTool _anagramTool;

        public AnagramSearchToolHandler(McpAnagramSearchTool anagramTool)
        {
            _anagramTool = anagramTool ?? throw new ArgumentNullException(nameof(anagramTool));
        }

        /// <summary>
        /// Handles the anagram search tool invocation with search word and optional parameters.
        /// </summary>
        /// <param name="word">The word to search anagrams for.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Search result with anagrams and metadata.</returns>
        public async Task<AnagramSearchResult> HandleSearchAnagramsAsync(
            string word,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                throw new ArgumentException("Word parameter cannot be null or empty.", nameof(word));
            }

            var startTime = DateTime.UtcNow;

            try
            {
                var anagrams = await _anagramTool.SearchAnagramsAsync(word, cancellationToken);

                var endTime = DateTime.UtcNow;
                var duration = (long)(endTime - startTime).TotalMilliseconds;

                return new AnagramSearchResult
                {
                    Word = word,
                    Anagrams = anagrams.ToList(),
                    AnagramCount = anagrams.Count(),
                    SearchDurationMs = duration
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Failed to search anagrams for word '{word}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the tool definition for Copilot/MCP to understand how to use this tool.
        /// </summary>
        public static object GetToolDefinition()
        {
            return new
            {
                name = "search_anagrams",
                description = "Search for anagrams of a given word. Finds all possible word rearrangements containing the same letters.",
                inputSchema = new
                {
                    type = "object",
                    properties = new
                    {
                        word = new
                        {
                            type = "string",
                            description = "The word to find anagrams for (e.g., 'listen', 'heart', 'praktika')"
                        }
                    },
                    required = new[] { "word" }
                }
            };
        }
    }
}
