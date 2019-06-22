using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Discord;
using Discord.WebSocket;

namespace SasnoBot.Services
{
    public class RoomLifetimeService
    {
        private DiscordSocketClient client;

        public RoomLifetimeService(DiscordSocketClient client)
        {
            this.client = client;
        }

        public void Initialize()
        {
        }

        public void AddChannelToMonitor(ulong voiceChannelId)
        {
            ChannelWithEmptyTime channel = new ChannelWithEmptyTime(voiceChannelId);
            CheckPeriodically(channel);
        }

        private void CheckPeriodically(ChannelWithEmptyTime channel)
        {
            var timer = new Timer(2000);
            timer.Elapsed += CheckerBuilder(timer, channel);
            timer.Start();
        }

        private ElapsedEventHandler CheckerBuilder(Timer timer, ChannelWithEmptyTime channel)
        {
            var res = new ElapsedEventHandler((sender, args) =>
            {
                var vc = client.GetChannel(channel.VoiceChannelId) as SocketVoiceChannel;
                if (vc == null)
                {
                    timer.Dispose();
                    return;
                }

                if (vc.Users.Any())
                {
                    channel.EmptyTimeSpan = null;
                    return;
                }
                else
                {
                    channel.EmptyTimeSpan = channel.EmptyTimeSpan ?? TimeSpan.Zero;
                    channel.EmptyTimeSpan += TimeSpan.FromMilliseconds(timer.Interval);
                }

                if (channel.EmptyTimeSpan > TimeSpan.FromMinutes(1))
                {
                    vc.DeleteAsync().GetAwaiter().GetResult();
                    timer.Dispose();
                }
            });
            return res;
        }

        class ChannelWithEmptyTime
        {
            public ChannelWithEmptyTime(ulong voiceChannelId, TimeSpan? emptyTimeSpan = null)
            {
                VoiceChannelId = voiceChannelId;
                EmptyTimeSpan = emptyTimeSpan;
            }

            public ulong VoiceChannelId { get; }
            public TimeSpan? EmptyTimeSpan { get; set; }
        }
    }
}
