using EDW.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EDW.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
