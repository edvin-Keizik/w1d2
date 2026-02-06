using AnagramSolver.Contracts;
using Moq;
using Xunit;


namespace AnagramSolver.BusinessLogic.Tests
{
    public class DictionaryLoaderTest
    {
        [Fact]
        public async Task LoadWords_WhenFileHasValidData_CallsAddWordCorrectly()
        {
            // Arrange
            var mockFileSystem = new Mock<IFileSystemWrapper>();
            var mockProcessor = new Mock<IWordProcessor>();

            var fakeLines = new List<string> { "alus ", "", "labas" };

            mockFileSystem.Setup(f => f.ReadLinesAsync(It.IsAny<string>()))
                .Returns(ToAsyncEnumerable(fakeLines));

            var loader = new DictionaryLoader(mockFileSystem.Object);

            // Act
            await loader.LoadWordsAsync("fakePath.txt", mockProcessor.Object);

            // Assert
            mockProcessor.Verify(p => p.AddWord("alus"), Times.Once);
            mockProcessor.Verify(p => p.AddWord("labas"), Times.Once);
            mockProcessor.Verify(p => p.AddWord(""), Times.Never);
        }

        private async IAsyncEnumerable<string> ToAsyncEnumerable(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                yield return line;
            }
            await Task.CompletedTask;
        }

        [Fact]
        public async Task LoadWords_WhenFileSystemThrows_HandlesExceptionGracefully()
        {
            // Arrange
            var mockFileSystem = new Mock<IFileSystemWrapper>();
            var mockProcessor = new Mock<IWordProcessor>();

            mockFileSystem.Setup(f => f.ReadLinesAsync(It.IsAny<string>()))
                          .Throws(new FileNotFoundException());

            var loader = new DictionaryLoader(mockFileSystem.Object);

            // Act
            var exception = await Record.ExceptionAsync(() => loader.LoadWordsAsync("badpath.txt", mockProcessor.Object));

            // Assert
            Assert.Null(exception);
        }
    }
}
