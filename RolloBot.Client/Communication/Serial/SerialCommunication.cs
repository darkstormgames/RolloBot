using System;
using System.Diagnostics;
using System.IO.Ports;

using RolloBot.Client;
using RolloBot.Client.Helper;

namespace RolloBot.Client.Communication.Serial
{
    public class SerialCommunication : DisposableBase
    {
        private readonly SerialPort serial;

        #region Constructors
        public SerialCommunication()
            : this("COM1") { }
        public SerialCommunication(string portName)
            : this(portName, 9600) { }
        public SerialCommunication(string portName, int baudRate)
            : this(portName, baudRate, Parity.None) { }
        public SerialCommunication(string portName, int baudRate, Parity parity)
            : this(portName, baudRate, parity, 8) { }
        public SerialCommunication(string portName, int baudRate, Parity parity, int dataBits)
            : this(portName, baudRate, parity, dataBits, StopBits.One) { }
        public SerialCommunication(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            serial = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
        }
        #endregion

        /// <summary>
        /// Specific function for Arduino-SwitchControl
        /// </summary>
        /// <param name="output">Specifies the physical Pin for signal-output</param>
        /// <param name="targetState">Specifies the target-signal for the pin</param>
        public long SendCommand(Pins output, PinState targetState)
        {
            return this.SendCommand(new Command(output, targetState));
        }

        public long SendCommand(Command command)
        {
            return this.SendCommand(command.ToByte());
        }

        public long SendCommand(byte output)
        {
            Stopwatch watch = Stopwatch.StartNew();
            if (serial.IsOpen)
            {
                serial.Write(new byte[1] { output }, 0, 1);
            }
            watch.Stop();
            return watch.ElapsedTicks;
        }

        public bool Open()
        {
            if (this.serial.IsOpen)
            {
                return true;
            }

            bool OK = true;
            try
            {
                this.serial.Open();
            }
            catch (Exception)
            {
                OK = false;
            }
            return OK;
        }

        public void Close()
        {
            this.serial.Close();
        }

        public bool Available()
        {
            return this.serial.IsOpen;
        }

        protected override void Cleanup()
        {
            serial.Dispose();
        }
    }
}
