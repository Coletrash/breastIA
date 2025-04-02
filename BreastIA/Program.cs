using Microsoft.EntityFrameworkCore;
using TuProyecto.Data;



var builder = WebApplication.CreateBuilder(args);

// Configura la conexión a la base de datos
var connectionString = builder.Configuration.GetConnectionString("Database_Connection_Services");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Agrega servicios al contenedor.
builder.Services.AddControllers();

// Configura Swagger/OpenAPI (opcional)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configura la canalización de solicitudes HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
