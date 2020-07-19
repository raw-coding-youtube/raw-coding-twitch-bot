using System.Linq;
using System.Threading.Tasks;
using MoistBot.Database;
using MoistBot.Models.Twitch;

namespace MoistBot.Services
{
    public class RegisterUserAction
    {
        private readonly AppDbContext _ctx;

        public RegisterUserAction(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public Task<int> TrySaveFollow(string userId)
        {
            var alreadyFollowed = _ctx.UserFollows.Any(x => x.TwitchUserId.Equals(userId));
            if (alreadyFollowed)
            {
                return Task.FromResult(0);
            }

            _ctx.UserFollows.Add(new UserFollow
            {
                TwitchUserId = userId,
            });
            return _ctx.SaveChangesAsync();
        }

        public Task<int> SaveSubscriber(UserSubscription subscription)
        {
            _ctx.UserSubscriptions.Add(subscription);
            return _ctx.SaveChangesAsync();
        }
    }
}