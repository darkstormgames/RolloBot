﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolloBot.Client.Communication.XInput
{
    public enum ButtonsConstants
    {
        DPadUp = 0x00000001,
        DPadDown = 0x00000002,
        DPadLeft = 0x00000004,
        DPadRight = 0x00000008,
        Start = 0x00000010,
        Back = 0x00000020,
        LeftThumb = 0x00000040,
        RightThumb = 0x00000080,
        LeftShoulder = 0x0100,
        RightShoulder = 0x0200,
        Guide = 0x0400,
        A = 0x1000,
        B = 0x2000,
        X = 0x4000,
        Y = 0x8000
    }
}
