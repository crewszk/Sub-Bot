using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Timers;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;

namespace SClassBot.Modules
{
    public class ModToolModule : ModuleBase<SocketCommandContext>
    {
        [Summary("Checks for a defined permission, posts embed to current channel if user lacks permissions")]
        private bool CheckPermissions(IGuildUser user, string permission)
        {
            var result = permission switch
            {
                "kick" => user.GuildPermissions.KickMembers,
                "ban" => user.GuildPermissions.BanMembers,
                "message" => user.GuildPermissions.ManageMessages,
                "role" => user.GuildPermissions.ManageRoles
            };
            
            var builder = new EmbedBuilder()
                .WithDescription("Nice try bitch. You don't have the right permissions for this command.")
                .WithAuthor(author =>
                {
                    author
                        .WithName("Sub-Bot Defense Mode")
                        .WithIconUrl("https://crewszk.github.io/images/botIconSpooky.gif")
                        .WithUrl("https://github.com/crewszk/Sub-Bot");
                })
                .WithColor(new Color(255, 0, 0))
                .WithFooter(footer =>
                {
                    footer
                        .WithText($"Command requested by {user.Username}")
                        .WithIconUrl(user.GetAvatarUrl());
                })
                .WithCurrentTimestamp();

            if(result == false)
                ReplyAsync("", false, builder.Build());

            return result;
        }
        
        [Command("s@kick")]
        [Alias("s@remove", "s@boot")]
        [Summary("Kicks a defined user, a reason can be provided")]
        public async Task KickAsync(IGuildUser user = null, [Remainder]string kickReason = "No Reason Provided")
        {
            var guildUser = Context.Guild.GetUser(Context.User.Id);
            if (!CheckPermissions(guildUser, "bane")) return;
            
            if (user == null)
            {
                await ReplyAsync("I need to know who you're trying to kick, try `s@help kick` for help");
                return;
            }
            
            await user.KickAsync(kickReason);

            var builder = new EmbedBuilder()
                .WithTitle("Get kicked nerd!")
                .WithDescription($"{user.Username} has been kicked from the {Context.Guild.Name}")
                .WithCurrentTimestamp()
                .WithColor(new Color(0, 255, 0))
                .WithAuthor(author =>
                {
                    author
                        .WithName("Sub-Bot Offense Mode")
                        .WithIconUrl("https://crewszk.github.io/images/botIcon.gif");
                })
                .WithFooter(footer =>
                {
                    footer
                        .WithText($"Command requested by {guildUser.Username}")
                        .WithIconUrl(guildUser.GetAvatarUrl());
                })
                .AddField("User", $"{user.Mention}", true)
                .AddField("Moderator", $"{Context.User.Username}", true)
                .AddField("Other Information", "Kick Request means can be invited back")
                .AddField("Command Used", $"``s@kick {user.Username} \"{kickReason}\"``");

            await ReplyAsync("", false, builder.Build());
        }

        [Command("s@ban")]
        [Alias("s@thanos", "s@banish", "s@begone")]
        [Summary("Bans a defined user, a reason can be provided")]
        public async Task BanAsync(IGuildUser user = null, [Remainder]string banReason = "No Reason Provided")
        {
            var guildUser = Context.Guild.GetUser(Context.User.Id);
            if (!CheckPermissions(guildUser, "ban")) return;
            
            if (user == null)
            {
                await ReplyAsync("I need to know who you're trying to ban, try `s@help ban` for help");
                return;
            }
            
            await user.BanAsync(7, banReason);
            
            var builder = new EmbedBuilder()
                .WithTitle("Get banned nerd!")
                .WithDescription($"{user.Username} has been banned from {Context.Guild.Name}")
                .WithCurrentTimestamp()
                .WithColor(new Color(0, 255, 0))
                .WithAuthor(author =>
                {
                    author
                        .WithName("Sub-Bot Offense Mode")
                        .WithIconUrl("https://crewszk.github.io/images/botIcon.gif");
                })
                .WithFooter(footer =>
                {
                    footer
                        .WithText($"Command requested by {guildUser.Username}")
                        .WithIconUrl(guildUser.GetAvatarUrl());
                })
                .AddField("User", $"{user.Mention}")
                .AddField("Moderator", $"{Context.User.Mention}")
                .AddField("Other Information", "A Ban means no return until further notice")
                .AddField("Command Used", $"``s@ban {user.Mention} \"{banReason}\"``");

            await ReplyAsync("", false, builder.Build());
        }

        [Command("s@mute")]
        [Alias("s@slience", "s@gag", "s@muzzle")]
        [Summary("Mutes a defined user for a given amount of time, gives the user the 'Muted' role")]
        public async Task MuteAsync(IGuildUser user = null, uint time = 1)
        {
            var guildUser = Context.Guild.GetUser(Context.User.Id);
            if (!CheckPermissions(guildUser, "role")) return;
            
            if (user == null)
            {
                await ReplyAsync("I need to know who you're trying to mute, try `s@help mute` for help");
                return;
            }
            
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Muted");
            await user.AddRoleAsync(role);

            var builder = new EmbedBuilder()
                .WithTitle("Get muted bitch")
                .WithDescription($"{user.Mention} has been muted for {time} minute(s).")
                .WithColor(new Color(0, 255, 0))
                .WithCurrentTimestamp()
                .WithAuthor(author =>
                {
                    author
                        .WithName("Sub-Bot Offense Mode")
                        .WithIconUrl("https://crewszk.github.io/images/botIconSpooky.gif");
                })
                .WithFooter(footer =>
                {
                    footer
                        .WithText($"Command requested by {guildUser.Username}")
                        .WithIconUrl(guildUser.GetAvatarUrl());
                });

            await ReplyAsync("", false, builder.Build());

            await Task.Delay(TimeSpan.FromMinutes(time)).ContinueWith(t => RemoveMuteSend(user, guildUser, Context.Channel, role));
        }

        [Summary("Helper Task for timed delay of removing the 'Muted' role")]
        private static async Task RemoveMuteSend(IGuildUser user, IUser guildUser, ISocketMessageChannel channel, IRole role)
        {
            var builder = new EmbedBuilder()
                .WithTitle("Unmuted")
                .WithDescription($"{user.Username} has been unmuted for now")
                .WithColor(new Color(RandColor.NewColor()))
                .WithCurrentTimestamp()
                .WithAuthor(author =>
                {
                    author
                        .WithName("Infraction Cleared")
                        .WithIconUrl("https://crewszk.github.io/images/botIcon.gif")
                        .WithUrl("https://github.com/crewszk/Sub-Bot");
                })
                .WithFooter(footer =>
                {
                    footer
                        .WithText($"Command originally requested by {guildUser.Username}")
                        .WithIconUrl(guildUser.GetAvatarUrl());
                });

            await user.RemoveRoleAsync(role);

            await channel.SendMessageAsync("", false, builder.Build());
        }

        [Command("s@prune")]
        [Priority(4)]
        [Summary("Prune a given amount of messages, default is most recent message")]
        public async Task PruneAsync(uint count = 1)
        {
            var guildUser = Context.Guild.GetUser(Context.User.Id);
            if (!CheckPermissions(guildUser, "message")) return;
            
            var messages = await Context.Channel.GetMessagesAsync((int)count + 1).FlattenAsync();

            messages = messages.Skip(1);
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
        }

        [Command("s@prune")]
        [Priority(3)]
        [Summary("Prune a given amount of messages of a given user")]
        public async Task PruneAsync(uint count, IGuildUser user)
        {
            var guildUser = Context.Guild.GetUser(Context.User.Id);
            if (!CheckPermissions(guildUser, "message")) return;
            
            var messages = await Context.Channel.GetMessagesAsync(100).FlattenAsync();

            messages = from msg in messages where msg.Author == user select msg;

            messages = messages.Take((int)count);
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
        }

        [Command("s@prune")]
        [Priority(3)]
        [Summary("Prune a given amount of messages defined by a flag")]
        public async Task PruneAsync(uint count, string flag)
        {
            var guildUser = Context.Guild.GetUser(Context.User.Id);
            if (!CheckPermissions(guildUser, "message")) return;
            
            var messages = await Context.Channel.GetMessagesAsync(100).FlattenAsync();
            
            messages = HandleFlagAsync(messages, flag, count);
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
        }

        [Command("s@prune")]
        [Priority(2)]
        [Summary("Prune a given amount of messages, defined by a flag, of a given user ")]
        public async Task PruneAsync(uint count, IGuildUser user, string flag)
        {
            var guildUser = Context.Guild.GetUser(Context.User.Id);
            if (!CheckPermissions(guildUser, "message")) return;
            
            var messages = await Context.Channel.GetMessagesAsync(100).FlattenAsync();
            messages = from msg in messages where msg.Author == user select msg;
            
            messages = HandleFlagAsync(messages, flag, count);
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
        }

        [Command("s@prune")]
        [Priority(1)]
        [Summary("Prune the most recent message of a given user")]
        public async Task PruneAsync(IGuildUser user)
        {
            var guildUser = Context.Guild.GetUser(Context.User.Id);
            if (!CheckPermissions(guildUser, "message")) return;
            
            var messages = await Context.Channel.GetMessagesAsync(100).FlattenAsync();
            messages = from msg in messages where msg.Author == user select msg;

            messages = messages.Take(1);
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
        
        }

        [Command("s@prune")]
        [Priority(1)]
        [Summary("Prune the most recent message defined by a flag")]
        public async Task PruneAsync(string flag)
        {
            var guildUser = Context.Guild.GetUser(Context.User.Id);
            if (!CheckPermissions(guildUser, "message")) return;

            var messages = await Context.Channel.GetMessagesAsync(100).FlattenAsync();
            messages = HandleFlagAsync(messages, flag, 1);
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
        }
        
        [Summary("Helper method for handling flags in the pruning process, returns a IMessage list of messages " +
                 "that contain the provided flag")]
        private static IEnumerable<IMessage> HandleFlagAsync(IEnumerable<IMessage> messages, string flag, uint count)
        {
            messages = flag switch
            {
                "-images" => (from msg in messages where msg.Attachments.Any() select msg),
                "-image" => (from msg in messages where msg.Attachments.Any() select msg),
                "-i" => (from msg in messages where msg.Attachments.Any() select msg),
                "-mentions" => (from msg in messages where msg.MentionedUserIds.Any() select msg),
                "-mention" => (from msg in messages where msg.MentionedUserIds.Any() select msg),
                "-m" => (from msg in messages where msg.MentionedUserIds.Any() select msg),
                "-links" => (from msg in messages where msg.Content.Contains("http") select msg),
                "-link" => (from msg in messages where msg.Content.Contains("http") select msg),
                "-l" => (from msg in messages where msg.Content.Contains("http") select msg),
                _ => (from msg in messages where msg.Content.Contains(flag) select msg)
            };

            messages = messages.Take((int)count);
            return messages;
        }
    }
}