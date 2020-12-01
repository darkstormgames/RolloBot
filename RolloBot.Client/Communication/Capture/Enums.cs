using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolloBot.Client.Communication.Capture
{
    public enum FPS
    {
        _60 = 9001,
        _30 = 0,
        _20,
        _15,
        _12,
        _10,
        _7_5,
        _6,
        _5,
        _4,
        _3,
        _2,
        _1
    }

    public enum CaptureState
    {
        Stopped = 0,
        Running = 1,
        Paused = 2,

        None = 99
    }
}
