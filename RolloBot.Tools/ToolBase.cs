using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RolloBot.Client;
using RolloBot.Client.Communication.Capture;
using RolloBot.Client.Communication;
using RolloBot.Client.Helper.OCR;

namespace RolloBot.Tools
{
    public abstract class ToolBase : DisposableBase
    {
        protected readonly ToolWindowBase mainWindow;

        protected ScreenshotRecognition screenshotRecognition;

        protected long FramesSinceStart = 0;
        protected Bitmap currentFrame;

        private bool isRunning = false;
        public bool IsRunning
        {
            get { return isRunning; }
            set
            {
                if (value != isRunning)
                {
                    isRunning = value;
                    mainWindow.OnPropertyChanged();
                }
            }
        }

        public abstract string Name { get; }
        public abstract string DisplayName { get; }
        public abstract string ToolTip { get; }

        public abstract bool NeedsVideo { get; }
        public abstract bool NeedsSerial { get; }
        public abstract bool NeedsXInput { get; }
        public abstract CaptureExecutionSpeed CaptureExecutionSpeed { get; }
        public abstract ExecutionTrigger ExecutionTrigger { get; }
        public abstract ToolConfigBase Config { get; }

        public ToolBase(ToolWindowBase mainWindow)
        {
            this.mainWindow = mainWindow;
        }


        public abstract void Execute();

        public bool CheckExecute()
        {
            bool canExecute = false;
            switch (CaptureExecutionSpeed)
            {
                case CaptureExecutionSpeed.ReallySlow:
                case CaptureExecutionSpeed.Slow:
                case CaptureExecutionSpeed.Medium:
                case CaptureExecutionSpeed.Fast:
                case CaptureExecutionSpeed.ReallyFast:
                case CaptureExecutionSpeed.RealTime:
                    if (FramesSinceStart % getFrameSkipCount() == 0)
                    {
                        canExecute = CheckExecutionTrigger();
                    }
                    break;

                case CaptureExecutionSpeed.None:
                default:
                    canExecute = CheckExecutionTrigger();
                    break;
            }
            
            return canExecute;
        }

        protected bool CheckExecutionTrigger()
        {
            switch(ExecutionTrigger)
            {
                case ExecutionTrigger.ScreenshotTaken:

                    break;

                case ExecutionTrigger.None:
                default:
                    return true;
            }
            return true;
        }

        private int getFrameSkipCount()
        {
            switch(this.CaptureExecutionSpeed)
            {
                case CaptureExecutionSpeed.ReallySlow: return 60;
                case CaptureExecutionSpeed.Slow: return 30;
                case CaptureExecutionSpeed.Fast: return 15;
                case CaptureExecutionSpeed.ReallyFast: return 2;
                case CaptureExecutionSpeed.RealTime: return 1;

                case CaptureExecutionSpeed.Medium:
                default:
                    return 20;
            }
        }

        public virtual bool Start()
        {
            if (NeedsVideo)
            {
                if (mainWindow.Communication.VideoCapture != null)
                {
                    mainWindow.Communication.VideoCapture.NewFrame += VideoCapture_NewFrame;
                }
                else
                {
                    mainWindow.AddLog(Client.LogType.Error, "Video Capture is not running!");
                    return false;
                }
            }
            if (NeedsSerial)
            {
                if (!mainWindow.Communication.SerialCommunication.Available())
                {
                    mainWindow.AddLog(Client.LogType.Error, "Serial connection is not available!");
                    return false;
                }
            }
            if (NeedsXInput)
            {
                // Check some stuff here...
            }

            this.screenshotRecognition = new ScreenshotRecognition();
            mainWindow.AddLog(Client.LogType.Success, $"Screenshot recognition engine started successfully.");

            this.IsRunning = true;
            mainWindow.AddLog(Client.LogType.Info, $"Starting \"{this.DisplayName}\"...");
            return true;
        }

        public virtual void Stop(bool withError = false)
        {
            if (withError)
            {
                mainWindow.AddLog(Client.LogType.Error, $"Function \"{this.DisplayName}\" stopped, because an error occured.");
            }
            else
            {
                mainWindow.AddLog(Client.LogType.Success, $"Function \"{this.DisplayName}\" stopped successfully.");
            }

            //
            //      Update toolWindow-UI
            //

            this.IsRunning = false;
        }

        protected void VideoCapture_NewFrame(object sender, CaptureFrameEventArgs e)
        {
            this.currentFrame = (Bitmap)e.Frame.Clone();
            if (IsRunning && CheckExecute())
            {
                Execute();
            }
            this.FramesSinceStart++;
        }

    }
}
