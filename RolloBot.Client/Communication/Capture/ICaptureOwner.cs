using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolloBot.Client.Communication.Capture
{
    public interface ICaptureOwner
    {
        VideoCapture VideoCapture { get; }
    }
}
