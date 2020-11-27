using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using RolloBot.Client.Helper;
using RolloBot.Client.Helper.GameObjects;
using RolloBot.Client.Helper.OCR;

namespace RolloBot.Tools.MK8DX.AutoToad
{
    public class AutoToad : ToolBase
    {
        private UserBot.UserBot userBot;
        private TrackRecognition trackRecognition;
        private BinaryRecognition recognitionEngine;

        public override string Name => "AutoToad";
        public override string DisplayName => "Automatic ToadV2 Bot";
        public override string ToolTip => "Don´t use this with your main account, as there is a high chance the account will get banned eventually.";
        public override string RunButtonText => "Run AutoToad";
        public override string StopButtonText => "Stop AutoToad";
        public override bool NeedsVideo => true;
        public override bool NeedsSerial => false;
        public override bool NeedsXInput => false;
        public override CaptureExecutionSpeed CaptureExecutionSpeed => CaptureExecutionSpeed.Fast;
        public override ExecutionTrigger ExecutionTrigger => ExecutionTrigger.None;
        public override ToolConfigBase Config => new Config(mainWindow);

        public AutoToad(ToolWindowBase mainWindow) : base(mainWindow)
        {
        }

        public override bool Start()
        {
            bool OK = base.Start();
            if (!OK) return false;

            string userToken = (string)Config["DiscordUserToken"].Value;

            if (string.IsNullOrEmpty(userToken))
            {
                mainWindow.AddLog(Client.LogType.Error, $"No token for the userbot found!");
                return false;
            }
            else
                userBot = new UserBot.UserBot(userToken);

            Task.Run(async () => await userBot.Start());
            
            Task.Run(async () =>
            {
                while (UserBot.UserBot.DiscordClient == null)
                    await Task.Delay(5);

                UserBot.UserBot.DiscordClient.Log += discordClient_Log;

                while (UserBot.UserBot.Commands == null)
                    await Task.Delay(5);

                UserBot.UserBot.Commands.AutoToadParameterCallback += commands_AutoToadParameterCallback;

                mainWindow.AddLog(Client.LogType.Success, $"UserBot started successfully.");
            });

            this.trackRecognition = new TrackRecognition();
            mainWindow.AddLog(Client.LogType.Success, $"Track recognition engine started successfully.");

            this.recognitionEngine = new BinaryRecognition();
            mainWindow.AddLog(Client.LogType.Success, $"Binary recognition engine started successfully.");

            if (Convert.ToUInt64(Config["DefaultChannel"].Value) != 0)
            {
                channelId = Convert.ToUInt64(Config["DefaultChannel"].Value);
            }
            
            if (!(bool)Config["RequireManualStart"].Value && Convert.ToUInt64(Config["DefaultChannel"].Value) != 0)
            {
                mainWindow.AddLog(Client.LogType.Info, $"AutoToad started automatically.");
                isActivated = true;
            }
            else
            {
                mainWindow.AddLog(Client.LogType.Info, $"Waiting for manual activation of AutoToad.");
            }

            return true;
        }

        public override void Stop(bool withError = false)
        {
            UserBot.UserBot.DiscordClient.LogoutAsync();
            this.userBot = null;

            base.Stop(withError);
        }

        private Task discordClient_Log(Discord.LogMessage arg)
        {
            switch (arg.Severity)
            {
                case Discord.LogSeverity.Info:
                    this.mainWindow.AddLog(Client.LogType.Info, arg.Message);
                    break;
                case Discord.LogSeverity.Error:
                    this.mainWindow.AddLog(Client.LogType.Error, arg.Message);
                    break;
                case Discord.LogSeverity.Warning:
                    this.mainWindow.AddLog(Client.LogType.Warning, arg.Message);
                    break;
                case Discord.LogSeverity.Critical:
                    this.mainWindow.AddLog(Client.LogType.Critical, arg.Message);
                    break;
            }
            return Task.CompletedTask;
        }

        private void commands_AutoToadParameterCallback(object sender, UserBot.CallbackEventArgs e)
        {
            switch (e.Parameter)
            {
                case UserBot.CallbackParameter.Started:
                    channelId = e.ChannelId;
                    isActivated = true;
                    mainWindow.AddLog(Client.LogType.Success, "Manual start of AutoToad successful");
                    break;
                case UserBot.CallbackParameter.Paused:
                    isPaused = true;
                    mainWindow.AddLog(Client.LogType.Info, "AutoToad paused...");
                    break;
                case UserBot.CallbackParameter.Resumed:
                    isPaused = false;
                    mainWindow.AddLog(Client.LogType.Info, "AutoToad resumed...");
                    break;
                case UserBot.CallbackParameter.Stopped:
                    isActivated = false;
                    this.Stop();
                    mainWindow.AddLog(Client.LogType.Info, "AutoToad stopped...");
                    break;
            }
        }

        private bool isActivated = false;
        private bool isPaused = false;
        private bool isWorking = false;
        private ulong channelId = ulong.MinValue;
        private DateTime timeoutEnd = DateTime.Now;
        public override void Execute()
        {
            if (isActivated && !isPaused && !isWorking && DateTime.Now >= timeoutEnd)
            {
                System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                isWorking = true;
                Bitmap img = ((Bitmap)this.currentFrame.Clone()).Resize(1280, 720);

                // Check, if current frame is track-screen
                if ((bool)Config["UseTrackTableBot"].Value && trackRecognition.IsTrack(img))
                {
                    mainWindow.AddLog(Client.LogType.Success, "current Frame is a Track-image!");
                    MK8DTrack track = trackRecognition.GetTrack(img);
                    if (track != null && channelId != ulong.MinValue)
                    {
                        mainWindow.AddLog(Client.LogType.Success, $"Selected track is {track.NameENG} ({track.GetAbbreviation()})");
                        //userBot.SendMessage(channelId, $"_addtrack {track.GetAbbreviation()}");
                        userBot.SendMessage(channelId, $"_tt {track.GetAbbreviation()}");
                        timeoutEnd = DateTime.Now.AddSeconds(15);
                    }
                    watch.Stop();
                    mainWindow.AddLog(Client.LogType.Info, $"Execution took {watch.ElapsedMilliseconds}ms");
                }
                else if (screenshotRecognition.IsScreenshot(img))
                {
                    mainWindow.AddLog(Client.LogType.Success, "Screenshot detected.");

                    CachedBitmap[] tagImages = img.ExtractPlayerTags();
                    Dictionary<int, string> tags = new Dictionary<int, string>();
                    int clanCount = 0;
                    for (int i = 0; i < 12; i++)
                    {
                        tags.Add((i + 1), recognitionEngine.RecognizePlayer(tagImages[i]));
                        //foreach (string tag in ((string)Config["ClanTag"].Value).Split('|'))
                        //{
                            if (tags[i + 1].Contains((string)Config["ClanTag"].Value))
                            {
                                clanCount++;
                                //break;
                            }
                        //}
                    }

                    if (clanCount == 6)
                    {
                        timeoutEnd = DateTime.Now.AddSeconds(7);
                        string output = "_race ";
                        for (int i = 1; i <= 12; i++)
                        {
                            //foreach (string tag in ((string)Config["ClanTag"].Value).Split('|'))
                            //{
                                if (tags[i].Contains((string)Config["ClanTag"].Value))
                                {
                                    output += i + " ";
                                    //break;
                                }
                            //}
                        }
                        userBot.SendMessage(channelId, output);
                        mainWindow.AddLog(Client.LogType.Success, $"Command \"{output}\" sent.");
                    }
                    else
                    {
                        mainWindow.AddLog(Client.LogType.Warning, $"Recognition of players failed! Manual input required...");
                        userBot.SendMessage(channelId, "Couldn´t read player-tags! Please type the race-command manually...");
                    }
                    watch.Stop();
                    mainWindow.AddLog(Client.LogType.Info, $"Execution took {watch.ElapsedMilliseconds}ms");
                }

                
            }
            isWorking = false;
        }

        protected override void Cleanup()
        {
            trackRecognition.Dispose();
            screenshotRecognition.Dispose();
            //recognitionEngine.Dispose();
            recognitionEngine = null;
            UserBot.UserBot.DiscordClient.Dispose();
            UserBot.UserBot.Commands.Dispose();
        }
    }
}
