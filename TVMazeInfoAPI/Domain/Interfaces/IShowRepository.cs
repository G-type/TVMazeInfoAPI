using TVMazeInfoAPI.Domain.Entities;

namespace TVMazeInfoAPI.Domain.Interfaces
{
    public interface IShowRepository
    {
        Task<IEnumerable<Show>> GetAllShowsAsync();
        Task<Show> GetShowByIdAsync(int id);
        Task SyncShowsAsync(DateTime? since);
        Task AddShowAsync(Show show);
        Task<IEnumerable<Show>> GetShowsFromTVMazeAsync(int page);
    }

}
