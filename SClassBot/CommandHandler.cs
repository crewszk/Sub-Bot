using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace SClassBot
{
    public class CommandHandler
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        public Token ClientToken { get; }

        public CommandHandler(IServiceProvider services, DiscordSocketClient client, CommandService commands)
        {
            var keyPath = Path.Combine(Environment.CurrentDirectory, @"Resources\", "config.json");
            var keyString = File.ReadAllText(keyPath);
            ClientToken = JsonConvert.DeserializeObject<Token>(keyString);
            
            _commands = commands;
            _client = client;
            _services = services;
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: _services);
        }
        
        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            if (!(messageParam is SocketUserMessage message)) return;
            var context = new SocketCommandContext(_client, message);

            var argPos = 0;
            if (!(message.HasStringPrefix(ClientToken.StandardPrefix, ref argPos) ||
                  message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
            {
                argPos = 0;
                if (!(message.HasStringPrefix(ClientToken.ModPrefix, ref argPos)))
                    return;
            }
            
            await _commands.ExecuteAsync(
                context: context,
                argPos: 0,
                services: _services);
        }
    }
}