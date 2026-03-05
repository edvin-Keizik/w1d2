using System.Text;
using AnagramSolver.Contracts.FrequencyAnalysis;

namespace AnagramSolver.BusinessLogic.FrequencyAnalysis;

public static class WordTokenizer
{
    private static readonly HashSet<char> LithuanianDiacritics =
        new(new[] { 'ą', 'č', 'ę', 'ė', 'į', 'š', 'ų', 'ū', 'ž',
                    'Ą', 'Č', 'Ę', 'Ė', 'Į', 'Š', 'Ų', 'Ū', 'Ž' });

    /// <summary>Tokenizes text into words, filtering stop words and normalizing to lowercase.</summary>
    /// <param name="text">The input text to tokenize</param>
    /// <param name="stopWordProvider">The stop word provider to filter common words</param>
    /// <returns>List of valid words with stop words removed and normalized to lowercase</returns>
    public static List<string> Tokenize(string text, IStopWordProvider stopWordProvider)
    {
        if (string.IsNullOrWhiteSpace(text))
            return [];

        var words = new List<string>();
        var buffer = new StringBuilder();
        var textSpan = text.AsSpan();

        foreach (char c in textSpan)
        {
            if (IsValidWordCharacter(c))
            {
                buffer.Append(char.ToLowerInvariant(c));
            }
            else if (buffer.Length > 0)
            {
                string word = buffer.ToString();
                if (!stopWordProvider.IsStopWord(word))
                {
                    words.Add(word);
                }
                buffer.Clear();
            }
        }

        if (buffer.Length > 0)
        {
            string word = buffer.ToString();
            if (!stopWordProvider.IsStopWord(word))
            {
                words.Add(word);
            }
        }

        return words;
    }

    /// <summary>Checks if a character is valid for a word (alphanumeric or Lithuanian diacritic).</summary>
    private static bool IsValidWordCharacter(char c) =>
        char.IsLetterOrDigit(c) || LithuanianDiacritics.Contains(c);

    /// <summary>Finds the longest word by character count. In case of ties, returns the first occurrence.</summary>
    /// <param name="words">The list of words to search</param>
    /// <returns>The longest word, or empty string if list is empty</returns>
    public static string FindLongestWord(List<string> words)
    {
        if (words.Count == 0)
            return string.Empty;

        return words.MaxBy(w => w.Length) ?? string.Empty;
    }
}
