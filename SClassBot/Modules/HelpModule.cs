using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace SClassBot.Modules
{
    
    
    [Group("s!help")]    //Group of help commands
    [Alias("s!h", "s!commands")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        //Reduces redundant author tagging in embedded message, returns the embedded build
        private static Embed BuildWithHelpMenuAuthor(EmbedBuilder builder)
        {
            builder
                .WithAuthor(author =>
                {
                    author
                        .WithName("Sub-Bot Help Menu")
                        .WithUrl("https://crewszk.github.io/Sub-Bot/index.html")
                        .WithIconUrl("https://crewszk.github.io/images/botIcon.gif");
                });

            return builder.Build();
        }
        
        //Command calls for man page's for each command
        [Command]
        public async Task Help()    //This is the main help embed with all the standard commands
        {
            var builder = new EmbedBuilder()    
                .WithTitle("Click for Github Link with Help, Updates, and More!")
                .WithDescription("Use `s!help [command]` to get specialized help! For Example: `s!help hug`\n")
                .WithUrl("https://github.com/crewszk/Sub-Bot")
                .WithColor(new Color(RandColor.NewColor()))
                .WithFooter(footer =>
                {
                    footer
                        .WithText("To see moderator commands, type s@help")
                        .WithIconUrl("https://crewszk.github.io/images/botIcon.gif");
                })
                .WithAuthor(author =>
                {
                    author
                        .WithName("Standard Help Page")
                        .WithUrl("https://crewszk.github.io/Sub-Bot/index.html")
                        .WithIconUrl("https://crewszk.github.io/images/botIcon.gif");
                })
                .AddField("ℹ️  Information", "`help`  `ping`  `info`\n`role`  `channel`  `server`", true)
                .AddField("👫 Social", "`hug` `lick` `cuddle`", true)
                .AddField("🤣 Memes", "`quote`  `traps`  `pickle`\n`stinky`", true)
                .AddField("🎲 Games", "`trivia`  `roll`  `8ball`");
            
            await ReplyAsync("", false, builder.Build());
        }

        [Command("info")]
        [Alias("information", "sauce")]
        public async Task InfoHelp()    //Info man page
        {
            var builder = new EmbedBuilder()        
                .WithTitle("s!info")
                .WithDescription("``` Shows information about users in the server. ```")
                .WithColor(new Color(RandColor.NewColor()))
                .AddField("***Examples***", "`s!info`\nshows info about you\n`s!info @Hella.#7777`\n" +
                                            "shows info about Hewwa uwu\n", true)
                .AddField("***Usages***", "`s!info [user]`", true)
                .AddField("***Aliases***", "*__information__*, *__sauce__*");
            
            await ReplyAsync("", false, BuildWithHelpMenuAuthor(builder));
        }

        [Command("role")]    
        [Alias("roleinfo", "rinfo")]
        public async Task RoleHelp()    //Role man page
        {
            var builder = new EmbedBuilder()        
                .WithTitle("s!role")
                .WithDescription("``` Shows information about roles in the server. ```")
                .WithColor(new Color(RandColor.NewColor()))
                .AddField("***Examples***", "`s!role`\nshows info about your roles\n`s!role Moderators`\n" +
                                            "shows info about the Moderators role", true)
                .AddField("***Usages***", "`s!role [role]`", true)
                .AddField("***Aliases***", "*__roleinfo__*, *__rinfo__*");

            await ReplyAsync("", false, BuildWithHelpMenuAuthor(builder));
        }

        [Command("channel")]
        [Alias("channelinfo", "cinfo")]
        public async Task ChannelHelp()    //Channel man page
        {
            var builder = new EmbedBuilder()    
                .WithTitle("s!channel")
                .WithDescription("``` Shows information about channels in the server. ```")
                .WithColor(new Color(RandColor.NewColor()))
                .AddField("***Examples***", "`s!channel`\nshows info about the current channel\n`s!channel rules`\n" +
                                            "shows info about the rules channel", true)
                .AddField("***Usages***", "`s!channel [#channel]", true)
                .AddField("***Aliases***", "*__channelinfo__*, *__cinfo__*");
            
            await ReplyAsync("", false, BuildWithHelpMenuAuthor(builder));
        }

        [Command("server")]
        [Alias("serverinfo", "sinfo")]
        public async Task ServerHelp()     //Server man page
        {
            var builder = new EmbedBuilder()   
                .WithTitle("s!server")
                .WithDescription("``` Shows information about the server ```")
                .WithColor(new Color(RandColor.NewColor()))
                .AddField("***Examples***", "`s!server`\nshows info about the server", true)
                .AddField("***Usages***", "`s!server`", true)
                .AddField("***Aliases***", "*__serverinfo__*, *__sinfo__*");
            
            await ReplyAsync("", false, BuildWithHelpMenuAuthor(builder));
        }

        [Command("ping")]
        [Alias("beep", "bip")]
        public async Task PingHelp()    //Ping man page
        {
            var builder = new EmbedBuilder()        
                .WithTitle("s!ping")
                .WithDescription("``` Displays pong to see if the bot is online. ```")
                .WithColor(new Color(RandColor.NewColor()))
                .AddField("***Examples***", "`s!ping`\nWill display \'Pong!\'\n`s!beep`\nWill display \'Boop!\'" +
                                            "`s!bip`\nWill display \'Bap!\'", true)
                .AddField("***Usages***", "`s!ping`", true)
                .AddField("***Aliases***", "*__beep__*, *__bip__*");
            
            await ReplyAsync("", false, BuildWithHelpMenuAuthor(builder));
        }

        [Command("hug")]
        public async Task HugHelp()    //Hug man page
        {
            var builder = new EmbedBuilder()        
                .WithTitle("s!hug")
                .WithDescription("``` Outputs a random hugging gif. ```")
                .WithColor(new Color(RandColor.NewColor()))
                .AddField("***Examples***", "`s!hug`\nWill display hugging gif\n`s!hug @Hella.#7777`\n" +
                                            "Post a gif to hug Hewwa uwu", true)
                .AddField("***Usages***", "`s!hug [user]`", true);
            
            await ReplyAsync("", false, BuildWithHelpMenuAuthor(builder));
        }

        [Command("lick")]
        public async Task LickHelp()    //Lick man page
        {
            var builder = new EmbedBuilder()        
                .WithTitle("s!lick")
                .WithDescription("``` Outputs a random licking gif. ```")
                .WithColor(new Color(RandColor.NewColor()))
                .AddField("***Examples***", "`s!lick`\nWill display licking gif\n`s!lick @Hella.#7777`\n" +
                                            "Post a gif to lick Hewwa owo", true)
                .AddField("***Usages***", "`s!lick [user]`", true);
            
            await ReplyAsync("", false, BuildWithHelpMenuAuthor(builder));
        }

        [Command("cuddle")]
        public async Task CuddleHelp()    //Cuddle man page
        {
            var builder = new EmbedBuilder()     
                .WithTitle("s!cuddle")
                .WithDescription("``` Outputs a random cuddle gif. ```")
                .WithColor(new Color(RandColor.NewColor()))
                .AddField("***Examples***", "`s!cuddle`\nWill display cuddle gif\n`s!cuddle @Hella.#7777`\n" +
                                            "Post a gif to cuddle Hewwa owo", true)
                .AddField("***Usages***", "`s!cuddle [user]`", true);
            
            await ReplyAsync("", false, BuildWithHelpMenuAuthor(builder));
        }

        [Command("quote")]
        public async Task QuoteHelp()    //Quote man page
        {    
            var builder = new EmbedBuilder()        
                .WithTitle("s!quote")
                .WithDescription("``` Posts a quote from a specified \n user or records a quote for \n later use. ```")
                .WithColor(new Color(RandColor.NewColor()))
                .AddField("***Examples***", "`s!quote @Hella.#7777`\nPosts Hewwa's quote\n`s!quote \"Ok Boomer\"\n" +
                                            "Records the quote as your quote\n`s@quote @Hella.#7777 \"Ok Boomer\"`\n" +
                                            "Records the quote as Hewwa's quote [Mods Only]", true)
                .AddField("***Usages***", "`s!quote [user]`\n`s!quote \"[quote]\"`\n`s@quote [user] \"[quote]\"`", true);
            
            await ReplyAsync("", false, BuildWithHelpMenuAuthor(builder));
        }

        [Command("traps")]
        public async Task TrapsHelp()    //Traps man page
        {
            var builder = new EmbedBuilder()        
                .WithTitle("s!traps")
                .WithDescription("``` Outputs a random trap gif/picture.```")
                .WithColor(new Color(RandColor.NewColor()))
                .AddField("***Examples***", "`s!traps`\nWill display a trap gif/picture", true)
                .AddField("***Usages***", "`s!traps`", true);
            
            await ReplyAsync("", false, BuildWithHelpMenuAuthor(builder));
        }

        [Command("pickle")]
        public async Task PickleHelp()    //Pickle man page
        {
            var builder = new EmbedBuilder()      
                .WithTitle("s!pickle")
                .WithDescription("``` Calculates a users pickle size.```")
                .WithColor(new Color(RandColor.NewColor()))
                .AddField("***Examples***", "`s!pickle`\nWill display your pickle size\n`s!pickle @Hella.#7777`\n" +
                                            "Will display Hewwa's humongous pickle rick", true)
                .AddField("***Usages***", "`s!pickle [user]`", true);
            
            await ReplyAsync("", false, BuildWithHelpMenuAuthor(builder));
        }

        [Command("stinky")]
        public async Task StinkyHelp()    //Stinky man page
        {
            var builder = new EmbedBuilder()      
                .WithTitle("s!stinky")
                .WithDescription("``` Uh Oh Stinky. ```")
                .WithColor(new Color(RandColor.NewColor()))
                .AddField("***Examples***", "`s!stinky`\nUh Oh Stinky\n`s!stinky @Hella.#7777`\n" +
                                            "Hewwa's a stinky uwu", true)
                .AddField("***Usages***", "`s!stinky [user]`\n`s!stinky`", true);
            
            await ReplyAsync("", false, BuildWithHelpMenuAuthor(builder));
        }

        [Command("trivia")]
        public async Task TriviaHelp()    //Trivia man page
        {
            var builder = new EmbedBuilder()        
                .WithTitle("s!trivia")
                .WithDescription("``` Posts random trivia. ```")
                .WithColor(new Color(RandColor.NewColor()))
                .AddField("***Examples***", "`s!trivia`\nPosts random trivia", true)
                .AddField("***Usages***", "`s!trivia`", true);
            
            await ReplyAsync("", false, BuildWithHelpMenuAuthor(builder));
        }

        [Command("roll")]
        public async Task RollHelp()    //Roll man page
        {
            var builder = new EmbedBuilder()        
                .WithTitle("s!roll")
                .WithDescription("``` Role a specified die. ```")
                .WithColor(new Color(RandColor.NewColor()))
                .AddField("***Examples***", "`s!roll 1d6`\nRolls a single d6\n`s!roll 3d20`\n" +
                                            "Rolls three d20 dice", true)
                .AddField("***Usages***", "`s!roll [amount]d[die]", true);
            
            await ReplyAsync("", false, BuildWithHelpMenuAuthor(builder));
        }

        [Command("8ball")]
        [Alias("magic-conch")]
        public async Task MagicConchHelp()    //8ball man page
        {
            var builder = new EmbedBuilder()        
                .WithTitle("s!8ball")
                .WithDescription("``` Play a game of 8ball with by \n asking a question. ```")
                .WithColor(new Color(RandColor.NewColor()))
                .AddField("***Examples***", "`s!8ball Is Hella cool?`\nWill post a random 8ball response", true)
                .AddField("***Usages***", "`s!8ball [statement]", true);
            
            await ReplyAsync("", false, BuildWithHelpMenuAuthor(builder));
        }
    }

    
    //Moderator only man pages
    [RequireRoleAttribute("⚖️Moderators⚖️")]
    [Group("s@help")]
    public class ModHelpModule : ModuleBase<SocketCommandContext>
    {
        //Reduces redundant author tagging in embedded message, returns the builded solution
        private static Embed BuildWithHelpMenuAuthor(EmbedBuilder builder)
        {
            builder
                .WithAuthor(author =>
                {
                    author
                        .WithName("Sub-Bot Mod Help Menu")
                        .WithUrl("https://crewszk.github.io/Sub-Bot/index.html")
                        .WithIconUrl("https://crewszk.github.io/images/botIcon.gif");
                });

            return builder.Build();
        }

        //Command calls for all mod based help messages
        [Command]
        public async Task ModHelp()    //Standard Mod Help man page
        {
            var builder = new EmbedBuilder()    
                .WithTitle("Click for GitHub link with Help, Updates, and More!")
                .WithDescription($"Use `help [command]` to get specialized help! For Example: `s@help kick`")
                .WithUrl("https://github.com/crewszk/Sub-Bot")
                .WithColor(new Color(RandColor.NewColor()))
                .WithFooter(footer =>
                {
                    footer
                        .WithText("To see standard commands, type s!help")
                        .WithIconUrl("https://crewszk.github.io/images/botIcon.gif");
                })
                .WithAuthor(author =>
                {
                    author
                        .WithName("Moderator Help Page")
                        .WithUrl("https://crewszk.github.io/Sub-Bot/index.html")
                        .WithIconUrl("https://crewszk.github.io/images/botIcon.gif");
                })
                .AddField("⚖ Moderation Tools", "`kick` `ban` `mute`\n`prune` `blacklist` `role`", true);
                //.AddField("🛠 Settings", "`disable` `enable` `welcome`\n`goodbye` `timezone`\n`nsfw` ", true);
            
            await ReplyAsync("", false, builder.Build());
        }

        [Command("kick")]
        [Alias("remove", "boot")]
        public async Task KickHelp()    //Kick man page
        {
            var builder = new EmbedBuilder()    
            .WithTitle("s@kick")
            .WithDescription("``` Kicks a specified user from the server. \n You may specify a reason. ```")
            .WithColor(new Color(RandColor.NewColor()))
            .AddField("***Examples***", "`s@kick @Hella.#7777`\nKicks Hewwa from the server uwu", true)
            .AddField("***Usages***", "`s@kick [user] [reason]`", true)
            .AddField("***Aliases***", "*__remove__*, *__boot__*");
            
            await ReplyAsync("", false ,BuildWithHelpMenuAuthor(builder));
        }

        [Command("ban")]
        [Alias("thanos", "banish", "begone")]
        public async Task BanHelp()    //Ban man page
        {
            var builder = new EmbedBuilder()    
                .WithTitle("s@ban")
                .WithDescription("``` Bans a specified user from the server. \n You may specify a reason. ```")
                .WithColor(new Color(RandColor.NewColor()))
                .AddField("***Examples***", "`s@ban @Hella.#7777`\nBans Hewwa from the server uwu", true)
                .AddField("***Usages***", "`s@ban [user] [reason]`", true)
                .AddField("***Aliases***", "*__outlaw__*, *__banish__*, *__begone__*");
            
            await ReplyAsync("", false ,BuildWithHelpMenuAuthor(builder));
        }

        [Command("mute")]
        [Alias("silence", "gag", "muzzle")]
        public async Task MuteHelp()    //Mute man page
        {
            var builder = new EmbedBuilder()    
                .WithTitle("s@mute")
                .WithDescription("``` Mutes a specified user in the server. \n You may specify a reason. ```")
                .WithColor(new Color(RandColor.NewColor()))
                .AddField("***Examples***", "`s@mute @Hella.#7777`\nMutes Hewwa uwu", true)
                .AddField("***Usages***", "`s@mute [user] [reason]`", true)
                .AddField("***Aliases***", "*__silence__*, *__gag__*, *__muzzle__*");
            
            await ReplyAsync("", false ,BuildWithHelpMenuAuthor(builder));
        }

        [Command("prune")]
        public async Task PruneHelp()    //Prune man page
        {
            var builder = new EmbedBuilder() 
                .WithTitle("s@prune")
                .WithDescription("``` Prunes messages from the channel.\n You may specify a user, a string, or a flag." +
                                 "\n Omitting a number prunes the most recent message.```")
                .WithColor(new Color(RandColor.NewColor()))
                .AddField("***Examples***",
                    "`s@prune 20`\nRemoves last 20 messages in channel\n`s@prune 10 @Hella.#7777`\n" +
                    "Removes last 20 messages sent by Hella\nin the channel\n`s@prune 5 Handholding`\n" +
                    "Removes last 5 messages with the string \"Handholding\"\n`s@prune 10 -images`\n" +
                    "Removes last 10 images posted", true)
                .AddField("***Usages***", "`s@prune [number]`\n`s@prune [user]`\n`s@prune [string]`\n`s@prune [flag]`\n" +
                                          "`s@prune [number] [user]`\n`s@prune [number] [string]`\n`s@prune [number] [flag]`\n" +
                                          "`s@prune [user] [string]`\n`s@prune [user] [flag]`\n`s@prune [number] [user] [flag]`\n" +
                                          "`s@prune [number] [user] [string]`", true)
                .AddField("***Flags***", "*__-image__*, *__-images__*, *__-i__*\n*__-links__*, *__-link__*, *__-l__*\n*__-mentions__*, *__-mention__*, *__-m__*");
            
            await ReplyAsync("", false ,BuildWithHelpMenuAuthor(builder));
        }

        [Command("blacklist")]
        public async Task BlacklistHelp()    //Blacklist man page
        {
            var builder = new EmbedBuilder()   
                .WithTitle("s@blacklist")
                .WithDescription("``` Places a specified user onto the \n servers blacklist. ```")
                .WithColor(new Color(RandColor.NewColor()))
                .AddField("***Examples***", "`s@blacklist add 140912052657979392`\nAdds specified user id to blacklist", true)
                .AddField("***Usages***", "`s@blacklist <add/remove/clear> [id]`", true);
            
            await ReplyAsync("", false ,BuildWithHelpMenuAuthor(builder));
        }
    }
}