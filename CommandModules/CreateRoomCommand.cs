using Discord.Commands;
using SasnoBot.Services;
using System.Linq;
using System.Threading.Tasks;

namespace SasnoBot.CommandModules
{
    public class RoomModule : ModuleBase<SocketCommandContext>
    {
        private RoomLifetimeService _roomLifetimeService;

        public RoomModule(RoomLifetimeService roomLifetimeService)
        {
            _roomLifetimeService = roomLifetimeService;
        }

        [Command("createRoom")]
        [Summary("Creates temporary room")]
        [Alias("cr", "create")]
        public async Task CrateRoomAsync([Remainder][Summary("The name of the room")] string roomName)
        {
            var guild = Context.Guild;

            if (guild.Channels.Any(ch => ch.Name == roomName))
            {
                await ReplyAsync($"Channel with name {roomName} already exists!");
                return;
            }

            var voiceChannel = await guild.CreateVoiceChannelAsync(roomName);
            _roomLifetimeService.AddChannelToMonitor(voiceChannel.Id);
        }
    }
}
