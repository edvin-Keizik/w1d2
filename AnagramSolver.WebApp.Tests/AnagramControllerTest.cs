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
        public void Index_WithValidWord_ReturnsAnagrams()
        {
            // Arrange
            var mockProcessor = new Mock<IWordProcessor>();
            var mockSettings = new AnagramSettings { MaxAnagramsToShow = 2, MinWordLength = 2};

            mockProcessor.Setup(word => word.GetAnagrams("alus", It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new List<Anagram> { new Anagram { Word = "sula" } });

            var controller = new AnagramController(mockProcessor.Object, mockSettings);

            // Act
            var result = controller.Index("alus") as ViewResult;
            var model = result.Model as AnagramViewModel;

            // Assert
            Assert.NotNull(model);
            Assert.Contains("sula", model.Result);
            Assert.Equal("alus", model.InputWord);
        }
        [Fact]
        public void Index_WithNoAnagramWord_ReturnsNoAnagramMessage()
        {
            // Arrange
            var mockProcessor = new Mock<IWordProcessor>();
            var mockSettings = new AnagramSettings { MaxAnagramsToShow = 2, MinWordLength = 2 };

            mockProcessor.Setup(word => word.GetAnagrams("man", It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new List<Anagram>());

            var controller = new AnagramController(mockProcessor.Object, mockSettings);

            // Act
            var result = controller.Index("man") as ViewResult;
            var model = result.Model as AnagramViewModel;


            // Assert
            Assert.NotNull(model);
            Assert.Empty(model.Result);
            Assert.Equal("man", model.InputWord);
        }

        [Fact]
        public void Index_WhenNullOrWhitespace_ReturnsNull()
        {
            // Arrange
            var mockProcessor = new Mock<IWordProcessor>();
            var mockSettings = new AnagramSettings { MaxAnagramsToShow = 2, MinWordLength = 2 };
            var controller = new AnagramController(mockProcessor.Object, mockSettings);

            // Act
            var result = controller.Index("") as ViewResult;
            var model = result.Model as AnagramViewModel;

            // Assert
            Assert.NotNull(model);

            mockProcessor.Verify(word => word.GetAnagrams(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>()),
                Times.Never);
        }
    }
}