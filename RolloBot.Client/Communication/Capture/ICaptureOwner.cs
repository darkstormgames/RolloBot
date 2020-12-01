using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolloBot.Client.Communication.Capture
{
    public interface ICaptureOwner
    {
        VideoCapture VideoCapture { get; }
        Bitmap CurrentImg { get; set; }
        CaptureState CaptureState { get; set; }

        void VideoRec_NewFrame(object sender, CaptureFrameEventArgs e);
    }
}
