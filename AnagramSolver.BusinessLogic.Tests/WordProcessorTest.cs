using Moq;
using Xunit;
using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;
using AnagramSolver.BusinessLogic.ChainOfResponsibility;


namespace AnagramSolver.BusinessLogic.Tests
{
    public class WordProcessorTest
    {
        [Fact]
        public async Task AddWord_WhenSignatureIsNew_AddsWordToDictionary()
        {
            // Arrange
            var mockEngine = new Mock<IAnagramSearchEngine>();
            var mockFilterPipeline = new FilterPipeline();
            var processor = new WordProcessor(mockEngine.Object, mockFilterPipeline);
            string testWord = "alus";
            string signature = "alsu";

            // Act
            processor.AddWord(testWord);

            // Assert
            await processor.GetAnagramsAsync(testWord, 1, 1, w => w.Length > 3, CancellationToken.None);

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
        public async Task AddWord_WhenSignatureExists_AddsWordToDictionary()
        {
            // Arrange
            var mockEngine = new Mock<IAnagramSearchEngine>();
            var mockFilterPipeline = new FilterPipeline();
            var processor = new WordProcessor(mockEngine.Object, mockFilterPipeline);
            string testWord = "alus";
            string signature = "alsu";
            string egzistingWord = "sula";


            // Act
            processor.AddWord(egzistingWord);
            processor.AddWord(testWord);

            // Assert
            await processor.GetAnagramsAsync(testWord, 1, 1, w => w.Length > 3, CancellationToken.None);

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
        public async Task AddWord_WhenWordExist_SkipWord()
        {
            // Arrange
            var mockEngine = new Mock<IAnagramSearchEngine>();
            var mockFilterPipeline = new FilterPipeline();
            var processor = new WordProcessor(mockEngine.Object, mockFilterPipeline);
            string testWord = "alus";
            string signature = "alsu";
            string egzistingWord = "alus";

            // Act
            processor.AddWord(egzistingWord);
            processor.AddWord(testWord);

            // Assert
            await processor.GetAnagramsAsync(testWord, 1, 1, w => w.Length > 3, CancellationToken.None);

            mockEngine.Verify(e => e.FindAllCombinations(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<List<string>>(),
                It.IsAny<List<string>>(),
                It.Is<Dictionary<string, List<string>>>(dict =>
            dict.ContainsKey(signature) &&
            dict[signature].Count == 1 &&
            dict[signature].Contains(egzistingWord)
            ),
                It.IsAny<List<List<string>>>(),
                It.IsAny<IEnumerable<string>>()
            ), Times.Once);
        }

        [Fact]
        public async Task GetSignature_CheckIfReturnSorrtedLetters()
        {
            // Arrange
            var mockEngine = new Mock<IAnagramSearchEngine>();
            var mockFilterPipeline = new FilterPipeline();
            var processor = new WordProcessor(mockEngine.Object, mockFilterPipeline);

            // Act
            await processor.GetAnagramsAsync("alus", 1, 1, w => w.Length > 3, CancellationToken.None);

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
        public async Task GetCandidatesKey_CheckIfReturnRightValue()
        {
            // Arrange
            var mockEngine = new Mock<IAnagramSearchEngine>();
            mockEngine.Setup(e => e.CanSubstract(It.IsAny<string>(), It.IsAny<string>(), out It.Ref<string>.IsAny))
                .Returns(true);

            var mockFilterPipeline = new FilterPipeline();
            var processor = new WordProcessor(mockEngine.Object, mockFilterPipeline);
            processor.AddWord("sula");

            // Act
            await processor.GetAnagramsAsync("alus", 1, 1, w => w.Length > 3, CancellationToken.None);

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
        public async Task GetCandidatesKey_WhenWordInDictionaryIsLongerThanInput_ShouldNotBeAddedToCandidates()
        {
            // Arrange
            var mockEngine = new Mock<IAnagramSearchEngine>();
            mockEngine.Setup(e => e.CanSubstract(It.IsAny<string>(), It.IsAny<string>(), out It.Ref<string>.IsAny))
                .Returns(true);

            var mockFilterPipeline = new FilterPipeline();
            var processor = new WordProcessor(mockEngine.Object, mockFilterPipeline);
            processor.AddWord("alphabet");

            // Act
            await processor.GetAnagramsAsync("alus", 1, 1, w => w.Length > 3, CancellationToken.None);

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
        public async Task GetAnagram_WhenNoAnagramPassed_ReturnEmptyList()
        {
            // Arrange
            var mockEngine = new Mock<IAnagramSearchEngine>();
            var mockFilterPipeline = new FilterPipeline();
            var processor = new WordProcessor(mockEngine.Object, mockFilterPipeline);

            processor.AddWord("labas");

            // Act
            IEnumerable<Anagram> result = await processor.GetAnagramsAsync("alus", 1, 1, w => w.Length > 3, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAnagram_WhenAnagramGetOneWord_ReturnResult()
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

            var mockFilterPipeline = new FilterPipeline();
            var processor = new WordProcessor(mockEngine.Object, mockFilterPipeline);

            // Act
            var result = await processor.GetAnagramsAsync("alus", 1, 1, w => w.Length > 3, CancellationToken.None);

            // Assert
            Assert.Single(result);
            var listResult = result.ToList();
            Assert.Contains("sula", listResult[0].Word);
        }

        [Fact]
        public async Task GetAnagram_WhenAnagramGetTwoWords_ReturnResult()
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
                    if(count == 2)
                    {
                    allResults.Add(new List<string> { "sula", "balas" });
                    }
                });

            var mockFilterPipeline = new FilterPipeline();
            var processor = new WordProcessor(mockEngine.Object, mockFilterPipeline);

            // Act
            var result = await processor.GetAnagramsAsync("alus labas", 2, 1, w => w.Length > 3, CancellationToken.None);

            // Assert
            Assert.Single(result);
            var listResult = result.ToList();
            Assert.Contains("sula balas", listResult[0].Word);
        }

        [Fact]
        public async Task GetAnagrams_WhenWordIsTooShort_ShouldBeFilteredOut()
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

            var mockFilterPipeline = new FilterPipeline();
            var processor = new WordProcessor(mockEngine.Object, mockFilterPipeline);

            // Act
            var result = await processor.GetAnagramsAsync("us", 1, 1, w => w.Length > 3, CancellationToken.None);

            // Assert
            Assert.Empty(result);

        }

        [Fact]
        public async Task GetAnagrams_WhenWordIsLongEnough_ShouldBeKept()
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

            var mockFilterPipeline = new FilterPipeline();
            var processor = new WordProcessor(mockEngine.Object, mockFilterPipeline);

            // Act
            var result = await processor.GetAnagramsAsync("balas", 1, 1, w => w.Length > 3, CancellationToken.None);

            // Assert
            Assert.Single(result);
            var listResult = result.ToList();
            Assert.Contains("labas", listResult[0].Word);

        }

        [Fact]
        public async Task GetAnagramsAsync_WhenTokenIsCancelled_ThrowsOperationCanceledException()
        {
            // Arrange
            var mockEngine = new Mock<IAnagramSearchEngine>();
            var mockFilterPipeline = new FilterPipeline();
            var processor = new WordProcessor(mockEngine.Object, mockFilterPipeline);

            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                processor.GetAnagramsAsync("alus", 1, 1, w => w.Length > 3, cts.Token));
        }
    }
}
