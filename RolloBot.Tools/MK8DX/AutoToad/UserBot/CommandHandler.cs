using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.API;
using Discord.WebSocket;

using RolloBot.Client;

namespace RolloBot.Tools.MK8DX.AutoToad.UserBot
{
    class CommandHandler : DisposableBase
    {
        private DiscordSocketClient client;
        private CommandService Commands;


        public CommandHandler(DiscordSocketClient client)
        {
            Commands = new CommandService(new CommandServiceConfig()
            {
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Info,
                ThrowOnError = false
            });

            this.client = client;
        }

        public async Task InstallCommandsAsync()
        {
            client.MessageReceived += handleCommandAsync;

            Commands.Log += userCommands_Log;
            await Commands.AddModuleAsync<AutoToadCommands>(null);
        }


        private async Task handleCommandAsync(SocketMessage messageParam)
        {
            if (!(messageParam is SocketUserMessage message)) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            var context = new CommandContext(client, message);
            var guildID = context.Guild.Id;

            if (message.Author.Id == client.CurrentUser.Id)
            {
                switch (message.Content.Split(' ')[0])
                {
                    case "Started":
                        await this.command_AutoToadCallback(new CallbackEventArgs(message.Author.Id, context.Guild.Id, message.Channel.Id, CallbackParameter.Started));
                        break;
                    case "Stopped":
                        await this.command_AutoToadCallback(new CallbackEventArgs(message.Author.Id, context.Guild.Id, message.Channel.Id, CallbackParameter.Stopped));
                        break;
                    case "Paused":
                        await this.command_AutoToadCallback(new CallbackEventArgs(message.Author.Id, context.Guild.Id, message.Channel.Id, CallbackParameter.Paused));
                        break;
                    case "Resumed":
                        await this.command_AutoToadCallback(new CallbackEventArgs(message.Author.Id, context.Guild.Id, message.Channel.Id, CallbackParameter.Resumed));
                        break;

                    default:
                        break;
                }
            }
            else
            {
                IResult result = null;
                if ((message.HasStringPrefix("_", ref argPos) || (guildID != 0)
                            || message.HasMentionPrefix(client.CurrentUser, ref argPos))
                            && !message.Author.IsBot)
                {
                    result = await Commands.ExecuteAsync(
                        context: context,
                        argPos: argPos,
                        services: null);
                }
            }
        }

        public event EventHandler<CallbackEventArgs> AutoToadParameterCallback;
        private Task command_AutoToadCallback(CallbackEventArgs e)
        {
            this.AutoToadParameterCallback?.Invoke(this, e);

            return Task.CompletedTask;
        }


        public event EventHandler<LogMessage> LogEvent;
        private Task userCommands_Log(LogMessage arg)
        {
            this.LogEvent?.Invoke(this, arg);

            return Task.CompletedTask;
        }

        protected override void Cleanup()
        {
            Commands = null;
        }
    }
}
