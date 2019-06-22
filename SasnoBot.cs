using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SasnoBot.Database;
using SasnoBot.Services;
using SasnoBot.Services.Interfaces;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SasnoBot
{
    class SasnoBot : IDisposable
    {
        private DiscordSocketClient _client;
        private IConfigurationRoot _configuration;
        private CommandService _commands;
        private IServiceProvider _services;
        private string[] _serviceArgs;

        public SasnoBot(string[] args = null)
        {
            _serviceArgs = args;
        }

        public async Task Configure()
        {
            _client = new DiscordSocketClient();

            var configBuilder = new ConfigurationBuilder()
                .AddCommandLine(_serviceArgs);
            _configuration = configBuilder.Build();

            _commands = new CommandService();

            RoomLifetimeService roomLifetimeService = new RoomLifetimeService(_client);
            roomLifetimeService.Initialize();

            _services = new ServiceCollection()
                .AddDbContext<SasnoBotDbContext>(ServiceLifetime.Scoped)
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton(roomLifetimeService)
                .AddScoped<IUserManager, UserManager>()
                .BuildServiceProvider();

            await InstallCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, _configuration["token"]);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            int argPos = 0;
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;

            var context = new SocketCommandContext(_client, message);
            try
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services, MultiMatchHandling.Exception);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    await context.Channel.SendMessageAsync(result.ErrorReason);
            }
            catch(Exception e)
            {
                await context.Channel.SendMessageAsync("Exception!");
                await context.Channel.SendMessageAsync($"`{e.GetType().FullName} - {e.Message}`");
            }
        }

        public void Dispose()
        {
            _client.LogoutAsync().GetAwaiter().GetResult();
        }
    }
}
