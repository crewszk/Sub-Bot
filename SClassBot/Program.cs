using System;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SClassBot.Modules;

namespace SClassBot
{
    internal class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private static Token token;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            var keyPath = Path.Combine(Environment.CurrentDirectory, @"Resources\", "config.json");
            var keyString = File.ReadAllText(keyPath);
            token = JsonConvert.DeserializeObject<Token>(keyString);
            
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();
            
            _client.Log += Log;

            await InstallCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, token.BotToken);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
        }
        
        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            if (!(messageParam is SocketUserMessage message)) return;
            var context = new SocketCommandContext(_client, message);

            var argPos = 0;
            if (!(message.HasStringPrefix(token.StandardPrefix, ref argPos) ||
                  message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
            {
                argPos = 0;
                if (!(message.HasStringPrefix(token.ModPrefix, ref argPos)))
                    return;
            }
            
            await _commands.ExecuteAsync(
                context: context,
                argPos: 0,
                services: null);
        }
    }
}