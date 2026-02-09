using Moq;
using Xunit;
using AnagramSolver.Contracts;
using AnagramSolver.Cli;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace AnagramSolver.Cli.Tests
{
    public class AppTest
    {
        private readonly Mock<IUserInputOutput> _mockUI;
        private readonly AnagramSettings _settings;

        public AppTest()
        {
            _mockUI = new Mock<IUserInputOutput>();
            _settings = new AnagramSettings { MinWordLength = 3, MaxAnagramsToShow = 1 };
        }

        private HttpClient CreateMockHttpClient(HttpResponseMessage response)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(response)
               .Verifiable();

            return new HttpClient(handlerMock.Object);
        }

        [Fact]
        public async Task App_Run_WhenApiReturnsAnagrams_DisplaysThem()
        {
            // Arrange
            var mockUI = new Mock<IUserInputOutput>();
            mockUI.SetupSequence(u => u.ReadLine()).Returns("alus").Returns("0");

            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("[\"sula\", \"alus\"]")
            };

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var fakeClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://fake.com/") };
            var app = new App(new AnagramSettings { MinWordLength = 3 }, mockUI.Object, fakeClient);

            // Act
            await app.Run(CancellationToken.None);

            // Assert
            mockUI.Verify(u => u.WriteLine(It.Is<string>(s => s.Contains("sula"))), Times.Once);
        }

        [Fact]
        public async Task App_Run_WhenWordTooShort_ShowsErrorMessage()
        {
            // Arrange
            _mockUI.SetupSequence(u => u.ReadLine())
                  .Returns("a")
                  .Returns("0");

            var handlerMock = new Mock<HttpMessageHandler>();
            var fakeClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://fake.com/") };

            var app = new App(_settings, _mockUI.Object, fakeClient);

            // Act
            await app.Run(CancellationToken.None);

            // Assert
            _mockUI.Verify(u => u.WriteLine(It.Is<string>(s => s.Contains("per trumpas"))), Times.Once);
        }
    }
}