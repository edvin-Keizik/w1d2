using Moq;
using Xunit;
using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;
using AnagramSolver.BusinessLogic.ChainOfResponsibility;
using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.BusinessLogic.Tests
{
    public class WordProcessorTest : IDisposable
    {
        private readonly AnagramDbContext _context;
        private readonly Mock<IAnagramSearchEngine> _mockEngine;
        private readonly FilterPipeline _mockFilterPipeline;

        public WordProcessorTest()
        {
            // Set up a fresh In-Memory Database for every test
            var options = new DbContextOptionsBuilder<AnagramDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AnagramDbContext(options);
            _mockEngine = new Mock<IAnagramSearchEngine>();
            _mockFilterPipeline = new FilterPipeline();

            // Note: In real scenarios, you'd add steps to the pipeline here if needed
        }

        private async Task SeedData(string signature, string words)
        {
            _context.WordGroupsEntity.Add(new WordGroupsEntity
            {
                Signature = signature,
                Words = words
            });
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetSignature_ReturnsSortedLetters_ThroughSearchEngineVerify()
        {
            // Arrange
            await SeedData("alsu", "alus,sula");
            var processor = new WordProcessor(_mockEngine.Object, _mockFilterPipeline, _context);

            // Act
            await processor.GetAnagramsAsync("alus", 1, 1, w => true);

            // Assert - Check if the letter bank was sorted correctly to "alsu"
            _mockEngine.Verify(e => e.FindAllCombinations(
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
        public async Task GetAnagrams_WhenMatchesFoundInDb_ReturnsResults()
        {
            // Arrange
            await SeedData("alsu", "sula");

            // Setup Mock to simulate finding one combination
            _mockEngine.Setup(e => e.FindAllCombinations(
                It.IsAny<string>(), It.IsAny<int>(), It.IsAny<List<string>>(),
                It.IsAny<List<string>>(), It.IsAny<Dictionary<string, List<string>>>(),
                It.IsAny<List<List<string>>>(), It.IsAny<IEnumerable<string>>()))
            .Callback<string, int, List<string>, List<string>, Dictionary<string, List<string>>, List<List<string>>, IEnumerable<string>>(
                (bank, count, path, cand, group, allResults, orig) =>
                {
                    allResults.Add(new List<string> { "sula" });
                });

            var processor = new WordProcessor(_mockEngine.Object, _mockFilterPipeline, _context);

            // Act
            var result = await processor.GetAnagramsAsync("alus", 1, 1, w => true);

            // Assert
            Assert.Single(result);
            Assert.Equal("sula", result.First().Word);
        }

        [Fact]
        public async Task GetCandidates_WhenWordIsLongerThanInput_ShouldBeFiltered()
        {
            // Arrange
            await SeedData("aabhlpet", "alphabet"); // Longer than "alus"

            var processor = new WordProcessor(_mockEngine.Object, _mockFilterPipeline, _context);

            // Act
            await processor.GetAnagramsAsync("alus", 1, 1, w => true);

            // Assert - Candidates list passed to SearchEngine should be empty
            _mockEngine.Verify(e => e.FindAllCombinations(
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
        public async Task GetAnagramsAsync_WhenCancelled_ThrowsException()
        {
            // Arrange
            var processor = new WordProcessor(_mockEngine.Object, _mockFilterPipeline, _context);
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                processor.GetAnagramsAsync("alus", 1, 1, w => true, cts.Token));
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}