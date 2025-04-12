using Microsoft.EntityFrameworkCore;
using BreastIA.Models;

namespace BreastIA.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<MammographicTest> MammographicTests { get; set; }

        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<HospitalDoctor> HospitalDoctor { get; set; }

        public DbSet<Doctor> Doctor { get; set; }
       
    }
}
