using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace CommandModules
{
    public class Basic : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("Pong");
        }
    }
}