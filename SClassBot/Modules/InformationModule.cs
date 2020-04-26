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
        
        [Command("s!ping")]
        public async Task Ping()
        {
            await ReplyAsync("Pong!");
        }

        [Command("s!pong")]
        public async Task Pong()
        {
            await ReplyAsync("Ping!");
        }

        [Command("s!beep")]
        public async Task Beep()
        {
            await ReplyAsync("Boop!");
        }

        [Command("s!bip")]
        public async Task Bip()
        {
            await ReplyAsync("Bap!");
        }

        
    }
}