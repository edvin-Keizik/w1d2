using Moq;
using Xunit;
using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;


namespace AnagramSolver.BusinessLogic.Tests
{
    public class WordProcessorTest
    {
        [Fact]
        public void AddWord_WhenSignatureIsNew_AddsWordToDictionary()
        {
            // Arrange
            var mockEngine = new Mock<IAnagramSearchEngine>();
            var processor = new WordProcessor(mockEngine.Object);
            string testWord = "alus";
            string signature = "alsu";

            // Act
            processor.AddWord(testWord);

            // Assert
            processor.GetAnagrams(testWord, 1, 3);

            mockEngine.Verify(e => e.FindAllCombinations(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<List<string>>(),
                It.IsAny<List<string>>(),
                It.Is<Dictionary<string, List<string>>>(dict =>
                    dict.ContainsKey(signature) && dict[signature].Contains(testWord)),
                It.IsAny<List<List<string>>>(),
                It.IsAny<IEnumerable<string>>()
            ), Times.Once);
        }

        [Fact]
        public void AddWord_WhenSignatureExists_AddsWordToDictionary()
        {
            // Arrange
            var mockEngine = new Mock<IAnagramSearchEngine>();
            var processor = new WordProcessor(mockEngine.Object);
            string testWord = "alus";
            string signature = "alsu";
            string egzistingWord = "sula";

            // Act
            processor.AddWord(egzistingWord);
            processor.AddWord(testWord);

            // Assert
            processor.GetAnagrams(testWord, 1, 3);

            mockEngine.Verify(e => e.FindAllCombinations(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<List<string>>(),
                It.IsAny<List<string>>(),
                It.Is<Dictionary<string, List<string>>>(dict =>
            dict.ContainsKey(signature) &&
            dict[signature].Count == 2 &&
            dict[signature].Contains(testWord) &&
            dict[signature].Contains(egzistingWord)
            ),
                It.IsAny<List<List<string>>>(),
                It.IsAny<IEnumerable<string>>()
            ), Times.Once);
        }

        [Fact]
        public void GetSignature_CheckIfReturnSorrtedLetters()
        {
            // Arrange
            var mockEngine = new Mock<IAnagramSearchEngine>();
            var processor = new WordProcessor(mockEngine.Object);

            // Act
            processor.GetAnagrams("alus", 1, 3);

            // Assert
            mockEngine.Verify(e => e.FindAllCombinations(
                "alsu",
                It.IsAny<int>(),
                It.IsAny<List<string>>(),
                It.IsAny<List<string>>(),
                It.IsAny<Dictionary<string, List<string>>>(),
                It.IsAny<List<List<string>>>(),
                It.IsAny<IEnumerable<string>>()
            ), Times.Once);
        }

        [Fact]
        public void GetCandidatesKey_CheckIfReturnRightValue()
        {
            // Arrange
            var mockEngine = new Mock<IAnagramSearchEngine>();
            mockEngine.Setup(e => e.CanSubstract(It.IsAny<string>(), It.IsAny<string>(), out It.Ref<string>.IsAny))
                .Returns(true);

            var processor = new WordProcessor(mockEngine.Object);
            processor.AddWord("sula");

            // Act
            processor.GetAnagrams("alus", 1, 3);

            // Assert
            mockEngine.Verify(e => e.FindAllCombinations(
                "alsu",
                1,
                It.IsAny<List<string>>(),
                It.Is<List<string>>(list => list.Contains("alsu")),
                It.IsAny<Dictionary<string, List<string>>>(),
                It.IsAny<List<List<string>>>(),
                It.IsAny<IEnumerable<string>>()
            ), Times.Once);
        }

        [Fact]
        public void GetCandidatesKey_WhenWordInDictionaryIsLongerThanInput_ShouldNotBeAddedToCandidates()
        {
            // Arrange
            var mockEngine = new Mock<IAnagramSearchEngine>();
            mockEngine.Setup(e => e.CanSubstract(It.IsAny<string>(), It.IsAny<string>(), out It.Ref<string>.IsAny))
                .Returns(true);

            var processor = new WordProcessor(mockEngine.Object);
            processor.AddWord("alphabet");

            // Act
            processor.GetAnagrams("alus", 1, 3);

            // Assert
            mockEngine.Verify(e => e.FindAllCombinations(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<List<string>>(),
                It.Is<List<string>>(list => list.Count == 0),
                It.IsAny<Dictionary<string, List<string>>>(),
                It.IsAny<List<List<string>>>(),
                It.IsAny<IEnumerable<string>>()
            ), Times.Once);
        }

        [Fact]
        public void GetAnagram_WhenNoAnagramPassed_ReturnEmptyList()
        {
            // Arrange
            var mockEngine = new Mock<IAnagramSearchEngine>();
            var processor = new WordProcessor(mockEngine.Object);

            processor.AddWord("labas");

            // Act
            List<Anagram> result = processor.GetAnagrams("alus", 1, 3);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetAnagram_WhenAnagramGetOneWord_ReturnResult()
        {
            // Arrange
            var mockEngine = new Mock<IAnagramSearchEngine>();
            mockEngine.Setup(e => e.FindAllCombinations(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<List<string>>(),
                It.IsAny<List<string>>(),
                It.IsAny<Dictionary<string, List<string>>>(),
                It.IsAny<List<List<string>>>(),
                It.IsAny<IEnumerable<string>>()))
            .Callback<
                string,
                int,
                List<string>,
                List<string>,
                Dictionary<string,
                List<string>>,
                List<List<string>>,
                IEnumerable<string>>(
                (bank, count, path, cand, group, allResults, orig) =>
                {
                    allResults.Add(new List<string> { "sula" });
                });

            var processor = new WordProcessor(mockEngine.Object);

            // Act
            var result = processor.GetAnagrams("alus", 1, 3);

            // Assert
            Assert.Single(result);
            Assert.Contains("sula", result[0].Word);
        }

        [Fact]
        public void GetAnagram_WhenAnagramGetTwoWords_ReturnResult()
        {
            // Arrange
            var mockEngine = new Mock<IAnagramSearchEngine>();
            mockEngine.Setup(e => e.FindAllCombinations(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<List<string>>(),
                It.IsAny<List<string>>(),
                It.IsAny<Dictionary<string, List<string>>>(),
                It.IsAny<List<List<string>>>(),
                It.IsAny<IEnumerable<string>>()))
            .Callback<
                string,
                int,
                List<string>,
                List<string>,
                Dictionary<string,
                List<string>>,
                List<List<string>>,
                IEnumerable<string>>(
                (bank, count, path, cand, group, allResults, orig) =>
                {
                    allResults.Add(new List<string> { "sula", "balas" });
                });

            var processor = new WordProcessor(mockEngine.Object);

            // Act
            var result = processor.GetAnagrams("alus labas", 2, 3);

            // Assert
            Assert.Single(result);
            Assert.Contains("sula balas", result[0].Word);
        }

        [Fact]
        public void GetAnagrams_WhenWordIsTooShort_ShouldBeFilteredOut()
        {
            // Arrange
            var mockEngine = new Mock<IAnagramSearchEngine>();
            mockEngine.Setup(e => e.FindAllCombinations(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<List<string>>(),
                It.IsAny<List<string>>(),
                It.IsAny<Dictionary<string, List<string>>>(),
                It.IsAny<List<List<string>>>(),
                It.IsAny<IEnumerable<string>>()))
            .Callback<
                string,
                int,
                List<string>,
                List<string>,
                Dictionary<string,
                List<string>>,
                List<List<string>>,
                IEnumerable<string>>(
                (bank, count, path, cand, group, allResults, orig) =>
                {
                    allResults.Add(new List<string> { "su"});
                });

            var processor = new WordProcessor(mockEngine.Object);

            // Act
            var result = processor.GetAnagrams("us", 1, 3);

            // Assert
            Assert.Empty(result);

        }

        [Fact]
        public void GetAnagrams_WhenWordIsLongEnough_ShouldBeKept()
        {
            // Arrange
            var mockEngine = new Mock<IAnagramSearchEngine>();
            mockEngine.Setup(e => e.FindAllCombinations(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<List<string>>(),
                It.IsAny<List<string>>(),
                It.IsAny<Dictionary<string, List<string>>>(),
                It.IsAny<List<List<string>>>(),
                It.IsAny<IEnumerable<string>>()))
            .Callback<
                string,
                int,
                List<string>,
                List<string>,
                Dictionary<string,
                List<string>>,
                List<List<string>>,
                IEnumerable<string>>(
                (bank, count, path, cand, group, allResults, orig) =>
                {
                    allResults.Add(new List<string> { "labas" });
                });

            var processor = new WordProcessor(mockEngine.Object);

            // Act
            var result = processor.GetAnagrams("balas", 1, 3);

            // Assert
            Assert.Single(result);
            Assert.Contains("labas", result[0].Word);

        }
    }
}
