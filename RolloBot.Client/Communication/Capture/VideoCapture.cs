using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge.Video;
using AForge.Video.DirectShow;

namespace RolloBot.Client.Communication.Capture
{
    public class VideoCapture
    {
        private readonly VideoCaptureDevice device;
        private readonly int saveEveryXFrame;

        public long FramesReceived { get; private set; }
        public long FramesOutput { get; private set; }
        public string SourceResolution { get; private set; }

        #region Constructors
        /// <summary>
        /// Default FPS are 15
        /// </summary>
        /// <param name="deviceIndex"></param>
        public VideoCapture(int deviceIndex) : this(deviceIndex, FPS._15) { }

        public VideoCapture(int deviceIndex, FPS framesPerSecond)
        {
            switch (framesPerSecond)
            {
                case FPS._1: this.saveEveryXFrame = 60; break;
                case FPS._2: this.saveEveryXFrame = 30; break;
                case FPS._3: this.saveEveryXFrame = 20; break;
                case FPS._4: this.saveEveryXFrame = 15; break;
                case FPS._5: this.saveEveryXFrame = 12; break;
                case FPS._6: this.saveEveryXFrame = 10; break;
                case FPS._7_5: this.saveEveryXFrame = 8; break;
                case FPS._10: this.saveEveryXFrame = 6; break;
                case FPS._12: this.saveEveryXFrame = 5; break;
                case FPS._15: this.saveEveryXFrame = 4; break;
                case FPS._20: this.saveEveryXFrame = 3; break;
                case FPS._30: this.saveEveryXFrame = 2; break;
                case FPS._60: this.saveEveryXFrame = 1; break;
            }

            FilterInfoCollection camCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            device = new VideoCaptureDevice(camCollection[deviceIndex].MonikerString);
            device.NewFrame += new NewFrameEventHandler(device_NewFrame);
            device.Start();
        }
        #endregion

        #region Events
        public event EventHandler<CaptureFrameEventArgs> NewFrame;
        private void onNewFrame(CaptureFrameEventArgs e)
        {
            NewFrame?.Invoke(this, e);
        }
        
        void device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (FramesReceived % saveEveryXFrame == 0)
            {
                onNewFrame(new CaptureFrameEventArgs(eventArgs.Frame));

                if (FramesOutput % (60 / saveEveryXFrame) == 0)
                    this.SourceResolution = string.Format("{0}x{1}", eventArgs.Frame.Width, eventArgs.Frame.Height);
                
                FramesOutput++;
            }

            FramesReceived++;
        }
        #endregion

        public void Stop()
        {
            device.Stop();
        }
    }
}
