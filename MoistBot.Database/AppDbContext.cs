using Microsoft.EntityFrameworkCore;
using MoistBot.Models;
using MoistBot.Models.Twitch;

namespace MoistBot.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TwitchUser> TwitchMetadata { get; set; }
        public DbSet<TwitchSubscription> TwitchSubscriptions { get; set; }
    }
}