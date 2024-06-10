using TVMazeInfoAPI.Domain.Entities;

namespace TVMazeInfoAPI.Domain.Interfaces
{
    public interface IShowService
    {
        Task<IEnumerable<Show>> GetAllShowsAsync();
        Task<Show> GetShowByIdAsync(int id);
        Task SyncShowsAsync(DateTime? since);
        Task<IEnumerable<Show>> GetShowsFromTVMazeAsync(int page);
        Task AddShowAsync(Show show);
    }
}
