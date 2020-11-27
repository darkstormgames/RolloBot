using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolloBot.Tools.MK8DX.AutoToad.UserBot
{
    internal enum CallbackParameter
    {
        Started,
        Stopped,
        Paused,
        Resumed
    }

    internal class CallbackEventArgs : EventArgs
    {
        public ulong UserId { get; private set; }
        public ulong GuildId { get; private set; }
        public ulong ChannelId { get; private set; }
        public CallbackParameter Parameter { get; private set; }

        public CallbackEventArgs(ulong userId, ulong guildId, ulong channelId, CallbackParameter parameter)
        {
            this.UserId = userId;
            this.GuildId = guildId;
            this.ChannelId = channelId;
            this.Parameter = parameter;
        }
    }
}
