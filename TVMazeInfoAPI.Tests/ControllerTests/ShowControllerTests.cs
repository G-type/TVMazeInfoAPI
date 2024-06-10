using Xunit;
using Moq;
using TVMazeInfoAPI.Presentation.Controllers;
using TVMazeInfoAPI.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TVMazeInfoAPI.Domain.Interfaces;
using TVMazeInfoAPI.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TVMazeInfoAPI.Tests.ControllerTests
{
    public class ShowsControllerTests
    {
        [Fact]
        public async Task GetAllShows_ReturnsOkResult_WithListOfShows()
        {
            // Arrange
            var mockShowService = new Mock<IShowService>();
            var apiKey = "225F7A63BD6942F584E846F440BAF0A704387A39AAF474A40EF3CBD3C0BE761B";
            mockShowService.Setup(service => service.GetAllShowsAsync())
                .ReturnsAsync(GetTestShows());

            var mockLogger = new Mock<ILogger<ShowsController>>();
            var mockHostApplicationLifetime = new Mock<IHostApplicationLifetime>();
            var mockConfiguration = new Mock<IConfiguration>();

            var controller = new ShowsController(
                mockShowService.Object,
                mockHostApplicationLifetime.Object,
                mockLogger.Object,
                mockConfiguration.Object,
                new JwtTokenService(mockConfiguration.Object));

            // Act
            var result = await controller.GetAllShows(apiKey);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Show>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetShowById_ReturnsOkResult_WithShow()
        {
            // Arrange
            var showId = 1;
            var apiKey = "225F7A63BD6942F584E846F440BAF0A704387A39AAF474A40EF3CBD3C0BE761B";
            var mockShowService = new Mock<IShowService>();
            mockShowService.Setup(service => service.GetShowByIdAsync(showId))
                .ReturnsAsync(new Show { Id = showId, Name = "Show1" });

            var mockLogger = new Mock<ILogger<ShowsController>>();
            var mockHostApplicationLifetime = new Mock<IHostApplicationLifetime>();
            var mockConfiguration = new Mock<IConfiguration>();

            var controller = new ShowsController(
                mockShowService.Object,
                mockHostApplicationLifetime.Object,
                mockLogger.Object,
                mockConfiguration.Object,
                new JwtTokenService(mockConfiguration.Object));

            // Act
            var result = await controller.GetShowById(showId, apiKey);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedShow = Assert.IsType<Show>(okResult.Value);
            Assert.Equal(showId, returnedShow.Id);
        }

        [Fact]
        public async Task GetShowById_ReturnsNotFound_WhenShowDoesNotExist()
        {
            // Arrange
            var showId = 1;
            var apiKey = "225F7A63BD6942F584E846F440BAF0A704387A39AAF474A40EF3CBD3C0BE761B";
            var mockShowService = new Mock<IShowService>();
            mockShowService.Setup(service => service.GetShowByIdAsync(showId))
                .ReturnsAsync((Show)null);

            var mockLogger = new Mock<ILogger<ShowsController>>();
            var mockHostApplicationLifetime = new Mock<IHostApplicationLifetime>();
            var mockConfiguration = new Mock<IConfiguration>();

            var controller = new ShowsController(
                mockShowService.Object,
                mockHostApplicationLifetime.Object,
                mockLogger.Object,
                mockConfiguration.Object,
                new JwtTokenService(mockConfiguration.Object));

            // Act
            var result = await controller.GetShowById(showId, apiKey);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task SyncShows_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var apiKey = "225F7A63BD6942F584E846F440BAF0A704387A39AAF474A40EF3CBD3C0BE761B";
            var mockShowService = new Mock<IShowService>();
            mockShowService.Setup(service => service.SyncShowsAsync(It.IsAny<DateTime?>()))
                .Returns(Task.CompletedTask);

            var mockLogger = new Mock<ILogger<ShowsController>>();
            var mockHostApplicationLifetime = new Mock<IHostApplicationLifetime>();

            var mockConfiguration = new Mock<IConfiguration>();

            var controller = new ShowsController(
                mockShowService.Object,
                mockHostApplicationLifetime.Object,
                mockLogger.Object,
                mockConfiguration.Object,
                new JwtTokenService(mockConfiguration.Object));

            // Act
            var result = await controller.SyncShows(apiKey);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task SyncShows_ReturnsUnauthorized_WhenApiKeyInvalid()
        {
            // Arrange
            var apiKey = "225F7A63BD6942F584E846F440BAF0A704387A39AAF474A40EF3CBD3C0BE761B";
            var mockShowService = new Mock<IShowService>();
            var mockLogger = new Mock<ILogger<ShowsController>>();
            var mockHostApplicationLifetime = new Mock<IHostApplicationLifetime>();

            var mockConfiguration = new Mock<IConfiguration>();

            var controller = new ShowsController(
                mockShowService.Object,
                mockHostApplicationLifetime.Object,
                mockLogger.Object,
                mockConfiguration.Object,
                new JwtTokenService(mockConfiguration.Object));

            // Act
            var result = await controller.SyncShows(apiKey);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task SyncShows_ReturnsStatusCode500_WhenErrorOccurs()
        {
            // Arrange
            var apiKey = "225F7A63BD6942F584E846F440BAF0A704387A39AAF474A40EF3CBD3C0BE761B";
            var mockShowService = new Mock<IShowService>();
            mockShowService.Setup(service => service.SyncShowsAsync(It.IsAny<DateTime?>()))
                .Throws(new Exception("Error de sincronización"));

            var mockLogger = new Mock<ILogger<ShowsController>>();
            var mockHostApplicationLifetime = new Mock<IHostApplicationLifetime>();

            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(config => config["ApiKey"])
                .Returns("TuClaveDeApi"); // Reemplaza con tu clave real

            var controller = new ShowsController(
                mockShowService.Object,
                mockHostApplicationLifetime.Object,
                mockLogger.Object,
                mockConfiguration.Object,
                new JwtTokenService(mockConfiguration.Object));

            // Act
            var result = await controller.SyncShows(apiKey);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        private List<Show> GetTestShows()
        {
            return new List<Show>
            {
                new Show { Id = 1, Name = "Show1" },
                new Show { Id = 2, Name = "Show2" }
            };
        }
    }
}