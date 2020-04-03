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
        private static readonly EmbedBuilder helpBuilder = new EmbedBuilder()    //This is the main help embed with all the standard commands
            .WithTitle("Click for Github Link with Help, Updates, and More!")
            .WithDescription("Use `s!help [command]` to get specialized help! For Example: `s!help hug`")
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
            .AddField("🎲 Games", "`trivia`  `roll`  `magic-conch`");

        private static readonly EmbedBuilder infoHelpBuilder = new EmbedBuilder()        //This builds upon the standardHelp template for a 'Mods' command help page
            .WithTitle("s!info")
            .WithDescription("``` Shows information about users in the server. ```")
            .WithColor(new Color(RandColor.NewColor()))
            .AddField("***Examples***", "`s!info`\nshows info about you\n`s!info @Hella.#7777`\n" +
                "shows info about Hewwa uwu\n", true)
            .AddField("***Usages***", "`s!info [user]`", true)
            .AddField("***Aliases***", "*__information__*, *__sauce__*");

        private static readonly EmbedBuilder roleHelpBuilder = new EmbedBuilder()
            .WithTitle("s!role")
            .WithDescription("``` Shows information about roles in the server. ```")
            .WithColor(new Color(RandColor.NewColor()))
            .AddField("***Examples***", "`s!role`\nshows info about your roles\n`s!role Moderators`\n" +
                                        "shows info about the Moderators role", true)
            .AddField("***Usages***", "`s!role [role]`", true)
            .AddField("***Aliases***", "*__roleinfo__*, *__rinfo__*");

        private static readonly EmbedBuilder channelHelpBuilder = new EmbedBuilder()
            .WithTitle("s!channel")
            .WithDescription("``` Shows information about channels in the server. ```")
            .WithColor(new Color(RandColor.NewColor()))
            .AddField("***Examples***", "`s!channel`\nshows info about the current channel\n`s!channel rules`\n" +
                                        "shows info about the rules channel", true)
            .AddField("***Usages***", "`s!channel [#channel]", true)
            .AddField("***Aliases***", "*__channelinfo__*, *__cinfo__*");

        private static readonly EmbedBuilder serverHelpBuilder = new EmbedBuilder()
            .WithTitle("s!server")
            .WithDescription("``` Shows information about the server ```")
            .WithColor(new Color(RandColor.NewColor()))
            .AddField("***Examples***", "`s!server`\nshows info about the server", true)
            .AddField("***Usages***", "`s!server`", true)
            .AddField("***Aliases***", "*__serverinfo__*, *__sinfo__*");

        private static readonly EmbedBuilder pingHelpBuilder = new EmbedBuilder()        //This builds upon the standardHelp template for a 'ping' command help page
            .WithTitle("s!ping")
            .WithDescription("``` Displays pong to see if the bot is online. ```")
            .WithColor(new Color(RandColor.NewColor()))
            .AddField("***Examples***", "`s!ping`\nWill display \'Pong!\'\n`s!beep`\nWill display \'Boop!\'" +
                                        "`s!bip`\nWill display \'Bap!\'", true)
            .AddField("***Usages***", "`s!ping`", true)
            .AddField("***Aliases***", "*__beep__*, *__bip__*");

        private static readonly EmbedBuilder hugHelpBuilder = new EmbedBuilder()
            .WithTitle("s!hug")
            .WithDescription("``` Outputs a random hugging gif. ```")
            .WithColor(new Color(RandColor.NewColor()))
            .AddField("***Examples***", "`s!hug`\nWill display hugging gif\n`s!hug @Hella.#7777`\n" +
                                        "Post a gif to hug Hewwa uwu", true)
            .AddField("***Usages***", "`s!hug [user]`", true);

        private static readonly EmbedBuilder lickHelpBuilder = new EmbedBuilder()
            .WithTitle("s!lick")
            .WithDescription("``` Outputs a random licking gif. ```")
            .WithColor(new Color(RandColor.NewColor()))
            .AddField("***Examples***", "`s!lick`\nWill display licking gif\n`s!lick @Hella.#7777`\n" +
                                        "Post a gif to lick Hewwa owo", true)
            .AddField("***Usages***", "`s!lick [user]`", true);
        
        private static readonly EmbedBuilder cuddleHelpBuilder = new EmbedBuilder()
            .WithTitle("s!cuddle")
            .WithDescription("``` Outputs a random cuddle gif. ```")
            .WithColor(new Color(RandColor.NewColor()))
            .AddField("***Examples***", "`s!cuddle`\nWill display cuddle gif\n`s!cuddle @Hella.#7777`\n" +
                                        "Post a gif to cuddle Hewwa owo", true)
            .AddField("***Usages***", "`s!cuddle [user]`", true);
        
        private static readonly EmbedBuilder quoteHelpBuilder = new EmbedBuilder()
            .WithTitle("s!quote")
            .WithDescription("``` Posts a quote from a specified \n user or records a quote for \n later use. ```")
            .WithColor(new Color(RandColor.NewColor()))
            .AddField("***Examples***", "`s!quote @Hella.#7777`\nPosts Hewwa's quote\n`s!quote \"Ok Boomer\"\n" +
                                        "Records the quote as your quote\n`s@quote @Hella.#7777 \"Ok Boomer\"`\n" +
                                        "Records the quote as Hewwa's quote [Mods Only]", true)
            .AddField("***Usages***", "`s!quote [user]`\n`s!quote \"[quote]\"`\n`s@quote [user] \"[quote]\"`", true);
        
        private static readonly EmbedBuilder trapsHelpBuilder = new EmbedBuilder()
            .WithTitle("s!traps")
            .WithDescription("``` Outputs a random trap gif/picture.```")
            .WithColor(new Color(RandColor.NewColor()))
            .AddField("***Examples***", "`s!traps`\nWill display a trap gif/picture", true)
            .AddField("***Usages***", "`s!traps`", true);
        
        private static readonly EmbedBuilder pickleHelpBuilder = new EmbedBuilder()
            .WithTitle("s!pickle")
            .WithDescription("``` Calculates a users pickle size.```")
            .WithColor(new Color(RandColor.NewColor()))
            .AddField("***Examples***", "`s!pickle`\nWill display your pickle size\n`s!pickle @Hella.#7777`\n" +
                                        "Will display Hewwa's humongous pickle rick", true)
            .AddField("***Usages***", "`s!pickle [user]`", true);
        
        private static readonly EmbedBuilder stinkyHelpBuilder = new EmbedBuilder()
            .WithTitle("s!stinky")
            .WithDescription("``` Uh Oh Stinky. ```")
            .WithColor(new Color(RandColor.NewColor()))
            .AddField("***Examples***", "`s!stinky`\nUh Oh Stinky\n`s!stinky @Hella.#7777`\n" +
                                        "Hewwa's a stinky uwu", true)
            .AddField("***Usages***", "`s!stinky [user]`\n`s!stinky`", true);
        
        private static readonly EmbedBuilder triviaHelpBuilder = new EmbedBuilder()
            .WithTitle("s!trivia")
            .WithDescription("``` Posts random trivia. ```")
            .WithColor(new Color(RandColor.NewColor()))
            .AddField("***Examples***", "`s!trivia`\nPosts random trivia", true)
            .AddField("***Usages***", "`s!trivia`", true);
        
        private static readonly EmbedBuilder rollHelpBuilder = new EmbedBuilder()
            .WithTitle("s!roll")
            .WithDescription("``` Role a specified die. ```")
            .WithColor(new Color(RandColor.NewColor()))
            .AddField("***Examples***", "`s!roll 1d6`\nRolls a single d6\n`s!roll 3d20`\n" +
                                        "Rolls three d20 dice", true)
            .AddField("***Usages***", "`s!roll [amount]d[die]", true);
        
        private static readonly EmbedBuilder magicConchHelpBuilder = new EmbedBuilder()
            .WithTitle("s!8ball")
            .WithDescription("``` Play a game of 8ball with by \n asking a question. ```")
            .WithColor(new Color(RandColor.NewColor()))
            .AddField("***Examples***", "`s!8ball Is Hella cool?`\nWill post a random 8ball response", true)
            .AddField("***Usages***", "`s!8ball [statement]", true);

        //Build all the commands upon start
        private static readonly Embed helpEmbed = helpBuilder.Build();
        private static readonly Embed infoHelpEmbed = BuildWithHelpMenuAuthor(infoHelpBuilder);
        private static readonly Embed roleHelpEmbed = BuildWithHelpMenuAuthor(roleHelpBuilder);
        private static readonly Embed channelHelpEmbed = BuildWithHelpMenuAuthor(channelHelpBuilder);
        private static readonly Embed serverHelpEmbed = BuildWithHelpMenuAuthor(serverHelpBuilder);
        private static readonly Embed pingHelpEmbed = BuildWithHelpMenuAuthor(pingHelpBuilder);
        private static readonly Embed hugHelpEmbed = BuildWithHelpMenuAuthor(hugHelpBuilder);
        private static readonly Embed lickHelpEmbed = BuildWithHelpMenuAuthor(lickHelpBuilder);
        private static readonly Embed cuddleHelpEmbed = BuildWithHelpMenuAuthor(cuddleHelpBuilder);
        private static readonly Embed quoteHelpEmbed = BuildWithHelpMenuAuthor(quoteHelpBuilder);
        private static readonly Embed trapsHelpEmbed = BuildWithHelpMenuAuthor(trapsHelpBuilder);
        private static readonly Embed pickleHelpEmbed = BuildWithHelpMenuAuthor(pickleHelpBuilder);
        private static readonly Embed stinkyHelpEmbed = BuildWithHelpMenuAuthor(stinkyHelpBuilder);
        private static readonly Embed triviaHelpEmbed = BuildWithHelpMenuAuthor(triviaHelpBuilder);
        private static readonly Embed rollHelpEmbed = BuildWithHelpMenuAuthor(rollHelpBuilder);
        private static readonly Embed magicConchHelpEmbed = BuildWithHelpMenuAuthor(magicConchHelpBuilder);
        
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
        
        //Sends out the Embedded message
        private async Task EmbedReply(Embed embed)
        {                                                                   
            await ReplyAsync("", false, embed);
        }

        [Command]    //When help is called without arguments
        public async Task Help()
        {
            await EmbedReply(helpEmbed);
        }

        [Command("info")]    //When help is called for the 'mods' help page
        [Alias("information", "sauce")]
        public async Task InfoHelp()
        {
            await EmbedReply(infoHelpEmbed);
        }

        [Command("role")]
        [Alias("roleinfo", "rinfo")]
        public async Task RoleHelp()
        {
            await EmbedReply(roleHelpEmbed);
        }

        [Command("channel")]
        [Alias("channelinfo", "cinfo")]
        public async Task ChannelHelp()
        {
            await EmbedReply(channelHelpEmbed);
        }

        [Command("server")]
        [Alias("serverinfo", "sinfo")]
        public async Task ServerHelp()
        {
            await EmbedReply(serverHelpEmbed);
        }

        [Command("ping")]    //When help is called for the 'ping' help page
        [Alias("beep", "bip")]
        public async Task PingHelp()
        {
            await EmbedReply(pingHelpEmbed);
        }

        [Command("hug")]
        public async Task HugHelp()
        {
            await EmbedReply(hugHelpEmbed);
        }

        [Command("lick")]
        public async Task LickHelp()
        {
            await EmbedReply(lickHelpEmbed);
        }

        [Command("cuddle")]
        public async Task CuddleHelp()
        {
            await EmbedReply(cuddleHelpEmbed);
        }

        [Command("quote")]
        public async Task QuoteHelp()
        {
            await EmbedReply(quoteHelpEmbed);
        }

        [Command("traps")]
        public async Task TrapsHelp()
        {
            await EmbedReply(trapsHelpEmbed);
        }

        [Command("pickle")]
        public async Task PickleHelp()
        {
            await EmbedReply(pickleHelpEmbed);
        }

        [Command("stinky")]
        public async Task StinkyHelp()
        {
            await EmbedReply(stinkyHelpEmbed);
        }

        [Command("trivia")]
        public async Task TriviaHelp()
        {
            await EmbedReply(triviaHelpEmbed);
        }

        [Command("roll")]
        public async Task RollHelp()
        {
            await EmbedReply(rollHelpEmbed);
        }

        [Command("8ball")]
        [Alias("magic-conch")]
        public async Task MagicConchHelp()
        {
            await EmbedReply(magicConchHelpEmbed);
        }
    }

    [RequireRoleAttribute("⚖️Moderators⚖️")]
    [Group("s@help")]
    public class ModHelpModule : ModuleBase<SocketCommandContext>
    {
        private static readonly EmbedBuilder modHelpBuilder = new EmbedBuilder()    //Standard Mod Help embedded message
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
            .AddField("⚖ Moderation Tools", "`kick` `ban` `mute`\n`prune` `blacklist`", true);
            //.AddField("🛠 Settings", "`disable` `enable` `welcome`\n`goodbye` `timezone`\n`nsfw` ", true);

        private static readonly EmbedBuilder kickHelpBuilder = new EmbedBuilder()    //Displays kick help embedded message
            .WithTitle("s@kick")
            .WithDescription("``` Kicks a specified user from the server. \n You may specify a reason. ```")
            .WithColor(new Color(RandColor.NewColor()))
            .AddField("***Examples***", "`s@kick @Hella.#7777`\nKicks Hewwa from the server uwu", true)
            .AddField("***Usages***", "`s@kick [user] [reason]`", true)
            .AddField("***Aliases***", "*__remove__*, *__boot__*, *__snap__*");

        private static readonly EmbedBuilder banHelpBuilder = new EmbedBuilder()    //Displays ban help embedded message
            .WithTitle("s@ban")
            .WithDescription("``` Bans a specified user from the server. \n You may specify a reason. ```")
            .WithColor(new Color(RandColor.NewColor()))
            .AddField("***Examples***", "`s@ban @Hella.#7777`\nBans Hewwa from the server uwu", true)
            .AddField("***Usages***", "`s@ban [user] [reason]`", true)
            .AddField("***Aliases***", "*__outlaw__*, *__banish__*, *__begone__*");

        private static readonly EmbedBuilder muteHelpBuilder = new EmbedBuilder()    //Displays mute help embedded message
            .WithTitle("s@mute")
            .WithDescription("``` Mutes a specified user in the server. \n You may specify a reason. ```")
            .WithColor(new Color(RandColor.NewColor()))
            .AddField("***Examples***", "`s@mute @Hella.#7777`\nMutes Hewwa uwu", true)
            .AddField("***Usages***", "`s@mute [user] [reason]`", true)
            .AddField("***Aliases***", "*__silence__*, *__gag__*, *__muzzle__*");

        private static readonly EmbedBuilder pruneHelpBuilder = new EmbedBuilder() //Displays mute help embedded message
            .WithTitle("s@prune")
            .WithDescription("``` Prunes a specified number of messages from \n " +
                             "the channel. You may specify \n a user, a string, or an entity. ```")
            .WithColor(new Color(RandColor.NewColor()))
            .AddField("***Examples***",
                "`s@prune 20`\nRemoves last 20 messages in channel\n`s@prune 10 @Hella.#7777`\n" +
                "Removes last 20 messages sent by Hella\nin the channel\n`s@prune 5 \"Handholding\"`\n" +
                "Removes last 5 messages with the substring \"Handholding\"\n`s@prune 10 images`\n" +
                "Removes last 10 images posted", true)
            .AddField("***Usages***", "`s@prune [number]`\n`s@prune [number] [user]`\n`s@prune [number] [\"string\"]\n" +
                                      "`s@prune [number] [flag]", true)
            .AddField("***Aliases***", "*none*")
            .AddField("***Flags***", "*__bots__*, *__images__*, *__mentions__*,\n*__links__*", true);
        
        private static readonly EmbedBuilder blacklistHelpBuilder = new EmbedBuilder()
            .WithTitle("s@blacklist")
            .WithDescription("``` Places a specified user onto the \n servers blacklist. ```")
            .WithColor(new Color(RandColor.NewColor()))
            .AddField("***Examples***", "`s@blacklist add 140912052657979392`\nAdds specified user id to blacklist", true)
            .AddField("***Usages***", "`s@blacklist <add/remove/clear> [id]`", true);

        //Builds all the embedded messages for use
        private static readonly Embed modHelpEmbed = modHelpBuilder.Build();
        private static readonly Embed kickHelpEmbed = BuildWithHelpMenuAuthor(kickHelpBuilder);
        private static readonly Embed banHelpEmbed = BuildWithHelpMenuAuthor(banHelpBuilder);
        private static readonly Embed muteHelpEmbed = BuildWithHelpMenuAuthor(muteHelpBuilder);
        private static readonly Embed pruneHelpEmbed = BuildWithHelpMenuAuthor(pruneHelpBuilder);
        private static readonly Embed blacklistHelpEmbed = BuildWithHelpMenuAuthor(blacklistHelpBuilder);
        
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
        
        //Sends out the embedded message
        private async Task EmbedReply(Embed embed)
        {                                                                   
            await ReplyAsync("", false, embed);
        }

        
        [Command]
        public async Task ModHelp()
        {
            await EmbedReply(modHelpEmbed);
        }

        [Command("kick")]
        [Alias("remove", "boot", "snap")]
        public async Task KickHelp()
        {
           await EmbedReply(kickHelpEmbed);
        }

        [Command("ban")]
        [Alias("outlaw", "banish", "begone")]
        public async Task BanHelp()
        {
            await EmbedReply(banHelpEmbed);
        }

        [Command("mute")]
        [Alias("silence", "gag", "muzzle")]
        public async Task MuteHelp()
        {
            await EmbedReply(muteHelpEmbed);
        }

        [Command("prune")]
        public async Task PruneHelp()
        {
            await EmbedReply(pruneHelpEmbed);
        }

        [Command("blacklist")]
        public async Task BlacklistHelp()
        {
            await EmbedReply(blacklistHelpEmbed);
        }
    }
}