using AndroidServerSide.Models;
using Microsoft.EntityFrameworkCore;

namespace AndroidServerSide.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Models.ServiceProvider> ServiceProviders { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        //public DbSet<Bookmark> Bookmarks { get; set; }
    }
}
