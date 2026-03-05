using System.Reflection;
using AnagramSolver.Contracts.FrequencyAnalysis;
using Microsoft.Extensions.Logging;

namespace AnagramSolver.BusinessLogic.FrequencyAnalysis;

public class StopWordProvider : IStopWordProvider
{
    private HashSet<string> _stopWordsLt = [];
    private HashSet<string> _stopWordsEn = [];
    private readonly ILogger<StopWordProvider> _logger;

    public StopWordProvider(ILogger<StopWordProvider> logger)
    {
        _logger = logger;
    }

    public bool IsStopWord(ReadOnlySpan<char> word)
    {
        if (word.IsEmpty)
            return false;

        var wordStr = new string(word).ToLowerInvariant();
        return _stopWordsLt.Contains(wordStr) || _stopWordsEn.Contains(wordStr);
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            
            await LoadStopWordsAsync(
                "AnagramSolver.BusinessLogic.FrequencyAnalysis.Data.stop-words-lt.txt",
                _stopWordsLt,
                assembly,
                cancellationToken);

            await LoadStopWordsAsync(
                "AnagramSolver.BusinessLogic.FrequencyAnalysis.Data.stop-words-en.txt",
                _stopWordsEn,
                assembly,
                cancellationToken);

            _logger.LogInformation("Stop words loaded: Lithuanian={LtCount}, English={EnCount}",
                _stopWordsLt.Count, _stopWordsEn.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize stop words");
            throw;
        }
    }

    private static Task LoadStopWordsAsync(
        string resourceName,
        HashSet<string> collection,
        Assembly assembly,
        CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            try
            {
                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                    throw new FileNotFoundException($"Resource not found: {resourceName}");

                using var reader = new StreamReader(stream);
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        collection.Add(line.Trim().ToLowerInvariant());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load stop words from {resourceName}", ex);
            }
        }, cancellationToken);
    }
}
