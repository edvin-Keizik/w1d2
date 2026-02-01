using Xunit;
using AnagramSolver.BusinessLogic;
using System.Text;

namespace AnagramSolver.BusinessLogic.Tests
{
    public class SearchEngineTest
    {
        [Fact]
        public void CanSubtrsct_Validator_ReturnTrueAndCorrectLeftover()
        {
            // Arrange
            var engine = new AnagramSearchEngine();
            string letterBank = "aalmnsu";// 'alus' + 'man'
            string signature = "alsu";//alus

            // Act
            bool result = engine.CanSubstract(letterBank, signature, out string leftover);
            
            // Assert
            Assert.True(result);
            Assert.Equal("amn", leftover);//man
        }

        [Theory]
        [InlineData("alsu", "zz")]
        [InlineData("alsz", "alsu")]
        [InlineData("alsu", "alsz")]
        public void CanSubstract_InvalidInputs_ReturnsFalse(string letterBank, string signature)
        {
            // Arrange
            var engine = new AnagramSearchEngine();

            // Act
            bool result = engine.CanSubstract(letterBank, signature, out _);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void FindAllCombinations_GivenBank_FindsMultipleValidSentences()
        {
            // Arrange
            var engine = new AnagramSearchEngine();
            string letterBank = "aalmnsu";// 'alus' + 'man'
            var wordGroup = new Dictionary<string, List<String>>
            {
                { "alsu", new List<string> { "alus" } },
                { "amn", new List<string> { "man" } },
            };
            var candidates = new List<string> { "alsu", "amn"};
            var result = new List<List<string>>();
            var originalWords = new List<string>();

            // Act
            engine.FindAllCombinations(letterBank, 2, new List<string>(), candidates, wordGroup, result, originalWords);

            // Assert
            Assert.Contains("alus", result[0]);
            Assert.Contains("man", result[0]);
        }

        [Fact]
        public void FindAllCombinations_WhenWordInOriginalWords_ShouldSkipIt()
        {
            // Arrange
            var engine = new AnagramSearchEngine();
            string letterBank = "alsu";// 'alus'
            var wordGroup = new Dictionary<string, List<String>>{ { "alsu", new List<string> { "alus" } }};
            var candidates = new List<string> { "alsu"};
            var result = new List<List<string>>();
            var originalWords = new List<string> { "alus"};

            // Act
            engine.FindAllCombinations(letterBank, 1, new List<string>(), candidates, wordGroup, result, originalWords);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void FindAllCombinations_WhenSignatureHasMultipleWords_ReturnBoth()
        {
            // Arragne
            var engine = new AnagramSearchEngine();
            string letterBank = "alsu";
            var wordGroup = new Dictionary<string, List<String>> { { "alsu", new List<string> { "alus", "sula"} } };
            var candidates = new List<string> { "alsu" };
            var result = new List<List<string>>();
            var originalWords = new List<string>();

            // Act
            engine.FindAllCombinations(letterBank, 1, new List<string>(), candidates, wordGroup, result, originalWords);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains("alus", result[0]);
            Assert.Contains("sula", result[1]);
        }

        [Fact]
        public void FindAllCombinations_WhenNoWordsNeeded_AndLettersRemaining_ReturnEmpty()
        {
            // Arrange
            var engine = new AnagramSearchEngine();
            var result = new List<List<string>>();

            // Act
            engine.FindAllCombinations("abc", 0, new List<string>(), new List<string>(), new Dictionary<string, List<string>>(), result, new List<string>());

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void FindAllCombinations_WhenCanSubstractResturnFalse_ReturnEmpty()
        {
            // Arrange
            var engine = new AnagramSearchEngine();
            var result = new List<List<string>>();
            var wordGroup = new Dictionary<string, List<string>> { { "zz", new List<string> { "zz" } } };
            var candidates = new List<string> { "zz" };

            // Act
            engine.FindAllCombinations("alsu", 1, new List<string>(), candidates, wordGroup, result, new List<string>());

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void FindAllCombinations_WhenNoWordsNeeded_AndNoLettersRemaining_ReturnEmpty()
        {
            // Arrange
            var engine = new AnagramSearchEngine();
            var result = new List<List<string>>();

            // Act
            engine.FindAllCombinations("", 0, new List<string> { "alus" }, new List<string>(), new Dictionary<string, List<string>>(), result, new List<string>());

            // Assert
            Assert.Single(result);
            Assert.Contains("alus", result[0]);
        }

    }
}