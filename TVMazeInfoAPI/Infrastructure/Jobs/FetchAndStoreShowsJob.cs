using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using TVMazeInfoAPI.Application.Services;
using TVMazeInfoAPI.Data;
using TVMazeInfoAPI.Domain.Entities;
using TVMazeInfoAPI.Domain.Interfaces;

namespace TVMazeInfoAPI.Infrastructure.Jobs
{
    public class FetchAndStoreShowsJob : BackgroundService
    {
        private readonly IShowService _showService;
        private readonly ILogger<FetchAndStoreShowsJob> _logger;
        private readonly TVMazeContext _context;
        private readonly IShowRepository _showRepository;
        private readonly IConfiguration _configuration;
        private bool syncCompleted = false;

        public FetchAndStoreShowsJob(IShowService showService,
                                    ILogger<FetchAndStoreShowsJob> logger,
                                    TVMazeContext context,
                                    IShowRepository showRepository)
        {
            _showService = showService;
            _logger = logger;
            _context = context;
            _showRepository = showRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested && !syncCompleted)
            {
                try
                {
                    _logger.LogInformation("Comenzando sincronización de shows desde TVMaze...");

                    int maxPages = 1;
                    var shows = new List<Show>();

                    for (int currentPage = 0; currentPage < maxPages; currentPage++)
                    {
                        var pageShows = await _showService.GetShowsFromTVMazeAsync(currentPage);
                        shows.AddRange(pageShows);
                    }

                    foreach (var show in shows)
                    {
                        var existingShow = await _context.Shows.FindAsync(show.Id);

                        if (existingShow != null)
                        {
                            _context.Entry(existingShow).CurrentValues.SetValues(show);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            await _showService.AddShowAsync(show);
                        }
                    }

                    syncCompleted = true; 
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error durante la sincronización de shows desde TVMaze.");

                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }

                if (syncCompleted)
                {
                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
                }
            }
        }
    }
}