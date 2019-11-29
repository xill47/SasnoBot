using Discord.Commands;
using SasnoBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SasnoBot.CommandModules.Filters
{
    public class RequireUserAllowedConfigurationAttribute : PreconditionAttribute
    {
        public async override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var userId = context.User.Id;
            var isAdmin = context.Guild.GetUserAsync(userId).Result.GuildPermissions.Administrator;

            if (isAdmin) { return PreconditionResult.FromSuccess(); }

            var userManager = (IUserManager)services.GetService(typeof(IUserManager));
            if (await userManager.IsUserAllowedToDoConfiguration(userId))
            {
                return PreconditionResult.FromError("You are not allowed to configure me");
            }
            else
            {
                return PreconditionResult.FromSuccess();
            }
        }
    }
}