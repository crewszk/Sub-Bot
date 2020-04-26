using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using SClassBot.Modules;

namespace SClassBot
{
    public class CommandHandler
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        public static Token ClientToken { get; private set; }

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
            _client.UserJoined += AnnouceJoinedUser;
            _client.UserLeft += AnnouceUserLeft;
            _client.UserBanned += AnnouceUserBanned;
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

        private async Task AnnouceUserBanned(IMentionable user, IGuild guild)
        {
            var goodbyeChannel = _client.GetChannel(ClientToken.GuildChannels.Goodbye) as SocketTextChannel;
            await goodbyeChannel.SendMessageAsync($"{user.Mention} has been banned from {guild.Name} for now! Oh well, probably had it coming.");
        }

        private async Task AnnouceJoinedUser(IMentionable user)
        {
            var welcomeChannel = _client.GetChannel(ClientToken.GuildChannels.Welcome) as SocketTextChannel;
            var rulesChannel = _client.GetChannel(ClientToken.GuildChannels.Rules) as SocketTextChannel;
            var birthdayChannel = _client.GetChannel(ClientToken.GuildChannels.Birthday) as SocketTextChannel;
            var selfRoleChannel = _client.GetChannel(ClientToken.GuildChannels.SelfRole) as SocketTextChannel;
            await welcomeChannel.SendMessageAsync($"Hey! {user.Mention} just joined {welcomeChannel.Guild.Name}! ♡ Check out " +
                                                  $"the {rulesChannel.Mention}, post your birthday here {birthdayChannel.Mention}, and get yourself " +
                                                  $"some {selfRoleChannel.Mention}!");
        }

        private async Task AnnouceUserLeft(IMentionable user)
        {
            var goodbyeChannel = _client.GetChannel(ClientToken.GuildChannels.Goodbye) as SocketTextChannel;
            await goodbyeChannel.SendMessageAsync($"{user.Mention} has left {goodbyeChannel.Guild.Name}! :(");
        }
    }
}