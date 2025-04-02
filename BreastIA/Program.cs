using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using BreastIA.Data;
using BreastIA.Repositories;
using BreastIA.Services;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexi�n desde la variable de entorno o appsettings.json
var connectionString = Environment.GetEnvironmentVariable("Database_Connection_Services")
                       ?? builder.Configuration.GetConnectionString("DatabaseConnection");

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("Error: No se pudo obtener la cadena de conexi�n. Revisa la variable de entorno en Azure.");
    return;
}

// Configurar la conexi�n a la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Registrar los servicios necesarios
builder.Services.AddScoped<IUserRepository, UserRepository>(); // Registro del repositorio de usuarios
builder.Services.AddScoped<AuthService>(); // Registro del servicio de autenticaci�n

// Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Habilitar archivos est�ticos y buscar index.html autom�ticamente
if (!app.Environment.IsDevelopment())
{
    app.UseDefaultFiles(); // Habilita la b�squeda de index.html
}
app.UseStaticFiles();

// Redirigir a index.html si se accede a "/"
app.MapGet("/", async context =>
{
    context.Response.Redirect("/index.html");
});

// Configuraci�n del pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();  // Para que los controladores de la API est�n disponibles

app.Run();
