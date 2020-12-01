using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;

namespace RolloBot.Client.Helper.UserBot
{
    class AutoToadCommands : ModuleBase<CommandContext>
    {
        [Command("autotoad")]
        [Summary("Starts, Stops, Pauses and Resumes the auto toad.")]
        //[Alias("init", "initialize")]
        public async Task Initialize([Summary("Possible actions: Start, Stop, Pause, Resume")]string action)
        {
            System.Console.WriteLine("|| " + action + " ||");
            switch (action.ToLower())
            {
                case "start":
                    await ReplyAsync("Started AutoToad...");
                    break;
                case "stop":
                    await ReplyAsync("Stopped AutoToad...");
                    break;
                case "pause":
                    await ReplyAsync("Paused AutoToad...");
                    break;
                case "resume":
                    await ReplyAsync("Resumed AutoToad...");
                    break;
                default:
                    await ReplyAsync("Action is not valid...\nPossible actions are:\n> Start\n> Stop\n> Pause\n> Resume");
                    break;
            }
        }
    }
}
