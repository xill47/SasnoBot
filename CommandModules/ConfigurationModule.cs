using Discord;
using Discord.Commands;
using SasnoBot.CommandModules.Filters;
using SasnoBot.Services.Interfaces;
using System.Threading.Tasks;

namespace SasnoBot.CommandModules
{
    [RequireUserAllowedConfiguration]
    public class ConfigurationModule : ModuleBase<SocketCommandContext>
    {
        private readonly IUserManager userManager;

        protected ConfigurationModule(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            
            base.BeforeExecute(command);
        }

        [Command("verbose")]
        [Summary("Makes output to this channel more verbose")]
        [Alias("v")]
        public async Task MakeOutputVerbose()
        {

        }

        [Command("canuseconfig"), Alias("cuc")]
        public async Task CheckPriveledge(IUser user = null)
        {
            if (user == null)
            {
                await ReplyAsync("You can use config");
            }
            else if (await userManager.IsUserAllowedToDoConfiguration(user.Id))
            {
                await ReplyAsync($"{user.Mention} is allowed to use config");
            } 
            else
            {
                await ReplyAsync("Not allowed to use config");
            }
        }

        [Command("addtoconfig"), Alias("atc")]
        public async Task AddConfigPriveledge(IUser user)
        {
            await userManager.SetUserAllowedToDoConfiguration(user.Id, true);
        }

        [Command("removefromconfig"), Alias("rfc")]
        public async Task RemoveFromConfig(IUser user)
        {
            await userManager.SetUserAllowedToDoConfiguration(user.Id, false);
        }
    }
}
