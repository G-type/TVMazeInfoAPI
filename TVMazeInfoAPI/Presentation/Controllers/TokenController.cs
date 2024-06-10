using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TVMazeInfoAPI.Application.Services;

namespace TVMazeInfoAPI.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService;

        public TokenController(JwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult GenerateToken(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                return BadRequest("La clave API es obligatoria");
            }

            var token = _jwtTokenService.GenerateToken(apiKey);

            return Ok(new { token });
        }
    }
}
