using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

namespace RolloBot.Client.Helper.UserBot
{
    public class UserBot
    {
        private readonly string clientToken;

        public static DiscordSocketClient DiscordClient { get; set; }
        public static CommandHandler Commands { get; set; }

        public UserBot(string clientToken)
        {
            this.clientToken = clientToken;
        }
        public UserBot(string clientToken, Exception ex)
        {
            this.clientToken = clientToken;
        }


        public async Task Start()
        {
            DiscordClient = new DiscordSocketClient(new DiscordSocketConfig
            {
                AlwaysDownloadUsers = true,
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 100
            });

            await DiscordClient.LoginAsync(0, clientToken);
            await DiscordClient.StartAsync();

            Commands = new CommandHandler(DiscordClient);
            await Commands.InstallCommandsAsync();

            await Task.Delay(-1);
        }
        
        public void SendMessage(ulong channelId, string message)
        {
            if (DiscordClient != null)
            {
                IMessageChannel channel = (DiscordClient.GetChannel(channelId) as IMessageChannel);
                if (channel != null)
                {
                    channel.SendMessageAsync(message);
                }
            }
        }
    }
}
