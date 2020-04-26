using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace SClassBot.Modules
{
    public class MemesModule : ModuleBase<SocketCommandContext>
    {
        [Command("s!stinky")]
        public async Task StinkyAsync(IGuildUser user = null)
        {
            var builder = new EmbedBuilder();

            if (user != null)
            {
                var targetUser = (user as SocketGuildUser).Nickname ?? (user as SocketGuildUser).Username;

                builder.WithDescription($"Uh oh, **{targetUser}** is stinky. Smelly poopy!")
                    .WithImageUrl(RandomReferences.NewGif("stinky"))
                    .WithColor(new Color(RandomReferences.NewColor()));
            }
            else
            {
                builder.WithDescription("Uh oh, stinky. Smelly poopy! Poooop, smelly poop!")
                    .WithImageUrl(RandomReferences.NewGif("stinky"))
                    .WithColor(new Color(RandomReferences.NewColor()));
            }
            
            await ReplyAsync("", false, builder.Build());
        }

        [Command("s!pickle")]
        public async Task PickleAsync(IGuildUser user = null)
        {
            var builder = new EmbedBuilder();
            
            var pickle = user == null                                                 
                ? Context.User.Id % 3000 != 0 ? (double) (Context.User.Id % 3000) / 100 : 0   
                : user.Id % 3000 != 0 ? (double) (user.Id % 3000) / 100 : 0;                  
            
            var targetUser = user == null                                                               
                ? (Context.User as SocketGuildUser).Nickname ?? (Context.User as SocketGuildUser).Username      
                : (user as SocketGuildUser).Nickname ?? (user as SocketGuildUser).Username;                     

            builder.WithDescription($"**{targetUser}** has a {pickle:F} cm pickle!")
                .WithColor(new Color(RandomReferences.NewColor()));

            if (pickle == 0)
            {
                builder.AddField("So there's this show about this scientist named Rick.", "He turns himself into a pickle, he becomes Pickle Rick\n" +
                                 "And I tell ya, he says \"I turned myself into a pickle!\"\nI'll never forget man, funniest shit I've ever seen");
            }
            
            await ReplyAsync("", false, builder.Build());
        }
    }
}