using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TVMazeInfoAPI.Domain.Entities;
using TVMazeInfoAPI.Domain.Interfaces;
using TVMazeInfoAPI.Application.Services;
using Xunit;

namespace TVMazeInfoAPI.Tests.ServiceTests
{
    public class ShowServiceTests
    {
        [Fact]
        public async Task GetAllShowsAsync_CallsShowRepository_GetAllShowsAsync()
        {
            // Arrange
            var mockShowRepository = new Mock<IShowRepository>();
            var mockShows = new List<Show> { new Show { Id = 1, Name = "Show 1" } };
            mockShowRepository.Setup(repo => repo.GetAllShowsAsync())
                .ReturnsAsync(mockShows);

            var showService = new ShowService(mockShowRepository.Object);

            // Act
            var result = await showService.GetAllShowsAsync();

            // Assert
            Assert.Equal(mockShows, result);
            mockShowRepository.Verify(repo => repo.GetAllShowsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetShowByIdAsync_CallsShowRepository_GetShowByIdAsync()
        {
            // Arrange
            var showId = 1;
            var mockShowRepository = new Mock<IShowRepository>();
            var mockShow = new Show { Id = showId, Name = "Show 1" };
            mockShowRepository.Setup(repo => repo.GetShowByIdAsync(showId))
                .ReturnsAsync(mockShow);

            var showService = new ShowService(mockShowRepository.Object);

            // Act
            var result = await showService.GetShowByIdAsync(showId);

            // Assert
            Assert.Equal(mockShow, result);
            mockShowRepository.Verify(repo => repo.GetShowByIdAsync(showId), Times.Once);
        }

        [Fact]
        public async Task SyncShowsAsync_CallsShowRepository_SyncShowsAsync()
        {
            // Arrange
            var mockShowRepository = new Mock<IShowRepository>();
            var sinceDate = DateTime.Now;

            var showService = new ShowService(mockShowRepository.Object);

            // Act
            await showService.SyncShowsAsync(sinceDate);

            // Assert
            mockShowRepository.Verify(repo => repo.SyncShowsAsync(sinceDate), Times.Once);
        }

        [Fact]
        public async Task GetShowsFromTVMazeAsync_CallsShowRepository_GetShowsFromTVMazeAsync()
        {
            // Arrange
            var page = 1;
            var mockShowRepository = new Mock<IShowRepository>();
            var mockShows = new List<Show> { new Show { Id = 1, Name = "Show 1" } };
            mockShowRepository.Setup(repo => repo.GetShowsFromTVMazeAsync(page))
                .ReturnsAsync(mockShows);

            var showService = new ShowService(mockShowRepository.Object);

            // Act
            var result = await showService.GetShowsFromTVMazeAsync(page);

            // Assert
            Assert.Equal(mockShows, result);
            mockShowRepository.Verify(repo => repo.GetShowsFromTVMazeAsync(page), Times.Once);
        }

        [Fact]
        public async Task AddShowAsync_CallsShowRepository_AddShowAsync()
        {
            // Arrange
            var show = new Show { Id = 1, Name = "Show 1" };
            var mockShowRepository = new Mock<IShowRepository>();
            mockShowRepository.Setup(repo => repo.AddShowAsync(show))
                .Returns(Task.CompletedTask);

            var showService = new ShowService(mockShowRepository.Object);

            // Act
            await showService.AddShowAsync(show);

            // Assert
            mockShowRepository.Verify(repo => repo.AddShowAsync(show), Times.Once);
        }
    }
}