using TVMazeInfoAPI.Domain.Entities;
using TVMazeInfoAPI.Domain.Interfaces;

namespace TVMazeInfoAPI.Application.Services
{
    public class ShowService : IShowService
    {
        private readonly IShowRepository _showRepository;

        public ShowService(IShowRepository showRepository)
        {
            _showRepository = showRepository;
        }

        public async Task<IEnumerable<Show>> GetAllShowsAsync()
        {
            return await _showRepository.GetAllShowsAsync();
        }

        public async Task<Show> GetShowByIdAsync(int id)
        {
            var show = await _showRepository.GetShowByIdAsync(id);

            if (show == null)
            {
            }

            return show;
        }

        public async Task SyncShowsAsync(DateTime? since)
        {
            await _showRepository.SyncShowsAsync(since);
        }

        public async Task<IEnumerable<Show>> GetShowsFromTVMazeAsync(int page)
        {
            return await _showRepository.GetShowsFromTVMazeAsync(page);
        }

        public async Task AddShowAsync(Show show)
        {
            await _showRepository.AddShowAsync(show);
        }
    }
}