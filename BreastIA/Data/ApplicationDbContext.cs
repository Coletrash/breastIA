using Microsoft.EntityFrameworkCore;

namespace TuProyecto.Data 
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        // Defimor los DbSet para cada tabla en la base de datos
        public DbSet<Usuario> Usuarios { get; set; }
        // Agrega más segun se vea necesario
    }
}
