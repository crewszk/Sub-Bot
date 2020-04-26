using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace SClassBot.Modules
{
    public class SocialModule : ModuleBase<SocketCommandContext>
    {
        [Command("s!hug")]
        public async Task HugAsync(IGuildUser user = null)
        {
            var builder = new EmbedBuilder();
            var guildUserName = (Context.User as SocketGuildUser).Nickname ?? (Context.User as SocketGuildUser).Username;

            if (user != null)
            {
                var targetUser = (user as SocketGuildUser).Nickname ?? (user as SocketGuildUser).Username;
                

                builder.WithDescription($"**{targetUser}**, you got a hug from **{guildUserName}**")
                    .WithImageUrl(RandomReferences.NewGif("hug"))
                    .WithColor(new Color(RandomReferences.NewColor()));
            }
            else
            {
                builder.WithDescription($"**{guildUserName}** is hugging themselves, don't be so alone 😭")
                    .WithImageUrl(RandomReferences.NewGif("alone"))
                    .WithColor(new Color(RandomReferences.NewColor()));
            }

            await ReplyAsync("", false, builder.Build());
        }

        [Command("s!cuddle")]
        public async Task CuddleAsync(IGuildUser user = null)
        {
            var builder = new EmbedBuilder();
            var guildUserName = (Context.User as SocketGuildUser).Nickname ?? (Context.User as SocketGuildUser).Username;

            if (user != null)
            {
                var targetUser = (user as SocketGuildUser).Nickname ?? (user as SocketGuildUser).Username;
                

                builder.WithDescription($"**{targetUser}**, you got a cuddle from **{guildUserName}**")
                    .WithImageUrl(RandomReferences.NewGif("cuddle"))
                    .WithColor(new Color(RandomReferences.NewColor()));
            }
            else
            {
                builder.WithDescription($"**{guildUserName}** is cuddling with themselves, don't be so alone 😭")
                    .WithImageUrl(RandomReferences.NewGif("alone"))
                    .WithColor(new Color(RandomReferences.NewColor()));
            }

            await ReplyAsync("", false, builder.Build());
        }

        [Command("s!alone")]
        public async Task AloneAsync()
        {
            var builder = new EmbedBuilder();
            var guildUserName = (Context.User as SocketGuildUser).Nickname ?? (Context.User as SocketGuildUser).Username;
            
            builder.WithDescription($"**{guildUserName}** is feeling alone, don't be so alone 😭")
                .WithImageUrl(RandomReferences.NewGif("alone"))
                .WithColor(new Color(RandomReferences.NewColor()));
            
            await ReplyAsync("", false, builder.Build());
        }

        [Command("s!dance")]
        public async Task DanceAsync(IGuildUser user = null)
        {
            var builder = new EmbedBuilder();
            var guildUserName = (Context.User as SocketGuildUser).Nickname ?? (Context.User as SocketGuildUser).Username;

            if (user != null)
            {
                var targetUser = (user as SocketGuildUser).Nickname ?? (user as SocketGuildUser).Username;
                

                builder.WithDescription($"**{targetUser}**, **{guildUserName}** is trying to dance with you!")
                    .WithImageUrl(RandomReferences.NewGif("dance"))
                    .WithColor(new Color(RandomReferences.NewColor()));
            }
            else
            {
                builder.WithDescription($"**{guildUserName}** is feeling the groove!")
                    .WithImageUrl(RandomReferences.NewGif("dance"))
                    .WithColor(new Color(RandomReferences.NewColor()));
            }

            await ReplyAsync("", false, builder.Build());
        }

        [Command("s!laugh")]
        public async Task LaughAsync(IGuildUser user = null)
        {
            var builder = new EmbedBuilder();
            var guildUserName = (Context.User as SocketGuildUser).Nickname ?? (Context.User as SocketGuildUser).Username;

            if (user != null)
            {
                var targetUser = (user as SocketGuildUser).Nickname ?? (user as SocketGuildUser).Username;
                

                builder.WithDescription($"**{targetUser}**, you're getting laughed at by **{guildUserName}**")
                    .WithImageUrl(RandomReferences.NewGif("laugh"))
                    .WithColor(new Color(RandomReferences.NewColor()));
            }
            else
            {
                builder.WithDescription($"**{guildUserName}** is bursting in laughter!")
                    .WithImageUrl(RandomReferences.NewGif("laugh"))
                    .WithColor(new Color(RandomReferences.NewColor()));
            }

            await ReplyAsync("", false, builder.Build());
        }

        [Command("s!poke")]
        public async Task PokeAsync(IGuildUser user = null)
        {
            var builder = new EmbedBuilder();
            var guildUserName = (Context.User as SocketGuildUser).Nickname ?? (Context.User as SocketGuildUser).Username;

            if (user != null)
            {
                var targetUser = (user as SocketGuildUser).Nickname ?? (user as SocketGuildUser).Username;
                

                builder.WithDescription($"**{targetUser}**, you're getting poked at by **{guildUserName}**")
                    .WithImageUrl(RandomReferences.NewGif("poke"))
                    .WithColor(new Color(RandomReferences.NewColor()));
            }
            else
            {
                await ReplyAsync($"Uhm **{guildUserName}**... You're just poking the air... 🙃");
                return;
            }

            await ReplyAsync("", false, builder.Build());
        }

        [Command("s!slap")]
        public async Task SlapAsync(IGuildUser user = null)
        {
            var builder = new EmbedBuilder();
            var guildUserName = (Context.User as SocketGuildUser).Nickname ?? (Context.User as SocketGuildUser).Username;

            if (user != null)
            {
                var targetUser = (user as SocketGuildUser).Nickname ?? (user as SocketGuildUser).Username;
                

                builder.WithDescription($"**{guildUserName}** just slapped **{targetUser}**!")
                    .WithImageUrl(RandomReferences.NewGif("slap"))
                    .WithColor(new Color(RandomReferences.NewColor()));
            }
            else
            {
                await ReplyAsync($"Uhm **{guildUserName}**... You're just slapping the air... 🙃");
                return;
            }

            await ReplyAsync("", false, builder.Build());
        }
    }
}