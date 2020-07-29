using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public Task<int> TrySaveFollow(string twitchUserId)
        {
            var meta = GetOrAddMeta(twitchUserId);

            meta.Followed = true;

            return _ctx.SaveChangesAsync();
        }

        public Task<int> SaveSubscription(TwitchSubscription sub)
        {
            var meta = GetOrAddMeta(sub.UserId);
            meta.Subscriptions.Add(sub);
            return _ctx.SaveChangesAsync();
        }

        private TwitchUser GetOrAddMeta(string twitchUserId)
        {
            var twitchMetadata = _ctx.TwitchMetadata.FirstOrDefault(x => x.Id == twitchUserId);

            return twitchMetadata ?? new TwitchUser();
        }
    }
}