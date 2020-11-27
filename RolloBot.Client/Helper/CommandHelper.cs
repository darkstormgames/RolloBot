using System.Text;

using RolloBot.Client.Communication.Serial;

namespace RolloBot.Client.Helper
{
    public static class CommandHelper
    {
        public static string ToCommandString(Command[] commands)
        {
            string output = string.Empty;

            foreach (Command command in commands)
            {
                if (!string.IsNullOrEmpty(output))
                {
                    output += "|";
                }

                if (command.isDigitalPin)
                {
                    output += string.Format("B{0}S{1}", (byte)command.Pin, (byte)command.State);
                }
                else
                {
                    output += string.Format("B{0}S{1}", (byte)command.Pin, (short)command.Value);
                }
            }

            return output;
        }

        public static int WaituS(int uS)
        {
            System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
            int loop_count = 0;
            long start_ticks = watch.ElapsedTicks; // ticks are about 1.92Mhz
            long ticks_needed = start_ticks + uS;
            while (watch.ElapsedTicks < (start_ticks + ticks_needed))
            {
                loop_count++;
            }
            return loop_count;
        }

        public static Command CharToCommand(char signal)
        {
            switch (signal)
            {
                case '-':
                    return new Command();

                case 'A':
                    return new Command(Pins.buttonA, PinState.HIGH);
                case 'a':
                    return new Command(Pins.buttonA, PinState.LOW);
                case 'B':
                    return new Command(Pins.buttonB, PinState.HIGH);
                case 'b':
                    return new Command(Pins.buttonB, PinState.LOW);
                case 'X':
                    return new Command(Pins.buttonX, PinState.HIGH);
                case 'x':
                    return new Command(Pins.buttonX, PinState.LOW);
                case 'Y':
                    return new Command(Pins.buttonY, PinState.HIGH);
                case 'y':
                    return new Command(Pins.buttonY, PinState.LOW);

                case 'L':
                    return new Command(Pins.buttonL, PinState.HIGH);
                case 'l':
                    return new Command(Pins.buttonL, PinState.LOW);
                case 'R':
                    return new Command(Pins.buttonR, PinState.HIGH);
                case 'r':
                    return new Command(Pins.buttonR, PinState.LOW);

                case 'P':
                    return new Command(Pins.buttonPlus, PinState.HIGH);
                case 'p':
                    return new Command(Pins.buttonPlus, PinState.LOW);
                case 'M':
                    return new Command(Pins.buttonMinus, PinState.HIGH);
                case 'm':
                    return new Command(Pins.buttonMinus, PinState.LOW);

                case 'W':
                    return new Command(Pins.buttonDUp, PinState.HIGH);
                case 'w':
                    return new Command(Pins.buttonDUp, PinState.LOW);
                case 'Q':
                    return new Command(Pins.buttonDLeft, PinState.HIGH);
                case 'q':
                    return new Command(Pins.buttonDLeft, PinState.LOW);
                case 'S':
                    return new Command(Pins.buttonDDown, PinState.HIGH);
                case 's':
                    return new Command(Pins.buttonDDown, PinState.LOW);
                case 'E':
                    return new Command(Pins.buttonDRight, PinState.HIGH);
                case 'e':
                    return new Command(Pins.buttonDRight, PinState.LOW);

                case 'H':
                    return new Command(Pins.buttonHome, PinState.HIGH);
                case 'h':
                    return new Command(Pins.buttonHome, PinState.LOW);
                case 'C':
                    return new Command(Pins.buttonCapture, PinState.HIGH);
                case 'c':
                    return new Command(Pins.buttonCapture, PinState.LOW);


            }

            return new Command();
        }

        public static byte ToByte(this Command command)
        {
            switch (command.Pin)
            {
                case Pins.buttonA:
                    if (command.State == PinState.HIGH)
                    {
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }
                case Pins.buttonB:
                    if (command.State == PinState.HIGH)
                    {
                        return 3;
                    }
                    else
                    {
                        return 4;
                    }
                case Pins.buttonX:
                    if (command.State == PinState.HIGH)
                    {
                        return 5;
                    }
                    else
                    {
                        return 6;
                    }
                case Pins.buttonY:
                    if (command.State == PinState.HIGH)
                    {
                        return 7;
                    }
                    else
                    {
                        return 8;
                    }

                case Pins.buttonL:
                    if (command.State == PinState.HIGH)
                    {
                        return 9;
                    }
                    else
                    {
                        return 10;
                    }
                case Pins.buttonR:
                    if (command.State == PinState.HIGH)
                    {
                        return 11;
                    }
                    else
                    {
                        return 12;
                    }
                case Pins.buttonZL:
                    if (command.State == PinState.HIGH)
                    {
                        return 13;
                    }
                    else
                    {
                        return 14;
                    }
                case Pins.buttonZR:
                    if (command.State == PinState.HIGH)
                    {
                        return 15;
                    }
                    else
                    {
                        return 16;
                    }

                case Pins.buttonPlus:
                    if (command.State == PinState.HIGH)
                    {
                        return 17;
                    }
                    else
                    {
                        return 18;
                    }
                case Pins.buttonMinus:
                    if (command.State == PinState.HIGH)
                    {
                        return 19;
                    }
                    else
                    {
                        return 20;
                    }

                case Pins.buttonDUp:
                    if (command.State == PinState.HIGH)
                    {
                        return 21;
                    }
                    else
                    {
                        return 22;
                    }
                case Pins.buttonDLeft:
                    if (command.State == PinState.HIGH)
                    {
                        return 23;
                    }
                    else
                    {
                        return 24;
                    }
                case Pins.buttonDDown:
                    if (command.State == PinState.HIGH)
                    {
                        return 25;
                    }
                    else
                    {
                        return 26;
                    }
                case Pins.buttonDRight:
                    if (command.State == PinState.HIGH)
                    {
                        return 27;
                    }
                    else
                    {
                        return 28;
                    }

                case Pins.buttonHome:
                    if (command.State == PinState.HIGH)
                    {
                        return 29;
                    }
                    else
                    {
                        return 30;
                    }
                case Pins.buttonCapture:
                    if (command.State == PinState.HIGH)
                    {
                        return 31;
                    }
                    else
                    {
                        return 32;
                    }

                case Pins.buttonLStick:
                    if (command.State == PinState.HIGH)
                    {
                        return 33;
                    }
                    else
                    {
                        return 34;
                    }
                case Pins.buttonRStick:
                    if (command.State == PinState.HIGH)
                    {
                        return 35;
                    }
                    else
                    {
                        return 36;
                    }

                // Range for Sticks: 40-231
                case Pins.stickLX: // 40-81
                    return (byte)((command.Value / 100) + 40);
                case Pins.stickLY: // 90-131
                    return (byte)((command.Value / 100) + 90);
                case Pins.stickRX: // 140-181
                    return (byte)((command.Value / 100) + 140);
                case Pins.stickRY: // 190-231
                    return (byte)((command.Value / 100) + 190);

            }

            return 255;
        }

        public static char CommandToChar(Command command)
        {
            switch (command.Pin)
            {
                case Pins.buttonA:
                    if (command.State == PinState.HIGH)
                    {
                        return 'A';
                    }
                    else
                    {
                        return 'a';
                    }
                case Pins.buttonB:
                    if (command.State == PinState.HIGH)
                    {
                        return 'B';
                    }
                    else
                    {
                        return 'b';
                    }
                case Pins.buttonX:
                    if (command.State == PinState.HIGH)
                    {
                        return 'X';
                    }
                    else
                    {
                        return 'x';
                    }
                case Pins.buttonY:
                    if (command.State == PinState.HIGH)
                    {
                        return 'Y';
                    }
                    else
                    {
                        return 'y';
                    }

                case Pins.buttonL:
                    if (command.State == PinState.HIGH)
                    {
                        return 'L';
                    }
                    else
                    {
                        return 'l';
                    }
                case Pins.buttonR:
                    if (command.State == PinState.HIGH)
                    {
                        return 'R';
                    }
                    else
                    {
                        return 'r';
                    }

                case Pins.buttonPlus:
                    if (command.State == PinState.HIGH)
                    {
                        return 'P';
                    }
                    else
                    {
                        return 'p';
                    }
                case Pins.buttonMinus:
                    if (command.State == PinState.HIGH)
                    {
                        return 'M';
                    }
                    else
                    {
                        return 'm';
                    }

                case Pins.buttonDUp:
                    if (command.State == PinState.HIGH)
                    {
                        return 'W';
                    }
                    else
                    {
                        return 'w';
                    }
                case Pins.buttonDLeft:
                    if (command.State == PinState.HIGH)
                    {
                        return 'Q';
                    }
                    else
                    {
                        return 'q';
                    }
                case Pins.buttonDDown:
                    if (command.State == PinState.HIGH)
                    {
                        return 'S';
                    }
                    else
                    {
                        return 's';
                    }
                case Pins.buttonDRight:
                    if (command.State == PinState.HIGH)
                    {
                        return 'E';
                    }
                    else
                    {
                        return 'e';
                    }

                case Pins.buttonHome:
                    if (command.State == PinState.HIGH)
                    {
                        return 'H';
                    }
                    else
                    {
                        return 'h';
                    }
                case Pins.buttonCapture:
                    if (command.State == PinState.HIGH)
                    {
                        return 'C';
                    }
                    else
                    {
                        return 'c';
                    }
                case Pins.stickLX:
                    return 'F';
                case Pins.stickLY:
                    return 'G';
            }

            return '-';
        }

        public static byte CommandToByte(Command command)
        {
            switch (command.Pin)
            {
                case Pins.buttonA:
                    if (command.State == PinState.HIGH)
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'A' })[0];
                    }
                    else
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'a' })[0];
                    }
                case Pins.buttonB:
                    if (command.State == PinState.HIGH)
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'B' })[0];
                    }
                    else
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'b' })[0];
                    }
                case Pins.buttonX:
                    if (command.State == PinState.HIGH)
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'X' })[0];
                    }
                    else
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'x' })[0];
                    }
                case Pins.buttonY:
                    if (command.State == PinState.HIGH)
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'Y' })[0];
                    }
                    else
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'y' })[0];
                    }

                case Pins.buttonL:
                    if (command.State == PinState.HIGH)
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'L' })[0];
                    }
                    else
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'l' })[0];
                    }
                case Pins.buttonR:
                    if (command.State == PinState.HIGH)
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'R' })[0];
                    }
                    else
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'r' })[0];
                    }

                case Pins.buttonPlus:
                    if (command.State == PinState.HIGH)
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'P' })[0];
                    }
                    else
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'p' })[0];
                    }
                case Pins.buttonMinus:
                    if (command.State == PinState.HIGH)
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'M' })[0];
                    }
                    else
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'm' })[0];
                    }

                case Pins.buttonDUp:
                    if (command.State == PinState.HIGH)
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'W' })[0];
                    }
                    else
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'w' })[0];
                    }
                case Pins.buttonDLeft:
                    if (command.State == PinState.HIGH)
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'Q' })[0];
                    }
                    else
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'q' })[0];
                    }
                case Pins.buttonDDown:
                    if (command.State == PinState.HIGH)
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'S' })[0];
                    }
                    else
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 's' })[0];
                    }
                case Pins.buttonDRight:
                    if (command.State == PinState.HIGH)
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'E' })[0];
                    }
                    else
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'e' })[0];
                    }

                case Pins.buttonHome:
                    if (command.State == PinState.HIGH)
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'H' })[0];
                    }
                    else
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'h' })[0];
                    }
                case Pins.buttonCapture:
                    if (command.State == PinState.HIGH)
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'C' })[0];
                    }
                    else
                    {
                        return Encoding.UTF7.GetBytes(new char[1] { 'c' })[0];
                    }
                case Pins.stickLX:
                    return Encoding.UTF7.GetBytes(new char[1] { 'F' })[0];
                case Pins.stickLY:
                    return Encoding.UTF7.GetBytes(new char[1] { 'G' })[0];
            }

            return 0;
        }
    }
}
