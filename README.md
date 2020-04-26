# Sub-Bot

## The one bot to do your every desire
My plan with Sub-Bot is to create a Discord.NET based bot client that houses various commands and ideas from popular bots, 
as well as plenty of ideas of my own. I want it to do it all.

## Versions

### v0.6 (Future) TO DO LIST
    - Music integration
    
### v0.5 (Future) TO DO LIST
    - Mod strike commands for keeping track of user issues
    - Ability to change prefix
    - Ability to set channel id's for various channels like welcome, birthday, command log, etc. for bots use in events

### v0.4 (In Progress) TO DO LIST
    - Finish social commands
    - Finish most meme commands
    - Ask for requests on SClass
    - Provide more gifs

### v0.3
    - Mod command log complete
        - While a command channel is specified in the JSON file, when a mod command is used it will be logged in this channel
        - It will generate a log file for each mod command event
    - Reduced and refactored redundant code throughout program
    - Some Meme commands complete
        - Stinky is similar to a social command, but it shows how stinky someone is
        - Pickle determines a user's pickle size, a special easter egg awaits for someone who has a 0cm pickle

### v0.2
    - Basic Mod commands complete
        - Kick and Ban command available
        - Mute a user for a specified amount of minutes
        - Prune messages by count, a user, or a flag
        - Checks for user priveleges and denies access if priveleges are missing
    - Help page updated
    - Some Social commands complete
        - Hug, poke, laugh, alone, dance, slap, and cuddle users (or yourself you loner)
        - Generates a random gif each time corresponding to the command topic, repeats reduce as gif count increases
    - Embeds generate a random pastel based color for side bar

### v0.1
    - Man and help pages for each command, as well as a general help command to show all available commands
    - Basic ping commands for checking when the bot is live
    - JSON file parsing for common objects such as:
        - command prefixes (both standard and mod prefixes)
        - Bot Client token
        - Channel ID's for various pages like Welcome, Birthday, and a Command Log channel
    - Welcome page events
    - User kicked, banned, and leave events