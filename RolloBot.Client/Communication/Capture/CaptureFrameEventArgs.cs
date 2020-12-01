using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolloBot.Client.Communication.Capture
{
    public class CaptureFrameEventArgs : EventArgs
    {
        public Bitmap Frame { get; private set; }

        public CaptureFrameEventArgs(Bitmap image)
        {
            this.Frame = image;
        }
    }
}
