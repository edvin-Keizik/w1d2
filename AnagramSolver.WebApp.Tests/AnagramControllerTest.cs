using System.Text;
using AnagramSolver.Contracts;
using AnagramSolver.WebApp.Controllers;
using AnagramSolver.WebApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;
using System.Text.Json;

namespace AnagramSolver.WebApp.Tests
{
    public class AnagramControllerTest
    {
        // Simple in-memory ISession implementation for tests
        private class TestSession : ISession
        {
            private readonly Dictionary<string, byte[]> _store = new();
            public IEnumerable<string> Keys => _store.Keys;
            public string Id { get; } = Guid.NewGuid().ToString();
            public bool IsAvailable { get; } = true;

            public void Clear() => _store.Clear();
            public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
            public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
            public void Remove(string key) => _store.Remove(key);
            public void Set(string key, byte[] value) => _store[key] = value;
            public bool TryGetValue(string key, out byte[] value) => _store.TryGetValue(key, out value);
        }

        private static DefaultHttpContext CreateHttpContextWithSession(out TestSession session)
        {
            var context = new DefaultHttpContext();
            session = new TestSession();
            context.Session = session;
            return context;
        }

        [Fact]
        public async Task Index_Get_WithInputWord_PopulatesModel_SetsCookie_And_UpdatesSessionHistory()
        {
            // Arrange
            var mockAnagrams = new Mock<IGetAnagrams>();
            mockAnagrams
                .Setup(x => x.GetAnagramsAsync(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<Func<string, bool>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { new Anagram { Word = "steam" }, new Anagram { Word = "teams" } });

            var settings = new AnagramSettings { MinWordLength = 1, MaxAnagramsToShow = 10 };
            var mockProcessor = new Mock<IWordProcessor>();

            var controller = new AnagramController(mockProcessor.Object, settings, mockAnagrams.Object);

            var httpContext = CreateHttpContextWithSession(out var session);
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            controller.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            // Act
            var result = await controller.Index("steam", CancellationToken.None);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<AnagramViewModel>(viewResult.Model);
            Assert.Equal("steam", model.InputWord);
            Assert.Contains("steam", model.Result);
            Assert.Contains("teams", model.Result);

            // Cookie should have been appended via Set-Cookie header
            Assert.True(httpContext.Response.Headers.TryGetValue("Set-Cookie", out var headerValues));
            Assert.Contains("lastSearch=steam", headerValues.ToString());

            // Session should contain SearchHistory JSON
            var got = session.TryGetValue("SearchHistory", out var raw);
            Assert.True(got);
            var json = Encoding.UTF8.GetString(raw!);
            var history = JsonSerializer.Deserialize<List<SearchHistoryItem>>(json);
            Assert.NotNull(history);
            Assert.Contains(history!, h => h.Word == "steam");
        }

        [Fact]
        public async Task Index_Post_WithValidModel_PopulatesResult_And_SetsViewBag()
        {
            // Arrange
            var mockAnagrams = new Mock<IGetAnagrams>();
            mockAnagrams
                .Setup(x => x.GetAnagramsAsync(
                    It.Is<string>(s => s == "notes"),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<Func<string, bool>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { new Anagram { Word = "stone" } });

            var settings = new AnagramSettings { MinWordLength = 1, MaxAnagramsToShow = 5 };
            var mockProcessor = new Mock<IWordProcessor>();

            var controller = new AnagramController(mockProcessor.Object, settings, mockAnagrams.Object);

            var httpContext = CreateHttpContextWithSession(out var session);
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            controller.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            var model = new AnagramViewModel { InputWord = "notes" };

            // Act
            var result = await controller.Index(model, CancellationToken.None);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var returnedModel = Assert.IsType<AnagramViewModel>(viewResult.Model);
            Assert.Contains("stone", returnedModel.Result);

            // ViewBag.LastSearch should be set
            Assert.Equal("notes", controller.ViewBag.LastSearch);

            // Session history updated
            var got = session.TryGetValue("SearchHistory", out var raw);
            Assert.True(got);
            var history = JsonSerializer.Deserialize<List<SearchHistoryItem>>(Encoding.UTF8.GetString(raw!));
            Assert.NotNull(history);
            Assert.Contains(history!, h => h.Word == "notes");
        }

        [Fact]
        public async Task AddWord_Post_Success_RedirectsToDictionary_AndSetsTempData()
        {
            // Arrange
            var mockProcessor = new Mock<IWordProcessor>();
            mockProcessor.Setup(p => p.AddWordAsync("newword")).ReturnsAsync(true);

            var mockAnagrams = new Mock<IGetAnagrams>();
            var settings = new AnagramSettings();

            var controller = new AnagramController(mockProcessor.Object, settings, mockAnagrams.Object);
            var httpContext = CreateHttpContextWithSession(out _);
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            controller.TempData = tempData;

            var model = new AnagramViewModel { InputWord = "newword" };

            // Act
            var result = await controller.AddWord(model);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Dictionary", redirect.ActionName);
            Assert.True(controller.TempData.ContainsKey("SuccessMessage"));
            Assert.Contains("newword", controller.TempData["SuccessMessage"]?.ToString());
        }

        [Fact]
        public async Task AddWord_Post_Duplicate_ReturnsViewWithErrorMessage()
        {
            // Arrange
            var mockProcessor = new Mock<IWordProcessor>();
            mockProcessor.Setup(p => p.AddWordAsync("dup")).ReturnsAsync(false);

            var mockAnagrams = new Mock<IGetAnagrams>();
            var settings = new AnagramSettings();

            var controller = new AnagramController(mockProcessor.Object, settings, mockAnagrams.Object);
            var httpContext = CreateHttpContextWithSession(out _);
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            controller.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            var model = new AnagramViewModel { InputWord = "dup" };

            // Act
            var result = await controller.AddWord(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(model, viewResult.Model);
            Assert.Equal("Sis zodis jau yra zodyne!", controller.ViewBag.ErrorMessage);
        }

        [Fact]
        public async Task Dictionary_ReturnsPagedWords_And_ViewBagPagination()
        {
            // Arrange
            var words = Enumerable.Range(1, 200).Select(i => $"word{i}").ToList();
            var mockProcessor = new Mock<IWordProcessor>();
            mockProcessor.Setup(p => p.GetDictionary()).ReturnsAsync(words);

            var mockAnagrams = new Mock<IGetAnagrams>();
            var settings = new AnagramSettings();

            var controller = new AnagramController(mockProcessor.Object, settings, mockAnagrams.Object);
            var httpContext = CreateHttpContextWithSession(out _);
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            controller.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            // Act - request page 2
            var result = await controller.Dictionary(page: 2);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<string>>(viewResult.Model);
            // default pageSize in controller is 90, page 2 should have 90 items (items 91-180)
            Assert.Equal(90, model.Count);
            Assert.Equal(2, controller.ViewBag.CurrentPage);
            Assert.Equal((int)Math.Ceiling(words.Count / 90.0), controller.ViewBag.TotalPages);
        }
    }
}