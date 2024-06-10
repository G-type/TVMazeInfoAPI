using Microsoft.EntityFrameworkCore;
using TVMazeInfoAPI.Data;
using TVMazeInfoAPI.Infrastructure.Jobs;
using TVMazeInfoAPI.Application.Services;
using TVMazeInfoAPI.Domain.Interfaces;
using TVMazeInfoAPI.Infrastructure.Data;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<FetchAndStoreShowsJob>();
builder.Services.AddTransient<JwtTokenService>();
builder.Services.AddDbContext<TVMazeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TVMazeDatabase")),
    ServiceLifetime.Singleton);

builder.Services.AddTransient<IShowRepository, ShowRepository>();
builder.Services.AddTransient<IShowService, ShowService>();
builder.Services.AddHttpClient();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"])),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();