using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using System.Text.Json;
using TVMazeInfoAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using TVMazeInfoAPI.Exceptions;
using TVMazeInfoAPI.Application.Services;

namespace TVMazeInfoAPI.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IShowService _showService;
        private readonly ILogger<ShowsController> _logger;
        private readonly IConfiguration _configuration;
        private readonly JwtTokenService _jwtTokenService;


        public ShowsController(IShowService showService,
                             IHostApplicationLifetime hostApplicationLifetime,
                             ILogger<ShowsController> logger,
                             IConfiguration configuration,
                             JwtTokenService jwtTokenService)
        {
            _showService = showService;
            _hostApplicationLifetime = hostApplicationLifetime;
            _logger = logger;
            _configuration = configuration;
            _jwtTokenService = jwtTokenService;
        }


        [HttpPost("Sync")]
        public async Task<IActionResult> SyncShows(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                return BadRequest("La clave API es obligatoria");
            }

            try
            {
                _logger.LogInformation("Iniciando la sincronización manual de shows...");

                DateTime? lastSyncDate = null;

                await _showService.SyncShowsAsync(lastSyncDate);

                _logger.LogInformation("Sincronización manual de shows completada.");
                Console.WriteLine("Sincronización manual de shows completada.");

                return Ok(new { message = "Sincronización iniciada correctamente." });
            }
            catch (TVMazeApiException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al sincronizar shows.");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllShows(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                return BadRequest("La clave API es obligatoria");
            }

            var shows = await _showService.GetAllShowsAsync();

            // Configura las opciones de serialización para manejar referencias circulares
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
            };

            // Serializa la lista de shows con las opciones configuradas
            return Ok(JsonSerializer.Serialize(shows, options));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShowById(int id, string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                return BadRequest("La clave API es obligatoria");
            }

            try
            {
                var show = await _showService.GetShowByIdAsync(id);

                if (show == null)
                {
                    return NotFound();
                }

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                };

                return Ok(JsonSerializer.Serialize(show, options));
            }
            catch (ShowNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener show por ID.");
                return StatusCode(500, "Error interno del servidor");
            }

        }
    }
}