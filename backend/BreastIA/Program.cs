using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using BreastIA.Data;
using BreastIA.Services;
using Microsoft.AspNetCore.Authentication;
using BreastIA.Models.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");

if (string.IsNullOrEmpty(connectionString))
{
    connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DatabaseConnection");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Registrar servicios
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddDistributedMemoryCache(); // Usar memoria como cach� de sesi�n
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10); // Tiempo de inactividad
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Agregar autenticaci�n con Google, Facebook e Instagram
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
})
.AddFacebook(options =>
{
    options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
})
.AddOAuth("Instagram", options =>
{
    options.ClientId = builder.Configuration["Authentication:Instagram:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Instagram:ClientSecret"];
    options.CallbackPath = "/signin-instagram";
    options.AuthorizationEndpoint = "https://api.instagram.com/oauth/authorize";
    options.TokenEndpoint = "https://api.instagram.com/oauth/access_token";
    options.UserInformationEndpoint = "https://graph.instagram.com/me?fields=id,username";
    options.SaveTokens = true;
    options.ClaimActions.MapJsonKey("urn:instagram:id", "id");
    options.ClaimActions.MapJsonKey("urn:instagram:username", "username");
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar el IHttpClientFactory
builder.Services.AddHttpClient();

// Configuraci�n para aumentar el l�mite de tama�o de archivo (50 MB en este caso)
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 52428800; // L�mite de 50 MB
});

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configurar los middlewares
app.UseHttpsRedirection();

// Habilitar autenticaci�n
app.UseAuthentication(); // Habilitar autenticaci�n

// Habilitar autorizaci�n
app.UseAuthorization();

// Habilitar sesi�n
app.UseSession(); // Habilitar sesi�n

app.MapControllers();

app.Run();
