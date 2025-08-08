using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Data
{
    public class AppDbContext : DbContext
    {
        public IConfiguration _configuration { get; set; }
        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DatabaseConnection"));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<About> Abouts { get; set; }
    }
}
