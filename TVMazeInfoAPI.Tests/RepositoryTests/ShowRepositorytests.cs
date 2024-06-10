using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using TVMazeInfoAPI.Data;
using TVMazeInfoAPI.Domain.Entities;
using TVMazeInfoAPI.Domain.Interfaces;
using TVMazeInfoAPI.Infrastructure.Data;
using Xunit;

namespace TVMazeInfoAPI.Tests.RepositoryTests
{
    public class ShowRepositoryTests
    {
        [Fact]
        public async Task GetAllShowsAsync_ReturnsAllShowsFromDbContext()
        {
            // Arrange
            var show1 = new Show { Id = 1, Name = "Show 1" };
            var show2 = new Show { Id = 2, Name = "Show 2" };
            var shows = new List<Show> { show1, show2 };

            var mockContext = new Mock<TVMazeContext>();
            mockContext.Setup(ctx => ctx.Shows)
                .Returns(DbSetMock(shows.AsQueryable()));

            var mockClientFactory = new Mock<IHttpClientFactory>();
            var mockShowService = new Mock<IShowService>();

            var repository = new ShowRepository(mockContext.Object, mockClientFactory.Object);

            // Act
            var result = await repository.GetAllShowsAsync();

            // Assert
            Assert.Equal(shows, result);
        }

        [Fact]
        public async Task GetShowByIdAsync_ReturnsCorrectShowFromDbContext()
        {
            // Arrange
            var showId = 1;
            var show = new Show { Id = showId, Name = "Show 1" };

            var mockContext = new Mock<TVMazeContext>();
            mockContext.Setup(ctx => ctx.Shows.FindAsync(showId))
                .ReturnsAsync(show);

            var mockClientFactory = new Mock<IHttpClientFactory>();
            var mockShowService = new Mock<IShowService>();

            var repository = new ShowRepository(mockContext.Object, mockClientFactory.Object);

            // Act
            var result = await repository.GetShowByIdAsync(showId);

            // Assert
            Assert.Equal(show, result);
        }

        [Fact]
        public async Task GetShowByIdAsync_ReturnsNull_WhenShowNotFound()
        {
            // Arrange
            var showId = 1;

            var mockContext = new Mock<TVMazeContext>();
            mockContext.Setup(ctx => ctx.Shows.FindAsync(showId))
                .ReturnsAsync((Show)null);

            var mockClientFactory = new Mock<IHttpClientFactory>();
            var mockShowService = new Mock<IShowService>();

            var repository = new ShowRepository(mockContext.Object, mockClientFactory.Object);

            // Act
            var result = await repository.GetShowByIdAsync(showId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddShowAsync_AddsShowToDbContext()
        {
            // Arrange
            var show = new Show { Id = 1, Name = "Show 1" };
            var mockContext = new Mock<TVMazeContext>();
            mockContext.Setup(ctx => ctx.Shows.Add(It.IsAny<Show>()))
                .Returns(It.IsAny<EntityEntry<Show>>());
            mockContext.Setup(ctx => ctx.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1); 

            var mockClientFactory = new Mock<IHttpClientFactory>();
            var mockShowService = new Mock<IShowService>();

            var repository = new ShowRepository(mockContext.Object, mockClientFactory.Object);

            // Act
            await repository.AddShowAsync(show);

            // Assert
            mockContext.Verify(ctx => ctx.Shows.Add(show), Times.Once);
            mockContext.Verify(ctx => ctx.SaveChangesAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task UpdateShowAsync_UpdatesShowInDbContext()
        {
            // Arrange
            var show = new Show { Id = 1, Name = "Show 1" };
            var mockContext = new Mock<TVMazeContext>();
            mockContext.Setup(ctx => ctx.Shows.FindAsync(show.Id))
                .ReturnsAsync(show);
            mockContext.Setup(ctx => ctx.SaveChangesAsync(CancellationToken.None))
                .ReturnsAsync(1); 

            var mockClientFactory = new Mock<IHttpClientFactory>();
            var mockShowService = new Mock<IShowService>();

            var repository = new ShowRepository(mockContext.Object, mockClientFactory.Object);

            // Act
            await repository.UpdateShowAsync(show);

            // Assert
            mockContext.Verify(ctx => ctx.SaveChangesAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GetShowsFromTVMazeAsync_CallsHttpClientGetAsync_WithCorrectUrl()
        {
            // Arrange
            var page = 1;
            var expectedUrl = $"https://api.tvmaze.com/shows?page={page}";
            var mockHttpClient = new Mock<HttpClient>();
            var mockHttpResponse = new Mock<HttpResponseMessage>();
            mockHttpResponse.SetupGet(r => r.IsSuccessStatusCode).Returns(true);
            mockHttpClient.Setup(c => c.GetAsync(expectedUrl))
                .ReturnsAsync(mockHttpResponse.Object);

            var mockClientFactory = new Mock<IHttpClientFactory>();
            mockClientFactory.Setup(cf => cf.CreateClient())
                .Returns(mockHttpClient.Object);

            var mockShowService = new Mock<IShowService>();

            var repository = new ShowRepository(null, mockClientFactory.Object);

            // Act
            await repository.GetShowsFromTVMazeAsync(page);

            // Assert
            mockHttpClient.Verify(c => c.GetAsync(expectedUrl), Times.Once);
        }

        [Fact]
        public async Task GetShowsFromTVMazeAsync_ReturnsListOfShows_WhenHttpClientReturnsSuccess()
        {
            // Arrange
            var page = 1;
            var mockHttpClient = new Mock<HttpClient>();
            var mockHttpResponse = new Mock<HttpResponseMessage>();
            mockHttpResponse.SetupGet(r => r.IsSuccessStatusCode).Returns(true);
            var showsJson = JsonSerializer.Serialize(new List<Show> { new Show { Id = 1, Name = "Show 1" } });
            mockHttpResponse.Setup(r => r.Content.ReadAsStringAsync())
                .ReturnsAsync(showsJson);
            mockHttpClient.Setup(c => c.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(mockHttpResponse.Object);

            var mockClientFactory = new Mock<IHttpClientFactory>();
            mockClientFactory.Setup(cf => cf.CreateClient())
                .Returns(mockHttpClient.Object);

            var mockShowService = new Mock<IShowService>();

            var repository = new ShowRepository(null, mockClientFactory.Object);

            // Act
            var result = await repository.GetShowsFromTVMazeAsync(page);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(1, result.Count());
        }

        [Fact]
        public async Task GetShowsFromTVMazeAsync_ThrowsException_WhenHttpClientReturnsError()
        {
            // Arrange
            var page = 1;
            var mockHttpClient = new Mock<HttpClient>();
            var mockHttpResponse = new Mock<HttpResponseMessage>();
            mockHttpResponse.SetupGet(r => r.IsSuccessStatusCode).Returns(false);
            mockHttpResponse.SetupGet(r => r.StatusCode).Returns(System.Net.HttpStatusCode.BadRequest);

            mockHttpClient.Setup(c => c.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(mockHttpResponse.Object);

            var mockClientFactory = new Mock<IHttpClientFactory>();
            mockClientFactory.Setup(cf => cf.CreateClient())
                .Returns(mockHttpClient.Object);

            var mockShowService = new Mock<IShowService>();

            var repository = new ShowRepository(null, mockClientFactory.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await repository.GetShowsFromTVMazeAsync(page));
        }

        [Fact]
        public async Task SyncAllShowsAsync_CallsGetShowsFromTVMazeAsync_WithPagination()
        {
            // Arrange
            var mockContext = new Mock<TVMazeContext>();
            var mockClientFactory = new Mock<IHttpClientFactory>();
            var mockShowService = new Mock<IShowService>();
            var mockHttpClient = new Mock<HttpClient>();
            var mockHttpResponse = new Mock<HttpResponseMessage>();
            mockHttpResponse.SetupGet(r => r.IsSuccessStatusCode).Returns(true);
            mockHttpClient.Setup(c => c.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(mockHttpResponse.Object);

            mockClientFactory.Setup(cf => cf.CreateClient())
                .Returns(mockHttpClient.Object);

            var repository = new ShowRepository(mockContext.Object, mockClientFactory.Object);

            // Act
            await repository.SyncAllShowsAsync();

            // Assert
            mockHttpClient.Verify(c => c.GetAsync(It.Is<string>(url => url.Contains("page=1"))), Times.AtLeastOnce);
            mockHttpClient.Verify(c => c.GetAsync(It.Is<string>(url => url.Contains("page=2"))), Times.AtLeastOnce);
        }

        [Fact]
        public async Task SyncAllShowsAsync_AddsNewShowsToDbContext()
        {
            // Arrange
            var show1 = new Show { Id = 1, Name = "Show 1" };
            var show2 = new Show { Id = 2, Name = "Show 2" };
            var shows = new List<Show> { show1, show2 };

            var mockContext = new Mock<TVMazeContext>();
            mockContext.Setup(ctx => ctx.Shows.FindAsync(It.IsAny<int>()))
                .ReturnsAsync((Show)null); 
            mockContext.Setup(ctx => ctx.SaveChangesAsync(CancellationToken.None))
                .ReturnsAsync(1); 

            var mockClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpClient = new Mock<HttpClient>();
            var mockHttpResponse = new Mock<HttpResponseMessage>();
            mockHttpResponse.SetupGet(r => r.IsSuccessStatusCode).Returns(true);
            var showsJson = JsonSerializer.Serialize(shows);
            mockHttpResponse.Setup(r => r.Content.ReadAsStringAsync())
                .ReturnsAsync(showsJson);
            mockHttpClient.Setup(c => c.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(mockHttpResponse.Object);

            mockClientFactory.Setup(cf => cf.CreateClient())
                .Returns(mockHttpClient.Object);

            var mockShowService = new Mock<IShowService>();
            mockShowService.Setup(service => service.AddShowAsync(It.IsAny<Show>()))
                .Returns(Task.CompletedTask);

            var repository = new ShowRepository(mockContext.Object, mockClientFactory.Object);

            // Act
            await repository.SyncAllShowsAsync();

            // Assert
            mockContext.Verify(ctx => ctx.Shows.Add(It.IsAny<Show>()), Times.Exactly(2));
            mockShowService.Verify(service => service.AddShowAsync(It.IsAny<Show>()), Times.Exactly(2));
        }

        [Fact]
        public async Task SyncAllShowsAsync_UpdatesExistingShowsInDbContext()
        {
            // Arrange
            var show1 = new Show { Id = 1, Name = "Show 1" };
            var show2 = new Show { Id = 2, Name = "Show 2" };
            var shows = new List<Show> { show1, show2 };

            var mockContext = new Mock<TVMazeContext>();
            mockContext.Setup(ctx => ctx.Shows.FindAsync(It.IsAny<int>()))
                .ReturnsAsync((Show)null);
            mockContext.Setup(ctx => ctx.SaveChangesAsync(CancellationToken.None))
                .ReturnsAsync(1);

            var mockClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpClient = new Mock<HttpClient>();
            var mockHttpResponse = new Mock<HttpResponseMessage>();
            mockHttpResponse.SetupGet(r => r.IsSuccessStatusCode).Returns(true);
            var showsJson = JsonSerializer.Serialize(shows);
            mockHttpResponse.Setup(r => r.Content.ReadAsStringAsync())
                .ReturnsAsync(showsJson);
            mockHttpClient.Setup(c => c.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(mockHttpResponse.Object);

            mockClientFactory.Setup(cf => cf.CreateClient())
                .Returns(mockHttpClient.Object);

            var mockShowService = new Mock<IShowService>();
            mockShowService.Setup(service => service.AddShowAsync(It.IsAny<Show>()))
                .Returns(Task.CompletedTask);

            var repository = new ShowRepository(mockContext.Object, mockClientFactory.Object);

            // Act
            await repository.SyncAllShowsAsync();

            // Assert
            mockContext.Verify(ctx => ctx.Shows.Add(It.IsAny<Show>()), Times.Exactly(2));
            mockShowService.Verify(service => service.AddShowAsync(It.IsAny<Show>()), Times.Exactly(2));
        }


        [Fact]
        public async Task SyncIncrementalShowsAsync_CallsGetShowsFromTVMazeAsync_WithUpdatedShowIds()
        {
            // Arrange
            var sinceDate = DateTime.Now.AddDays(-1); 
            var updatedShowIds = new List<int> { 1, 2, 3 };

            var mockContext = new Mock<TVMazeContext>();
            mockContext.Setup(ctx => ctx.Shows.FindAsync(It.IsAny<int>()))
                .ReturnsAsync((Show)null); 
            mockContext.Setup(ctx => ctx.SaveChangesAsync(CancellationToken.None))
                .ReturnsAsync(1);

            var mockClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpClient = new Mock<HttpClient>();
            var mockHttpResponse = new Mock<HttpResponseMessage>();
            mockHttpResponse.SetupGet(r => r.IsSuccessStatusCode).Returns(true);
            mockHttpResponse.Setup(r => r.Content.ReadAsStringAsync())
                .ReturnsAsync(JsonSerializer.Serialize(updatedShowIds));
            mockHttpClient.Setup(c => c.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(mockHttpResponse.Object);

            mockClientFactory.Setup(cf => cf.CreateClient())
                .Returns(mockHttpClient.Object);

            var mockShowService = new Mock<IShowService>();
            mockShowService.Setup(service => service.AddShowAsync(It.IsAny<Show>()))
                .Returns(Task.CompletedTask);

            var repository = new ShowRepository(mockContext.Object, mockClientFactory.Object);

            // Act
            await repository.SyncIncrementalShowsAsync(sinceDate);

            // Assert
            mockHttpClient.Verify(c => c.GetAsync($"https://api.tvmaze.com/updates/shows?since={sinceDate.ToString("yyyy-MM-dd")}"), Times.Once);
            mockContext.Verify(ctx => ctx.SaveChangesAsync(CancellationToken.None), Times.Exactly(updatedShowIds.Count));
        }

        

        private DbSet<T> DbSetMock<T>(IQueryable<T> data) where T : class
        {
            var queryable = new Mock<DbSet<T>>();
            queryable.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            queryable.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            queryable.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            queryable.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return queryable.Object;
        }

    }
}