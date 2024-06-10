using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var apiKey = context.Request.Query["apiKey"].FirstOrDefault();
        var validApiKey = _configuration.GetValue<string>("SecretKey");

        if (string.IsNullOrEmpty(apiKey) || apiKey != validApiKey)
        {
            context.Response.StatusCode = 401;
            return;
        }

        await _next(context);
    }
}