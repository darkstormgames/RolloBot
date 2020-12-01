using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolloBot.Client.Communication.XInput
{
    public interface IXInputOwner
    {
        XInputPoller XInputPoller { get; }
        bool IsControllerEnabled { get; set; }
    }
}
