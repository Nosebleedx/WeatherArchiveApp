using Microsoft.EntityFrameworkCore;

namespace WeatherArchiveApp.Data
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DbConnectionString"));

        }

        public DbSet<WeatherData> WeatherData { get; set; }
    }
}
