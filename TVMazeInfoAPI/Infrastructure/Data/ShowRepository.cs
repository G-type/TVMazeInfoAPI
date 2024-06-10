using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TVMazeInfoAPI.Data;
using TVMazeInfoAPI.Domain.Entities;
using TVMazeInfoAPI.Domain.Interfaces;
using TVMazeInfoAPI.Exceptions;

namespace TVMazeInfoAPI.Infrastructure.Data
{
    public class ShowRepository : IShowRepository
    {
        private readonly TVMazeContext _context;
        private readonly IHttpClientFactory _clientFactory;

        public ShowRepository(TVMazeContext context,
            IHttpClientFactory clientFactory)
        {
            _context = context;
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<Show>> GetAllShowsAsync()
        {
            return await _context.Shows
                .Include(s => s.Episodes)
                .Include(s => s.Cast)
                .Include(s => s.Crew)
                .Include(s => s.Network)
                    .ThenInclude(n => n.Country)
                .ToListAsync();
        }

        public async Task<Show> GetShowByIdAsync(int id)
        {
            var show = await _context.Shows
                .Include(s => s.Episodes)
                .Include(s => s.Cast)
                .Include(s => s.Crew)
                .Include(s => s.Network)
                    .ThenInclude(n => n.Country)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (show == null)
            {
                throw new ShowNotFoundException(id); // Lanza una excepción personalizada
            }
            else
            {
                return show;
            }

        }

        public async Task AddShowAsync(Show show)
        {
            _context.Shows.Add(show);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateShowAsync(Show show)
        {
            _context.Entry(show).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Show>> GetShowsFromTVMazeAsync(int page)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://api.tvmaze.com/shows?page={page}");

            try
            {
                response.EnsureSuccessStatusCode(); // Lanza excepción si hay error

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<Show>>(content);
            }
            catch (HttpRequestException ex)
            {
                throw new TVMazeApiException(response.StatusCode, $"Error al obtener shows de TVMaze: {response.StatusCode}");
            }
        }

        public async Task SyncShowsAsync(DateTime? since)
        {
            if (since.HasValue)
            {
                await SyncIncrementalShowsAsync(since.Value);
            }
            else
            {
                await SyncAllShowsAsync();
            }
        }

        public async Task SyncAllShowsAsync(int maxPages = 1)
        {
            var shows = new List<Show>();

            for (int currentPage = 0; currentPage < maxPages; currentPage++)
            {
                var pageShows = await GetShowsFromTVMazeAsync(currentPage);
                shows.AddRange(pageShows);
            }

            foreach (var show in shows)
            {
                var existingShow = await _context.Shows.FindAsync(show.Id);

                if (existingShow != null)
                {
                    _context.Entry(existingShow).CurrentValues.SetValues(show);
                }
                else
                {
                    _context.Shows.Add(show);
                }
            }

            await _context.SaveChangesAsync(); 
        }

        public async Task SyncIncrementalShowsAsync(DateTime since)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://api.tvmaze.com/updates/shows?since={since.ToString("yyyy-MM-dd")}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var updatedShowIds = JsonSerializer.Deserialize<List<int>>(content);

                foreach (var showId in updatedShowIds)
                {
                    var showToUpdate = await _context.Shows.FindAsync(showId);

                    if (showToUpdate != null)
                    {
                        var updatedShow = await GetUpdatedShowFromTvMazeAsync(showId);

                        if (updatedShow != null)
                        {
                            _context.Entry(showToUpdate).CurrentValues.SetValues(updatedShow);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
            else
            {
            }
        }

        private async Task<Show> GetUpdatedShowFromTvMazeAsync(int showId)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://api.tvmaze.com/shows/{showId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Show>(content);
            }

            return null;
        }
    }
}