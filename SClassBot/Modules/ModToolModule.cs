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
        //private static string ModPrefix = CommandLoggingandTokenAccess.ClientToken.ModPrefix;
        
        [Summary("Checks for a defined permission, posts embed to current channel if user lacks permissions")]
        private bool CheckPermissions(IGuildUser user, string permission)
        {
            var result = permission switch
            {
                "kick" => user.GuildPermissions.KickMembers,
                "ban" => user.GuildPermissions.BanMembers,
                "message" => user.GuildPermissions.ManageMessages,
                "role" => user.GuildPermissions.ManageRoles,
                _ => false
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
            if (!CheckPermissions(guildUser, "kick")) return;
            
            if (user == null)
            {
                await ReplyAsync("I need to know who you're trying to kick, try `s@help kick` for help");
                return;
            }
            
            var targetUser = (user as SocketGuildUser).Nickname ?? (user as SocketGuildUser).Username;
            
            await user.KickAsync(kickReason);

            await BotLogAsync(guildUser, "s@kick", $"decided to kick ***{targetUser}***.", kickReason);
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
            
            var targetUser = (user as SocketGuildUser).Nickname ?? (user as SocketGuildUser).Username;
            
            await user.BanAsync(7, banReason);

            await BotLogAsync(guildUser, "s@ban", $"decided to ban ***{targetUser}***.", banReason);
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
            
            var targetUser = (user as SocketGuildUser).Nickname ?? (user as SocketGuildUser).Username;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Muted");
            await user.AddRoleAsync(role);

            await BotLogAsync(guildUser, "s@mute",
                $"decided to mute **{targetUser}** in {(Context.Channel as SocketTextChannel).Mention}.\n" +
                $"They have been muted for {time} minute(s)");
            
            Task.Delay(TimeSpan.FromMinutes(time)).ContinueWith(t => 
                RemoveMuteSend(user, Context.Guild.GetTextChannel(CommandHandler.ClientToken.GuildChannels.BotLog), role));
        }

        [Summary("Helper Task for timed delay of removing the 'Muted' role")]
        private static async Task RemoveMuteSend(IGuildUser user, ISocketMessageChannel channel, IRole role)
        {
            var targetUser = user.Nickname ?? user.Username;
            var builder = new EmbedBuilder()
                .WithDescription($"**{targetUser}** has been un-muted.")
                .WithColor(new Color(RandomReferences.NewColor()));

            await user.RemoveRoleAsync(role);

            await channel.SendMessageAsync("", false, builder.Build());
        }

        [Command("s@prune")]
        [Priority(4)]
        [Summary("Prune a given amount of messages, default is most recent message")]
        public async Task PruneAsync(int count = 1)
        {
            var guildUser = Context.Guild.GetUser(Context.User.Id);
            if (!CheckPermissions(guildUser, "message")) return;
            
            //Limit of 100 messages to prune, minimum 1 message
            if (count > 100) count = 99;
            else if (count <= 0) count = 1;
            
            var messages = await Context.Channel.GetMessagesAsync(count + 1).FlattenAsync();
            
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages.Skip(1));

            await BotLogAsync(guildUser, "s@prune",
                $"pruned {count} message(s) in {(Context.Channel as SocketTextChannel).Mention}.\n" +
                "The messages were made by various users.");
        }

        [Command("s@prune")]
        [Priority(3)]
        [Summary("Prune a given amount of messages of a given user")]
        public async Task PruneAsync(int count, IGuildUser user)
        {
            var guildUser = Context.Guild.GetUser(Context.User.Id);
            var targetUser = (user as SocketGuildUser).Nickname ?? (user as SocketGuildUser).Username;
            if (!CheckPermissions(guildUser, "message")) return;
            
            //Limit of 100 messages to prune, minimum 1 message
            if (count > 100) count = 99;
            else if (count <= 0) count = 1;
            
            var messages = 
                from msg in await Context.Channel.GetMessagesAsync(100).FlattenAsync()
                where msg.Author == user
                select msg;
            
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages.Take(count));

            await BotLogAsync(guildUser, "s@prune",
                $"pruned {count} message(s) in {(Context.Channel as SocketTextChannel).Mention}.\n" +
                $"The messages were made by **{targetUser}**.");
        }

        [Command("s@prune")]
        [Priority(3)]
        [Summary("Prune a given amount of messages defined by a flag")]
        public async Task PruneAsync(int count, string flag)
        {
            var guildUser = Context.Guild.GetUser(Context.User.Id);
            if (!CheckPermissions(guildUser, "message")) return;
            
            //Limit of 100 messages to prune, minimum 1 message
            if (count > 100) count = 99;
            else if (count <= 0) count = 1;
            
            var messages = HandleFlagAsync(await Context.Channel.GetMessagesAsync(100).FlattenAsync(), flag, count);
            
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);

            await BotLogAsync(guildUser, "s@prune",
                $"pruned {count} message(s) in {(Context.Channel as SocketTextChannel).Mention} with the flag \"{flag}\".\n" +
                "The messages were made by various users");
        }

        [Command("s@prune")]
        [Priority(2)]
        [Summary("Prune a given amount of messages, defined by a flag, of a given user ")]
        public async Task PruneAsync(int count, IGuildUser user, string flag)
        {
            var guildUser = Context.Guild.GetUser(Context.User.Id);
            var targetUser = (user as SocketGuildUser).Nickname ?? (user as SocketGuildUser).Username;
            if (!CheckPermissions(guildUser, "message")) return;
            
            //Limit of 100 messages to prune, minimum 1 message
            if (count > 100) count = 99;
            else if (count <= 0) count = 1;
            
            var messages =
                HandleFlagAsync(
                    from msg in await Context.Channel.GetMessagesAsync(100).FlattenAsync()
                        where msg.Author == user
                        select msg, flag, count);

            //When the message to be pruned is made by the command user, skip ahead once so the command input doesn't get targeted
            if (Context.User == user)
                messages = messages.Skip(1);
            
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);

            await BotLogAsync(guildUser, "s@prune",
                $"pruned {count} message(s) in {(Context.Channel as SocketTextChannel).Mention} with the flag \"{flag}\".\n" +
                $"The messages were made by **{targetUser}**.");
        }

        [Command("s@prune")]
        [Priority(1)]
        [Summary("Prune the most recent message of a given user")]
        public async Task PruneAsync(IGuildUser user)
        {
            var guildUser = Context.Guild.GetUser(Context.User.Id);
            var targetUser = (user as SocketGuildUser).Nickname ?? (user as SocketGuildUser).Username;
            if (!CheckPermissions(guildUser, "message")) return;

            var messages = 
                from msg in await Context.Channel.GetMessagesAsync(100).FlattenAsync()
                    where msg.Author == user
                    select msg;
            
            //When the message to be pruned is made by the command user, skip ahead once so the command input doesn't get targeted
            if (Context.User == user)
                messages = messages.Skip(1);

            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages.Take(1));
        
            await BotLogAsync(guildUser, "s@prune",
                $"pruned 1 message in {(Context.Channel as SocketTextChannel).Mention}.\n" +
                $"The message was made by **{targetUser}**.");
        }

        [Command("s@prune")]
        [Priority(1)]
        [Summary("Prune the most recent message defined by a flag")]
        public async Task PruneAsync(string flag)
        {
            var guildUser = Context.Guild.GetUser(Context.User.Id);
            if (!CheckPermissions(guildUser, "message")) return;
            
            //Return as list for multiple enumerations
            var messages = 
                HandleFlagAsync(await Context.Channel.GetMessagesAsync(100).FlattenAsync(), flag, 1).ToList();
            
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);

            var targetUser = (messages[0].Author as SocketGuildUser).Nickname ?? (messages[0].Author as SocketGuildUser).Username;
            
            await BotLogAsync(guildUser, "s@prune",
             $"pruned 1 message in {(Context.Channel as SocketTextChannel).Mention} with the flag \"{flag}\".\n" +
             $"The message was made by **{targetUser}**.");
        }
        
        [Summary("Helper method for handling flags in the pruning process, returns a IMessage list of messages" +
                 "that contain the provided flag. If the flag isn't a special keyword, it skips the Authors message" +
                 "to preserve the prune command")]
        private static IEnumerable<IMessage> HandleFlagAsync(IEnumerable<IMessage> messages, string flag, int count)
        {
            return flag switch
            {
                "-images" => (from msg in messages where msg.Attachments.Any() select msg).Take(count),
                "-image" => (from msg in messages where msg.Attachments.Any() select msg).Take(count),
                "-i" => (from msg in messages where msg.Attachments.Any() select msg).Take(count),
                "-mentions" => (from msg in messages where msg.MentionedUserIds.Any() select msg).Take(count),
                "-mention" => (from msg in messages where msg.MentionedUserIds.Any() select msg).Take(count),
                "-m" => (from msg in messages where msg.MentionedUserIds.Any() select msg).Take(count),
                "-links" => (from msg in messages where msg.Content.Contains("http") select msg).Take(count),
                "-link" => (from msg in messages where msg.Content.Contains("http") select msg).Take(count),
                "-l" => (from msg in messages where msg.Content.Contains("http") select msg).Take(count),
                _ => (from msg in messages where msg.Content.Contains(flag) select msg).Take(count)
            };
        }

        [Summary("Logging method for logging bot command outcomes for mod commands inside a specified command channel")]
        private async Task BotLogAsync(IGuildUser user, string command, string context, string reason = null)
        {
            var targetUser = user.Nickname ?? user.Username;
            var avatar = user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl();
            
            var builder = new EmbedBuilder()
                .WithDescription($"**{targetUser}** {context}")
                .WithFooter(footer =>
                {
                    footer
                        .WithText($"Command requested by {targetUser}")
                        .WithIconUrl(avatar);
                })
                .WithColor(new Color(RandomReferences.NewColor()));

            if (reason != null)
                builder.AddField("*Reason*", $"{reason}", true);

            await Context.Guild.GetTextChannel(CommandHandler.ClientToken.GuildChannels.BotLog)
                .SendMessageAsync("", false, builder.Build());
        }
    }
}