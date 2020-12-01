using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using RolloBot.Client.Configuration;

namespace RolloBot
{
    public partial class App : Application
    {
        public static bool IsDebug { get; internal set; } = true;

        public static Configuration Config { get; internal set; }

        [STAThread]
        public static void Main()
        {
            AppDomain.CurrentDomain.SetupInformation.PrivateBinPathProbe = @".\bin";

            App application = new App();
            MainWindow mainWindow = new MainWindow();
            application.Run(mainWindow);
        }
    }
}
