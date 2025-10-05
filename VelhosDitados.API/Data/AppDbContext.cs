using Microsoft.EntityFrameworkCore;
using VelhosDitados.API.Models;

namespace VelhosDitados.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Ditado> Ditados { get; set; }
    }
}
