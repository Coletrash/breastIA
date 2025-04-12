using Microsoft.EntityFrameworkCore;
using BreastIA.Models;

namespace BreastIA.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        // Definimos los DbSet para cada tabla en la base de datos
        public DbSet<User> Users { get; set; }
    }
}
