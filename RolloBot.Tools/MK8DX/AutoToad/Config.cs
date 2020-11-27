using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolloBot.Tools.MK8DX.AutoToad
{
    public class Config : ToolConfigBase
    {
        public Config(ToolWindowBase mainWindow) : base(mainWindow)
        {
            this.iniFile = new Client.Configuration.IniFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\darkstormgames\RolloBot\", "AutoToad.ini");

            this.ConfigItems = new System.Collections.ObjectModel.ObservableCollection<ToolConfigItem>()
            {
                new ToolConfigItem(this)
                {
                    DisplayName = "Discord User Token",
                    Name = "DiscordUserToken",
                    ToolTip = "",
                    DefaultValue = "",
                    Type = typeof(string)
                },new ToolConfigItem(this)
                {
                    DisplayName = "Clan-Tag",
                    Name = "ClanTag",
                    ToolTip = "Only edit this value in the AutoToad.ini-file in the application folder!!!\n\nSeparate different variations with \"|\".\nDon´t use spaces, unless it´s part of the tag!",
                    DefaultValue = "モメ",
                    Type = typeof(string)
                },
                new ToolConfigItem(this)
                {
                    DisplayName = "Default Channel-ID",
                    Name = "DefaultChannel",
                    ToolTip = "",
                    DefaultValue = 0,
                    Type = typeof(ulong)
                },
                new ToolConfigItem(this)
                {
                    DisplayName = "Require Manual Start",
                    Name = "RequireManualStart",
                    ToolTip = "Channel-ID has to be set, if this is unchecked!",
                    DefaultValue = true,
                    Type = typeof(bool)
                },
                new ToolConfigItem(this)
                {
                    DisplayName = "Send TrackTable-Commands",
                    Name = "UseTrackTableBot",
                    ToolTip = "",
                    DefaultValue = false,
                    Type = typeof(bool)
                },
                new ToolConfigItem(this)
                {
                    DisplayName = "Send ToadV2-TableTracker-Commands",
                    Name = "UseToadV2Tracks",
                    ToolTip = "The table tracker is currently disabled for ToadV2-Bot...\nHopefully, it will come back soon...",
                    DefaultValue = false,
                    Type = typeof(bool)
                },
            };

            base.loadOrDefault();
        }
    }
}
