using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolloBot.Client.Communication.Serial
{
    public enum PinState : byte
    {
        LOW = 0,
        HIGH = 1
    }

    public enum Pins : byte
    {
        // Communication PINS
        RX0 = 0,
        TX0 = 1,
        TX3 = 14,
        RX3 = 15,
        TX2 = 16,
        RX2 = 17,
        TX1 = 18,
        RX1 = 19,
        SDA = 20,
        SCL = 21,

        // PWM PINS
        pwmReserve02 = 2,
        pwmReserve03 = 3,
        pwmReserve04 = 4,
        stickLX = 5,
        stickLY = 6,
        stickRX = 7,
        stickRY = 8,
        pwmReserve09 = 9,
        pwmReserve10 = 10,
        pwmReserve11 = 11,
        resetPIN = 12,
        onboardLED = 13,

        // Digital PINS
        buttonA = 22,
        buttonY = 23,
        buttonX = 24,
        buttonB = 25,
        buttonHome = 26,
        buttonPlus = 27,
        buttonCapture = 28,
        buttonMinus = 29,
        buttonDLeft = 30,
        buttonDDown = 31,
        buttonDRight = 32,
        buttonDUp = 33,
        buttonR = 34,
        buttonL = 35,
        buttonZL = 36,
        buttonZR = 37,
        buttonLStick = 38,
        buttonRStick = 39,
        lcdRS = 40,
        lcdEnable = 41,
        lcdD4 = 42,
        lcdD5 = 43,
        lcdD6 = 44,
        lcdD7 = 45,
        input8 = 46,
        input7 = 47,
        input5 = 48,
        input6 = 49,
        input3 = 50,
        input4 = 51,
        input2 = 52,
        input1 = 53,

        NONE = 99,
    }
}
