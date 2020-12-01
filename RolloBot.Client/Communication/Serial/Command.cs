using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolloBot.Client.Communication.Serial
{
    public class Command
    {
        public bool isDigitalPin { get; private set; }
        public bool isStickAxis { get; private set; }

        public Pins Pin { get; private set; }
        public PinState State { get; private set; }
        public short Value { get; private set; }

        /// <summary>
        /// Empty Command (usually a delay)
        /// </summary>
        public Command()
        {
            isDigitalPin = false;
            isStickAxis = false;
            Pin = Pins.NONE;
            State = PinState.LOW;
            Value = -1;
        }

        /// <summary>
        /// Command for digital pins/controller buttons
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="state"></param>
        public Command(Pins pin, PinState state)
        {
            isDigitalPin = true;
            isStickAxis = false;

            Pin = pin;
            State = state;
            Value = -1;
        }

        /// <summary>
        /// Command for joysticks
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="value"></param>
        public Command(Pins pin, short value)
        {
            isDigitalPin = false;
            isStickAxis = true;

            Pin = pin;
            State = PinState.LOW;
            Value = value;
        }

        public override string ToString()
        {
            if (!isDigitalPin && !isStickAxis)
            {
                return "Delay (1ms)";
            }
            else if (isDigitalPin && !isStickAxis)
            {
                return string.Format("{0} | {1}", Pin.ToString(), State.ToString());
            }
            else if (!isDigitalPin && isStickAxis)
            {
                return string.Format("{0} | {1}", Pin.ToString(), Value);
            }
            else
            {
                return "Delay (1ms)";
            }

        }
    }
}
