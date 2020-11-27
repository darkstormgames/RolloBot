using System;
using System.Runtime.InteropServices;

namespace RolloBot.Client.Communication.XInput
{
    class NativeMethods
    {
        internal const string DLLName = "XInputInterface";

        [DllImport(DLLName)]
        public static extern uint XInputGamePadGetState(uint playerIndex, out GamePadState.RawState state);
        [DllImport(DLLName)]
        public static extern void XInputGamePadSetState(uint playerIndex, float leftMotor, float rightMotor);
    }

    public enum ButtonState
    {
        Pressed,
        Released
    }

    public struct GamePadButtons
    {
        internal GamePadButtons(ButtonState start, ButtonState back, ButtonState leftStick, ButtonState rightStick,
                                ButtonState leftShoulder, ButtonState rightShoulder, ButtonState guide,
                                ButtonState a, ButtonState b, ButtonState x, ButtonState y)
        {
            this.Start = start;
            this.Back = back;
            this.LeftStick = leftStick;
            this.RightStick = rightStick;
            this.LeftShoulder = leftShoulder;
            this.RightShoulder = rightShoulder;
            this.Guide = guide;
            this.A = a;
            this.B = b;
            this.X = x;
            this.Y = y;
        }

        public ButtonState Start { get; }

        public ButtonState Back { get; }

        public ButtonState LeftStick { get; }

        public ButtonState RightStick { get; }

        public ButtonState LeftShoulder { get; }

        public ButtonState RightShoulder { get; }

        public ButtonState Guide { get; }

        public ButtonState A { get; }

        public ButtonState B { get; }

        public ButtonState X { get; }

        public ButtonState Y { get; }
    }

    public struct GamePadDPad
    {
        internal GamePadDPad(ButtonState up, ButtonState down, ButtonState left, ButtonState right)
        {
            this.Up = up;
            this.Down = down;
            this.Left = left;
            this.Right = right;
        }

        public ButtonState Up { get; }

        public ButtonState Down { get; }

        public ButtonState Left { get; }

        public ButtonState Right { get; }
    }

    public struct GamePadThumbSticks
    {
        public struct StickValue
        {
            internal StickValue(float x, float y)
            {
                this.X = x;
                this.Y = y;
            }

            public float X { get; }

            public float Y { get; }
        }

        internal GamePadThumbSticks(StickValue left, StickValue right)
        {
            this.Left = left;
            this.Right = right;
        }

        public StickValue Left { get; }

        public StickValue Right { get; }
    }

    public struct GamePadTriggers
    {
        internal GamePadTriggers(float left, float right)
        {
            this.Left = left;
            this.Right = right;
        }

        public float Left { get; }

        public float Right { get; }
    }

    public struct GamePadState
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct RawState
        {
            public uint dwPacketNumber;
            public GamePad Gamepad;

            [StructLayout(LayoutKind.Sequential)]
            public struct GamePad
            {
                public ushort wButtons;
                public byte bLeftTrigger;
                public byte bRightTrigger;
                public short sThumbLX;
                public short sThumbLY;
                public short sThumbRX;
                public short sThumbRY;
            }
        }



        internal GamePadState(bool isConnected, RawState rawState, GamePadDeadZone deadZone)
        {
            this.IsConnected = isConnected;

            if (!isConnected)
            {
                rawState.dwPacketNumber = 0;
                rawState.Gamepad.wButtons = 0;
                rawState.Gamepad.bLeftTrigger = 0;
                rawState.Gamepad.bRightTrigger = 0;
                rawState.Gamepad.sThumbLX = 0;
                rawState.Gamepad.sThumbLY = 0;
                rawState.Gamepad.sThumbRX = 0;
                rawState.Gamepad.sThumbRY = 0;
            }

            PacketNumber = rawState.dwPacketNumber;
            Buttons = new GamePadButtons(
                (rawState.Gamepad.wButtons & (uint)ButtonsConstants.Start) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.wButtons & (uint)ButtonsConstants.Back) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.wButtons & (uint)ButtonsConstants.LeftThumb) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.wButtons & (uint)ButtonsConstants.RightThumb) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.wButtons & (uint)ButtonsConstants.LeftShoulder) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.wButtons & (uint)ButtonsConstants.RightShoulder) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.wButtons & (uint)ButtonsConstants.Guide) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.wButtons & (uint)ButtonsConstants.A) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.wButtons & (uint)ButtonsConstants.B) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.wButtons & (uint)ButtonsConstants.X) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.wButtons & (uint)ButtonsConstants.Y) != 0 ? ButtonState.Pressed : ButtonState.Released
            );
            DPad = new GamePadDPad(
                (rawState.Gamepad.wButtons & (uint)ButtonsConstants.DPadUp) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.wButtons & (uint)ButtonsConstants.DPadDown) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.wButtons & (uint)ButtonsConstants.DPadLeft) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.wButtons & (uint)ButtonsConstants.DPadRight) != 0 ? ButtonState.Pressed : ButtonState.Released
            );

            ThumbSticks = new GamePadThumbSticks(
                Utils.ApplyLeftStickDeadZone(rawState.Gamepad.sThumbLX, rawState.Gamepad.sThumbLY, deadZone),
                Utils.ApplyRightStickDeadZone(rawState.Gamepad.sThumbRX, rawState.Gamepad.sThumbRY, deadZone)
            );
            Triggers = new GamePadTriggers(
                Utils.ApplyTriggerDeadZone(rawState.Gamepad.bLeftTrigger, deadZone),
                Utils.ApplyTriggerDeadZone(rawState.Gamepad.bRightTrigger, deadZone)
            );
        }

        public uint PacketNumber { get; }

        public bool IsConnected { get; }

        public GamePadButtons Buttons { get; }

        public GamePadDPad DPad { get; }

        public GamePadTriggers Triggers { get; }

        public GamePadThumbSticks ThumbSticks { get; }
    }

    public enum PlayerIndex
    {
        One = 0,
        Two,
        Three,
        Four
    }

    public enum GamePadDeadZone
    {
        Circular,
        IndependentAxes,
        None
    }

    public class GamePad
    {
        public static GamePadState GetState(PlayerIndex playerIndex)
        {
            return GetState(playerIndex, GamePadDeadZone.IndependentAxes);
        }

        public static GamePadState GetState(PlayerIndex playerIndex, GamePadDeadZone deadZone)
        {
            uint result = NativeMethods.XInputGamePadGetState((uint)playerIndex, out GamePadState.RawState state);
            return new GamePadState(result == Utils.Success, state, deadZone);
        }

        public static void SetVibration(PlayerIndex playerIndex, float leftMotor, float rightMotor)
        {
            NativeMethods.XInputGamePadSetState((uint)playerIndex, leftMotor, rightMotor);
        }
    }
}
