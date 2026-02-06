using AnagramSolver.Contracts;
using AnagramSolver.WebApp.Controllers;
using AnagramSolver.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;

namespace AnagramSolver.WebApp.Tests
{
    public class AnagramControllerTest
    {
        [Fact]
        public async Task Index_WithValidWord_ReturnsAnagrams()
        {
            // Arrange
            var mockProcessor = new Mock<IWordProcessor>();
            var mockSettings = new AnagramSettings { MaxAnagramsToShow = 2, MinWordLength = 2};

            var cts = new CancellationTokenSource();
            cts.Cancel();

            mockProcessor.Setup(word => word.GetAnagramsAsync("alus", It.IsAny<int>(), It.IsAny<int>(), cts.Token))
                .ReturnsAsync(new List<Anagram> { new Anagram { Word = "sula" } });

            var controller = new AnagramController(mockProcessor.Object, mockSettings);

            // Act
            var result = await controller.Index("alus", cts.Token) as ViewResult;
            var model = result.Model as AnagramViewModel;

            // Assert
            Assert.NotNull(model);
            Assert.Contains("sula", model.Result);
            Assert.Equal("alus", model.InputWord);
        }
        [Fact]
        public async Task Index_WithNoAnagramWord_ReturnsNoAnagramMessage()
        {
            // Arrange
            var mockProcessor = new Mock<IWordProcessor>();
            var mockSettings = new AnagramSettings { MaxAnagramsToShow = 2, MinWordLength = 2 };

            var cts = new CancellationTokenSource();
            cts.Cancel();

            mockProcessor.Setup(word => word.GetAnagramsAsync("man", It.IsAny<int>(), It.IsAny<int>(), cts.Token))
                .ReturnsAsync(new List<Anagram>());

            var controller = new AnagramController(mockProcessor.Object, mockSettings);

            // Act
            var result = await controller.Index("man", cts.Token) as ViewResult;
            var model = result.Model as AnagramViewModel;


            // Assert
            Assert.NotNull(model);
            Assert.Empty(model.Result);
            Assert.Equal("man", model.InputWord);
        }

        [Fact]
        public async Task Index_WhenNullOrWhitespace_ReturnsNull()
        {
            // Arrange
            var mockProcessor = new Mock<IWordProcessor>();
            var mockSettings = new AnagramSettings { MaxAnagramsToShow = 2, MinWordLength = 2 };
            var controller = new AnagramController(mockProcessor.Object, mockSettings);

            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act
            var result = await controller.Index("", cts.Token) as ViewResult;
            var model = result.Model as AnagramViewModel;

            // Assert
            Assert.NotNull(model);

            mockProcessor.Verify(word => word.GetAnagramsAsync(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                cts.Token),
                Times.Never);
        }
    }
}