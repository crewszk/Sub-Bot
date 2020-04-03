using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace SClassBot.Modules
{
    public class InformationModule : ModuleBase<SocketCommandContext>
    {
        private async Task EmbedReply(Embed embed)
        {
            await ReplyAsync("", false, embed);
        }
        
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("Pong!");
        }

        [Command("beep")]
        public async Task Beep()
        {
            await ReplyAsync("Boop!");
        }

        [Command("bip")]
        public async Task Bip()
        {
            await ReplyAsync("Bap!");
        }
        
        
    }
}