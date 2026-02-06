using Moq;
using Xunit;
using AnagramSolver.Contracts;
using AnagramSolver.Cli;


namespace AnagramSolver.Cli.Tests
{
    public class AppTest
    {
        [Fact]
        public async Task App_Run_FullFlow_DisplaysAnagramsAndExits()
        {
            // Arrange
            var mockUI = new Mock<IUserInputOutput>();
            var mockProcessor = new Mock<IWordProcessor>();
            var mockLoader = new Mock<IDictionaryLoader>();
            var settings = new AnagramSettings { MinWordLength = 3, MaxAnagramsToShow = 1 };

            mockUI.SetupSequence(u => u.ReadLine())
                .Returns("alus")
                .Returns("0");

            var expectedAnagrams = new List<Anagram> { new Anagram { Word = "alsu" } };

            var cts = new CancellationTokenSource();
            cts.Cancel();

            mockProcessor.Setup(p => p.GetAnagramsAsync("alus", 1, 3, cts.Token))
                         .ReturnsAsync(expectedAnagrams);

            var app = new App("fakePath.txt", settings, mockProcessor.Object, mockLoader.Object, mockUI.Object);

            // Act
            await app.Run(cts.Token);

            // Assert
            mockUI.Verify(u => u.WriteLine("Atsisiunciamas zodynas"), Times.Once);

            mockLoader.Verify(l => l.LoadWordsAsync("fakePath.txt", mockProcessor.Object), Times.Once);

            mockUI.Verify(u => u.WriteLine("alsu"), Times.Once);
        }

        [Fact]
        public async Task App_Run_WhenWordTooShort_ShowsErrorMessage()
        {
            // Arrange
            var mockUI = new Mock<IUserInputOutput>();
            var mockProcessor = new Mock<IWordProcessor>();
            var mockLoader = new Mock<IDictionaryLoader>();
            var settings = new AnagramSettings { MinWordLength = 3 };

            mockUI.SetupSequence(u => u.ReadLine())
                  .Returns("a")
                  .Returns("0");

            var app = new App("path.txt", settings, mockProcessor.Object, mockLoader.Object, mockUI.Object);

            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act
            await app.Run(cts.Token);

            // Assert
            mockUI.Verify(u => u.WriteLine(It.Is<string>(s => s.Contains("per trumpas"))), Times.Once);

            mockProcessor.Verify(p => p.GetAnagramsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), cts.Token), Times.Never);
        }

        [Fact]
        public async Task App_Run_WhenNoAnagramsFound_DisplaysMissingMessage()
        {
            // Arrange
            var mockUI = new Mock<IUserInputOutput>();
            var mockProcessor = new Mock<IWordProcessor>();
            var mockLoader = new Mock<IDictionaryLoader>();

            var cts = new CancellationTokenSource();
            cts.Cancel();

            mockUI.SetupSequence(u => u.ReadLine()).Returns("ąžuolas").Returns("0");

            mockProcessor.Setup(p => p.GetAnagramsAsync("ąžuolas", It.IsAny<int>(), It.IsAny<int>(), cts.Token))
                         .ReturnsAsync(new List<Anagram>());

            var app = new App(
                "path.txt",
                new AnagramSettings { MinWordLength = 3 },
                mockProcessor.Object,
                mockLoader.Object,
                mockUI.Object
            );

            // Act
            await app.Run(cts.Token);

            // Assert
            mockUI.Verify(u => u.WriteLine(It.Is<string>(s => s.Contains("nera"))), Times.Once);
        }
    }
}