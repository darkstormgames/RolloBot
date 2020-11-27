using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RolloBot.Client.Communication.Serial;

namespace RolloBot.Client.Communication.XInput
{
    public class XInputPoller
    {


        public XInputPoller()
        {

        }



        private Command[] CompareControllerStates(GamePadState oldState, GamePadState currentState)
        {
            List<Command> commands = new List<Command>();

            // Check button A
            if (IsCurrentButtonStateDifferent(oldState.Buttons.A, currentState.Buttons.A))
            {
                commands.Add(new Command(Pins.buttonA, currentState.Buttons.A == ButtonState.Pressed ? PinState.HIGH : PinState.LOW));
            }
            // Check button B
            if (IsCurrentButtonStateDifferent(oldState.Buttons.B, currentState.Buttons.B))
            {
                commands.Add(new Command(Pins.buttonB, currentState.Buttons.B == ButtonState.Pressed ? PinState.HIGH : PinState.LOW));
            }
            // Check button X
            if (IsCurrentButtonStateDifferent(oldState.Buttons.X, currentState.Buttons.X))
            {
                commands.Add(new Command(Pins.buttonX, currentState.Buttons.X == ButtonState.Pressed ? PinState.HIGH : PinState.LOW));
            }
            // Check button Y
            if (IsCurrentButtonStateDifferent(oldState.Buttons.Y, currentState.Buttons.Y))
            {
                commands.Add(new Command(Pins.buttonY, currentState.Buttons.Y == ButtonState.Pressed ? PinState.HIGH : PinState.LOW));
            }

            // Check button PLUS
            if (IsCurrentButtonStateDifferent(oldState.Buttons.Start, currentState.Buttons.Start))
            {
                commands.Add(new Command(Pins.buttonPlus, currentState.Buttons.Start == ButtonState.Pressed ? PinState.HIGH : PinState.LOW));
            }
            // Check button MINUS
            if (IsCurrentButtonStateDifferent(oldState.Buttons.Back, currentState.Buttons.Back))
            {
                commands.Add(new Command(Pins.buttonMinus, currentState.Buttons.Back == ButtonState.Pressed ? PinState.HIGH : PinState.LOW));
            }

            // Check button L
            if (IsCurrentButtonStateDifferent(oldState.Buttons.LeftShoulder, currentState.Buttons.LeftShoulder))
            {
                commands.Add(new Command(Pins.buttonL, currentState.Buttons.LeftShoulder == ButtonState.Pressed ? PinState.HIGH : PinState.LOW));
            }
            // Check button R
            if (IsCurrentButtonStateDifferent(oldState.Buttons.RightShoulder, currentState.Buttons.RightShoulder))
            {
                commands.Add(new Command(Pins.buttonR, currentState.Buttons.RightShoulder == ButtonState.Pressed ? PinState.HIGH : PinState.LOW));
            }

            // Check button DPad Up
            if (IsCurrentButtonStateDifferent(oldState.DPad.Up, currentState.DPad.Up))
            {
                commands.Add(new Command(Pins.buttonDUp, currentState.DPad.Up == ButtonState.Pressed ? PinState.HIGH : PinState.LOW));
            }
            // Check button DPad Down
            if (IsCurrentButtonStateDifferent(oldState.DPad.Down, currentState.DPad.Down))
            {
                commands.Add(new Command(Pins.buttonDDown, currentState.DPad.Down == ButtonState.Pressed ? PinState.HIGH : PinState.LOW));
            }
            // Check button DPad Left
            if (IsCurrentButtonStateDifferent(oldState.DPad.Left, currentState.DPad.Left))
            {
                commands.Add(new Command(Pins.buttonDLeft, currentState.DPad.Left == ButtonState.Pressed ? PinState.HIGH : PinState.LOW));
            }
            // Check button DPad Right
            if (IsCurrentButtonStateDifferent(oldState.DPad.Right, currentState.DPad.Right))
            {
                commands.Add(new Command(Pins.buttonDRight, currentState.DPad.Right == ButtonState.Pressed ? PinState.HIGH : PinState.LOW));
            }

            // Check Left Stick X
            if (IsCurrentStickAngleDifferent(oldState.ThumbSticks.Left.X, currentState.ThumbSticks.Left.X))
            {
                commands.Add(new Command(Pins.stickLX, (short)currentState.ThumbSticks.Left.X));
            }
            // Check Left Stick Y
            if (IsCurrentStickAngleDifferent(oldState.ThumbSticks.Left.Y, currentState.ThumbSticks.Left.Y))
            {
                commands.Add(new Command(Pins.stickLY, (short)currentState.ThumbSticks.Left.Y));
            }
            // Check Right Stick X
            if (IsCurrentStickAngleDifferent(oldState.ThumbSticks.Right.X, currentState.ThumbSticks.Right.X))
            {
                commands.Add(new Command(Pins.stickRX, (short)currentState.ThumbSticks.Right.X));
            }
            // Check Right Stick Y
            if (IsCurrentStickAngleDifferent(oldState.ThumbSticks.Right.Y, currentState.ThumbSticks.Right.Y))
            {
                commands.Add(new Command(Pins.stickRY, (short)currentState.ThumbSticks.Right.Y));
            }


            if (commands.Count > 0)
            {
                return commands.ToArray();
            }
            else
            {
                return new Command[0];
            }
        }

        private bool IsCurrentStickAngleDifferent(float oldState, float currentState)
        {
            if (oldState - currentState > 10 || currentState - oldState > 10)
                return true;
            else
                return false;
        }

        private bool IsCurrentButtonStateDifferent(ButtonState oldState, ButtonState currentState)
        {
            if (oldState != currentState)
                return true;
            else
                return false;
        }
    }
}
