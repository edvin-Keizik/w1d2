using AnagramSolver.Contracts;
using Moq;
using Xunit;


namespace AnagramSolver.BusinessLogic.Tests
{
    public class DictionaryLoaderTest
    {
        [Fact]
        public void LoadWords_WhenFileHasValidData_CallsAddWordCorrectly()
        {
            // Arrange
            var mockFileSystem = new Mock<IFileSystemWrapper>();
            var mockProcessor = new Mock<IWordProcessor>();

            var fakeLines = new List<string> { "alus ", "", "labas" };

            mockFileSystem.Setup(f => f.ReadLines(It.IsAny<string>()))
                .Returns(fakeLines);

            var loader = new DictionaryLoader(mockFileSystem.Object);

            // Act
            loader.LoadWords("fakePath.txt", mockProcessor.Object);

            // Assert
            mockProcessor.Verify(p => p.AddWord("alus"), Times.Once);
            mockProcessor.Verify(p => p.AddWord("labas"), Times.Once);
            mockProcessor.Verify(p => p.AddWord(""), Times.Never);
        }

        [Fact]
        public void LoadWords_WhenFileSystemThrows_HandlesExceptionGracefully()
        {
            // Arrange
            var mockFileSystem = new Mock<IFileSystemWrapper>();
            var mockProcessor = new Mock<IWordProcessor>();

            mockFileSystem.Setup(f => f.ReadLines(It.IsAny<string>()))
                          .Throws(new FileNotFoundException());

            var loader = new DictionaryLoader(mockFileSystem.Object);

            // Act
            var exception = Record.Exception(() => loader.LoadWords("badpath.txt", mockProcessor.Object));

            // Assert
            Assert.Null(exception);
        }
    }
}
