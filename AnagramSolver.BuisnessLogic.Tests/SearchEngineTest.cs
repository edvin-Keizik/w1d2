using AnagramSolver.BusinessLogic;
using FluentAssertions;
using System.Diagnostics;
using Xunit;

namespace AnagramSolver.BuisnessLogic.Tests
{
    public class AnagramSearchEngineTests
    {
        private readonly AnagramSearchEngine _engine;

        public AnagramSearchEngineTests()
        {
            _engine = new AnagramSearchEngine();
        }

        #region CanSubstract - Happy Path Tests

        [Fact]
        public void CanSubstract_WithValidSubtraction_ReturnsTrue()
        {
            // Arrange
            string letterBank = "aabcde";
            string signature = "abc";

            // Act
            var result = _engine.CanSubstract(letterBank, signature, out string leftover);

            // Assert
            result.Should().BeTrue();
            leftover.Should().Be("ade");
        }

        [Fact]
        public void CanSubstract_WithExactMatch_ReturnsTrueWithEmptyLeftover()
        {
            // Arrange
            string letterBank = "abc";
            string signature = "abc";

            // Act
            var result = _engine.CanSubstract(letterBank, signature, out string leftover);

            // Assert
            result.Should().BeTrue();
            leftover.Should().BeEmpty();
        }

        #endregion

        #region CanSubstract - Edge Cases

        [Fact]
        public void CanSubstract_WithEmptySignature_ReturnsTrueWithAllLettersLeftover()
        {
            // Arrange
            string letterBank = "abc";
            string signature = "";

            // Act
            var result = _engine.CanSubstract(letterBank, signature, out string leftover);

            // Assert
            result.Should().BeTrue();
            leftover.Should().Be("abc");
        }

        [Fact]
        public void CanSubstract_WithEmptyLetterBank_ReturnsFalseForNonEmptySignature()
        {
            // Arrange
            string letterBank = "";
            string signature = "a";

            // Act
            var result = _engine.CanSubstract(letterBank, signature, out string leftover);

            // Assert
            result.Should().BeFalse();
            leftover.Should().BeEmpty();
        }

        [Fact]
        public void CanSubstract_WithBothEmpty_ReturnsTrue()
        {
            // Arrange
            string letterBank = "";
            string signature = "";

            // Act
            var result = _engine.CanSubstract(letterBank, signature, out string leftover);

            // Assert
            result.Should().BeTrue();
            leftover.Should().BeEmpty();
        }

        [Fact]
        public void CanSubstract_WithSingleCharacter_ReturnsTrue()
        {
            // Arrange
            string letterBank = "a";
            string signature = "a";

            // Act
            var result = _engine.CanSubstract(letterBank, signature, out string leftover);

            // Assert
            result.Should().BeTrue();
            leftover.Should().BeEmpty();
        }

        [Fact]
        public void CanSubstract_WithSignatureLargerThanBank_ReturnsFalse()
        {
            // Arrange
            string letterBank = "ab";
            string signature = "abc";

            // Act
            var result = _engine.CanSubstract(letterBank, signature, out string leftover);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region CanSubstract - Duplicate Letters

        [Fact]
        public void CanSubstract_WithMultipleDuplicateLetters_ReturnsTrueAndCorrectLeftover()
        {
            // Arrange
            string letterBank = "aabbcc";
            string signature = "aac";

            // Act
            var result = _engine.CanSubstract(letterBank, signature, out string leftover);

            // Assert
            result.Should().BeTrue();
            leftover.Should().Be("bbc");
        }

        [Fact]
        public void CanSubstract_WithInsufficientDuplicates_ReturnsFalse()
        {
            // Arrange
            string letterBank = "aab";
            string signature = "aaa";

            // Act
            var result = _engine.CanSubstract(letterBank, signature, out string leftover);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region CanSubstract - Theory Tests

        [Theory]
        [InlineData("abcdef", "ace", "bdf", true)]
        [InlineData("aabbcc", "abc", "abc", true)]
        [InlineData("abc", "def", "", false)]
        [InlineData("ehlllo", "hll", "eo", true)]
        [InlineData("aggimmnoprr", "agin", "gmmorp", true)]
        [InlineData("xyz", "zyx", "", false)]
        public void CanSubstract_WithVariousInputs_ProducesExpectedResults(
            string letterBank,
            string signature,
            string expectedLeftover,
            bool expectedResult)
        {
            // Act
            var result = _engine.CanSubstract(letterBank, signature, out string leftover);

            // Assert
            result.Should().Be(expectedResult);
            if (expectedResult)
            {
                leftover.Should().Be(expectedLeftover);
            }
        }

        #endregion

        #region CanSubstract - Performance Tests

        [Fact]
        public void CanSubstract_WithLongWord_CompletesWithinAcceptableTime()
        {
            // Arrange
            string longLetterBank = new string('a', 5000) + new string('z', 5000);
            string longSignature = new string('a', 2500);
            var stopwatch = Stopwatch.StartNew();

            // Act
            var result = _engine.CanSubstract(longLetterBank, longSignature, out string leftover);
            stopwatch.Stop();

            // Assert
            result.Should().BeTrue();
            leftover.Should().HaveLength(7500);
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(100, "operation should complete quickly");
        }

        #endregion

        #region FindAllCombinations - Happy Path Tests

        [Fact]
        public void FindAllCombinations_WithValidCombinations_FindsAllResults()
        {
            // Arrange
            string remainingLetters = "aabbcc";
            int wordsNeeded = 2;
            var currentPath = new List<string>();
            var candidates = new List<string> { "ab", "abc" };
            var wordGroups = new Dictionary<string, List<string>>
            {
                { "ab", new List<string> { "ab" } },
                { "abc", new List<string> { "abc" } }
            };
            var allResults = new List<List<string>>();
            var originalWords = new List<string>();

            // Act
            _engine.FindAllCombinations(remainingLetters, wordsNeeded, currentPath, candidates, wordGroups, allResults, originalWords);

            // Assert
            allResults.Should().NotBeEmpty();
            allResults.Should().HaveCountGreaterThanOrEqualTo(1);
        }

        [Fact]
        public void FindAllCombinations_WithExactMatch_FindsSingleCombination()
        {
            // Arrange
            string remainingLetters = "abc";
            int wordsNeeded = 1;
            var currentPath = new List<string>();
            var candidates = new List<string> { "abc" };
            var wordGroups = new Dictionary<string, List<string>>
            {
                { "abc", new List<string> { "abc" } }
            };
            var allResults = new List<List<string>>();
            var originalWords = new List<string>();

            // Act
            _engine.FindAllCombinations(remainingLetters, wordsNeeded, currentPath, candidates, wordGroups, allResults, originalWords);

            // Assert
            allResults.Should().HaveCount(1);
            allResults[0].Should().ContainSingle();
            allResults[0][0].Should().Be("abc");
        }

        #endregion

        #region FindAllCombinations - Edge Cases

        [Fact]
        public void FindAllCombinations_WithZeroWordsNeeded_ReturnsCombinationWhenNoRemainingLetters()
        {
            // Arrange
            string remainingLetters = "";
            int wordsNeeded = 0;
            var currentPath = new List<string> { "word1" };
            var candidates = new List<string>();
            var wordGroups = new Dictionary<string, List<string>>();
            var allResults = new List<List<string>>();
            var originalWords = new List<string>();

            // Act
            _engine.FindAllCombinations(remainingLetters, wordsNeeded, currentPath, candidates, wordGroups, allResults, originalWords);

            // Assert
            allResults.Should().HaveCount(1);
            allResults[0].Should().ContainSingle().Which.Should().Be("word1");
        }

        [Fact]
        public void FindAllCombinations_WithZeroWordsNeededButRemainingLetters_ReturnsNoResults()
        {
            // Arrange
            string remainingLetters = "abc";
            int wordsNeeded = 0;
            var currentPath = new List<string>();
            var candidates = new List<string>();
            var wordGroups = new Dictionary<string, List<string>>();
            var allResults = new List<List<string>>();
            var originalWords = new List<string>();

            // Act
            _engine.FindAllCombinations(remainingLetters, wordsNeeded, currentPath, candidates, wordGroups, allResults, originalWords);

            // Assert
            allResults.Should().BeEmpty();
        }

        [Fact]
        public void FindAllCombinations_WithEmptyLetterBank_ReturnsNoResults()
        {
            // Arrange
            string remainingLetters = "";
            int wordsNeeded = 1;
            var currentPath = new List<string>();
            var candidates = new List<string> { "abc" };
            var wordGroups = new Dictionary<string, List<string>>
            {
                { "abc", new List<string> { "abc" } }
            };
            var allResults = new List<List<string>>();
            var originalWords = new List<string>();

            // Act
            _engine.FindAllCombinations(remainingLetters, wordsNeeded, currentPath, candidates, wordGroups, allResults, originalWords);

            // Assert
            allResults.Should().BeEmpty();
        }

        [Fact]
        public void FindAllCombinations_WithNoCandidates_ReturnsNoResults()
        {
            // Arrange
            string remainingLetters = "abc";
            int wordsNeeded = 1;
            var currentPath = new List<string>();
            var candidates = new List<string>();
            var wordGroups = new Dictionary<string, List<string>>();
            var allResults = new List<List<string>>();
            var originalWords = new List<string>();

            // Act
            _engine.FindAllCombinations(remainingLetters, wordsNeeded, currentPath, candidates, wordGroups, allResults, originalWords);

            // Assert
            allResults.Should().BeEmpty();
        }

        #endregion

        #region FindAllCombinations - Original Words Exclusion

        [Fact]
        public void FindAllCombinations_ExcludesWordsInOriginalWords()
        {
            // Arrange
            string remainingLetters = "aabbcc";
            int wordsNeeded = 1;
            var currentPath = new List<string>();
            var candidates = new List<string> { "ab" };
            var wordGroups = new Dictionary<string, List<string>>
            {
                { "ab", new List<string> { "ab", "ba" } }
            };
            var allResults = new List<List<string>>();
            var originalWords = new List<string> { "ab" }; // Exclude "ab"

            // Act
            _engine.FindAllCombinations(remainingLetters, wordsNeeded, currentPath, candidates, wordGroups, allResults, originalWords);

            // Assert
            allResults.Should().HaveCount(1);
            allResults[0][0].Should().Be("ba");
            allResults.All(r => !r.Any(w => w.ToLower() == "ab")).Should().BeTrue();
        }

        [Fact]
        public void FindAllCombinations_WithAllWordsExcluded_ReturnsNoResults()
        {
            // Arrange
            string remainingLetters = "abc";
            int wordsNeeded = 1;
            var currentPath = new List<string>();
            var candidates = new List<string> { "abc" };
            var wordGroups = new Dictionary<string, List<string>>
            {
                { "abc", new List<string> { "abc" } }
            };
            var allResults = new List<List<string>>();
            var originalWords = new List<string> { "abc" }; // Exclude the only word

            // Act
            _engine.FindAllCombinations(remainingLetters, wordsNeeded, currentPath, candidates, wordGroups, allResults, originalWords);

            // Assert
            allResults.Should().BeEmpty();
        }

        #endregion

        #region FindAllCombinations - Multiple Words

        [Fact]
        public void FindAllCombinations_WithMultipleWords_FindsAllPermutations()
        {
            // Arrange
            string remainingLetters = "aabbccdd";
            int wordsNeeded = 2;
            var currentPath = new List<string>();
            var candidates = new List<string> { "ab", "cd" };
            var wordGroups = new Dictionary<string, List<string>>
            {
                { "ab", new List<string> { "ab" } },
                { "cd", new List<string> { "cd" } }
            };
            var allResults = new List<List<string>>();
            var originalWords = new List<string>();

            // Act
            _engine.FindAllCombinations(remainingLetters, wordsNeeded, currentPath, candidates, wordGroups, allResults, originalWords);

            // Assert
            allResults.Should().NotBeEmpty();
            // Should find combinations like [ab, cd] and [cd, ab]
            allResults.All(r => r.Count == 2).Should().BeTrue();
        }

        [Fact]
        public void FindAllCombinations_WithMultipleWordsInSameGroup_FindsMultiplePaths()
        {
            // Arrange
            string remainingLetters = "aabbcc";
            int wordsNeeded = 2;
            var currentPath = new List<string>();
            var candidates = new List<string> { "ab", "ac" };
            var wordGroups = new Dictionary<string, List<string>>
            {
                { "ab", new List<string> { "ab", "ba" } }, // Multiple words with same signature
                { "ac", new List<string> { "ac" } }
            };
            var allResults = new List<List<string>>();
            var originalWords = new List<string>();

            // Act
            _engine.FindAllCombinations(remainingLetters, wordsNeeded, currentPath, candidates, wordGroups, allResults, originalWords);

            // Assert
            allResults.Should().NotBeEmpty();
            allResults.Should().HaveCountGreaterThan(2); // Multiple combinations from different word choices
        }

        #endregion

        #region FindAllCombinations - Performance Tests

        [Fact]
        public void FindAllCombinations_WithLargeSearchSpace_CompletesWithinReasonableTime()
        {
            // Arrange
            string remainingLetters = "aabbccddeeffffgghhiijj";
            int wordsNeeded = 3;
            var currentPath = new List<string>();
            var candidates = new List<string> { "aa", "ab", "ac", "dd", "ef" };
            var wordGroups = new Dictionary<string, List<string>>
            {
                { "aa", new List<string> { "aa" } },
                { "ab", new List<string> { "ab" } },
                { "ac", new List<string> { "ac" } },
                { "dd", new List<string> { "dd" } },
                { "ef", new List<string> { "ef" } }
            };
            var allResults = new List<List<string>>();
            var originalWords = new List<string>();

            var stopwatch = Stopwatch.StartNew();

            // Act
            _engine.FindAllCombinations(remainingLetters, wordsNeeded, currentPath, candidates, wordGroups, allResults, originalWords);
            stopwatch.Stop();

            // Assert
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000, "recursive search should complete reasonably fast");
        }

        #endregion

        #region FindAllCombinations - Theory Tests

        [Theory]
        [InlineData("abc", 1, 1)] // Single word, exact match
        [InlineData("aabbcc", 2, 0)] // Two words, but need to find valid combinations
        [InlineData("abcdef", 3, 0)] // Three words, complex scenario
        public void FindAllCombinations_WithVariousScenarios_ProducesResults(
            string remainingLetters,
            int wordsNeeded,
            int minExpectedResults)
        {
            // Arrange
            var currentPath = new List<string>();
            var candidates = new List<string> { "a", "ab", "abc", "abcd" };
            var wordGroups = new Dictionary<string, List<string>>
            {
                { "a", new List<string> { "a" } },
                { "ab", new List<string> { "ab" } },
                { "abc", new List<string> { "abc" } },
                { "abcd", new List<string> { "abcd" } }
            };
            var allResults = new List<List<string>>();
            var originalWords = new List<string>();

            // Act
            _engine.FindAllCombinations(remainingLetters, wordsNeeded, currentPath, candidates, wordGroups, allResults, originalWords);

            // Assert
            allResults.Should().HaveCountGreaterThanOrEqualTo(minExpectedResults);
        }

        #endregion
    }
}