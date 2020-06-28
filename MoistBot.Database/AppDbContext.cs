using Microsoft.EntityFrameworkCore;
using MoistBot.Models.Twitch;

namespace MoistBot.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserFollow> UserFollows { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }

    }
}