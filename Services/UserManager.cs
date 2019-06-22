using Microsoft.EntityFrameworkCore;
using SasnoBot.Database;
using SasnoBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SasnoBot.Services
{
    public class UserManager : IUserManager
    {
        private readonly SasnoBotDbContext context;

        public UserManager(SasnoBotDbContext sasnoBotDbContext)
        {
            this.context = sasnoBotDbContext;
        }

        public async Task<bool> IsUserAllowedToDoConfiguration(ulong userId)
        {
            var query = context.Users
                .Where(user => user.DiscordUserId == userId && user.ConfigurationPriveleges == true);

            var res = await query.ToListAsync();

            return res.Any();
        }

        public async Task SetUserAllowedToDoConfiguration(ulong userId, bool allowed)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.DiscordUserId == userId);
            if (user != null)
            {
                user.ConfigurationPriveleges = allowed;
            }
            else
            {
                context.Add(new User
                {
                    DiscordUserId = userId,
                    ConfigurationPriveleges = allowed
                });
            }
            await context.SaveChangesAsync();
        }
    }
}
