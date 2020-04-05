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
        private CommandHandler commandHandler;
        
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();
            
            commandHandler = new CommandHandler(_services, _client, _commands);
            
            _client.Log += Log;

            await commandHandler.InstallCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, commandHandler.ClientToken.BotToken);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}