using Microsoft.EntityFrameworkCore;
using FootballPrediction.Models;

namespace FootballPrediction.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Match> Matches { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=..\\..\\..\\football.db");
        }
    }
}
