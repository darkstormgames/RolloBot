using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RolloBot.Client.Communication.Capture;
using RolloBot.Client.Communication.Serial;
using RolloBot.Client.Communication.XInput;

namespace RolloBot.Client.Communication
{
    public interface ICommunicationsOwner : ICaptureOwner, ISerialOwner, IXInputOwner
    {
        bool IsPreviewPaused { get; set; }
    }
}
