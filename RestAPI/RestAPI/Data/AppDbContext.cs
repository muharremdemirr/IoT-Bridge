using Microsoft.EntityFrameworkCore;
using RestAPI.Models;

namespace RestAPI.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions <AppDbContext> options) : base(options)
        {
        }

       public DbSet <SensorData> SensorData { get; set; }
    }
}
