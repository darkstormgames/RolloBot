using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolloBot.Client.Configuration
{
    public class Configuration
    {
        private IniFile configFile;

        #region DevicesConfig
        private int captureDevice;
        public int CaptureDevice
        {
            get
            {
                return captureDevice;
            }
            set
            {
                captureDevice = value;

                configFile.Write("CaptureDevice", value.ToString(), "Devices");
            }
        }

        private Resolutions captureResolution;
        public Resolutions CaptureResolution
        {
            get
            {
                return captureResolution;
            }
            set
            {
                captureResolution = value;

                configFile.Write("CaptureResolution", ((int)value).ToString(), "Devices");
            }
        }

        private string serialPort;
        public string SerialPort
        {
            get
            {
                return serialPort;
            }
            set
            {
                serialPort = value;

                configFile.Write("SerialPort", value, "Devices");
            }
        }

        private int serialBaudRate;
        public int SerialBaudRate
        {
            get
            {
                return serialBaudRate;
            }
            set
            {
                serialBaudRate = value;

                configFile.Write("SerialBaudRate", value.ToString(), "Devices");
            }
        }

        private Parity serialParity;
        public Parity SerialParity
        {
            get
            {
                return serialParity;
            }
            set
            {
                serialParity = value;

                configFile.Write("SerialParity", ((int)value).ToString(), "Devices");
            }
        }

        private int serialDataBits;
        public int SerialDataBits
        {
            get
            {
                return serialDataBits;
            }
            set
            {
                serialDataBits = value;

                configFile.Write("SerialDataBits", value.ToString(), "Devices");
            }
        }

        private StopBits serialStopBits;
        public StopBits SerialStopBits
        {
            get
            {
                return serialStopBits;
            }
            set
            {
                serialStopBits = value;

                configFile.Write("SerialStopBits", ((int)value).ToString(), "Devices");
            }
        }
        #endregion

        #region StartupConfig
        private bool serialOnStartup;
        public bool SerialOnStartup
        {
            get
            {
                return serialOnStartup;
            }
            set
            {
                serialOnStartup = value;

                configFile.Write("SerialOnStartup", value.ToString(), "Startup");

            }
        }

        private bool controllerOnStartup;
        public bool ControllerOnStartup
        {
            get
            {
                return controllerOnStartup;
            }
            set
            {
                controllerOnStartup = value;

                configFile.Write("ControllerOnStartup", value.ToString(), "Startup");


            }
        }

        private bool captureOnStartup;
        public bool CaptureOnStartup
        {
            get
            {
                return captureOnStartup;
            }
            set
            {
                captureOnStartup = value;

                configFile.Write("CaptureOnStartup", value.ToString(), "Startup");

            }
        }

        private bool buttonsOnStartup;
        public bool ButtonsOnStartup
        {
            get
            {
                return buttonsOnStartup;
            }
            set
            {
                buttonsOnStartup = value;

                configFile.Write("ButtonsOnStartup", value.ToString(), "Startup");

            }
        }
        #endregion

        #region VideoConfig

        #endregion


        public Configuration()
        {
            this.configFile = new IniFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\darkstormgames\RolloBot\");
            this.loadConfig();
        }

        private Configuration loadConfig()
        {
            #region DeviceConfig
            if (!configFile.KeyExists("CaptureDevice", "Devices"))
            {
                this.CaptureDevice = 0;
            }
            else
            {
                if (int.TryParse(configFile.Read("CaptureDevice", "Devices"), out int result))
                {
                    this.captureDevice = result;
                }
                else
                {
                    this.CaptureDevice = 0;
                }
            }

            if (!configFile.KeyExists("CaptureResolution", "Devices"))
            {
                this.CaptureResolution = (Resolutions)1;
            }
            else
            {
                if (int.TryParse(configFile.Read("CaptureResolution", "Devices"), out int result))
                {
                    this.captureResolution = (Resolutions)result;
                }
                else
                {
                    this.CaptureResolution = (Resolutions)1;
                }
            }

            if (!configFile.KeyExists("SerialPort", "Devices"))
            {
                this.SerialPort = "COM3";
            }
            else
            {
                this.serialPort = configFile.Read("SerialPort", "Devices");
            }

            if (!configFile.KeyExists("SerialBaudRate", "Devices"))
            {
                this.SerialBaudRate = 19200;
            }
            else
            {
                if (int.TryParse(configFile.Read("SerialBaudRate", "Devices"), out int result))
                {
                    this.serialBaudRate = result;
                }
                else
                {
                    this.SerialBaudRate = 19200;
                }
            }

            if (!configFile.KeyExists("SerialParity", "Devices"))
            {
                this.SerialParity = (Parity)2;
            }
            else
            {
                if (int.TryParse(configFile.Read("SerialParity", "Devices"), out int result))
                {
                    this.serialParity = (Parity)result;
                }
                else
                {
                    this.SerialParity = (Parity)2;
                }
            }

            if (!configFile.KeyExists("SerialDataBits", "Devices"))
            {
                this.SerialDataBits = 8;
            }
            else
            {
                if (int.TryParse(configFile.Read("SerialDataBits", "Devices"), out int result))
                {
                    this.serialDataBits = result;
                }
                else
                {
                    this.SerialDataBits = 8;
                }
            }

            if (!configFile.KeyExists("SerialStopBits", "Devices"))
            {
                this.SerialStopBits = (StopBits)1;
            }
            else
            {
                if (int.TryParse(configFile.Read("SerialStopBits", "Devices"), out int result))
                {
                    this.serialStopBits = (StopBits)result;
                }
                else
                {
                    this.SerialStopBits = (StopBits)1;
                }
            }
            #endregion

            #region StartupConfig
            if (!configFile.KeyExists("SerialOnStartup", "Startup"))
            {
                this.SerialOnStartup = false;
            }
            else
            {
                if (bool.TryParse(configFile.Read("SerialOnStartup", "Startup"), out bool result))
                {
                    this.serialOnStartup = result;
                }
                else
                {
                    this.SerialOnStartup = false;
                }
            }

            if (!configFile.KeyExists("ControllerOnStartup", "Startup"))
            {
                this.ControllerOnStartup = false;
            }
            else
            {
                if (bool.TryParse(configFile.Read("ControllerOnStartup", "Startup"), out bool result))
                {
                    this.controllerOnStartup = result;
                }
                else
                {
                    this.ControllerOnStartup = false;
                }
            }

            if (!configFile.KeyExists("CaptureOnStartup", "Startup"))
            {
                this.CaptureOnStartup = false;
            }
            else
            {
                if (bool.TryParse(configFile.Read("CaptureOnStartup", "Startup"), out bool result))
                {
                    this.captureOnStartup = result;
                }
                else
                {
                    this.CaptureOnStartup = false;
                }
            }

            if (!configFile.KeyExists("ButtonsOnStartup", "Startup"))
            {
                this.ButtonsOnStartup = false;
            }
            else
            {
                if (bool.TryParse(configFile.Read("ButtonsOnStartup", "Startup"), out bool result))
                {
                    this.buttonsOnStartup = result;
                }
                else
                {
                    this.ButtonsOnStartup = false;
                }
            }
            #endregion

            #region VideoConfig

            #endregion


            return this;
        }

        public static Configuration LoadDefaultConfig()
        {
            return new Configuration();
        }
    }
}
