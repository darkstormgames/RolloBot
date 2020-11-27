using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using RolloBot.Client.Communication;
using RolloBot.Client.Communication.Capture;
using RolloBot.Client.Communication.Serial;
using RolloBot.Client.Communication.XInput;
using RolloBot.Client.Configuration;
using RolloBot.Controls;

namespace RolloBot
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void window_Initialized(object sender, EventArgs e)
        {
            App.Config = Client.Configuration.Configuration.LoadDefaultConfig();
            //positions = Points.GetPoints(App.Config.CaptureResolution);
            //this.createButtons();
        }

        private void window_Closing(object sender, CancelEventArgs e)
        {
            //if (VideoCapture != null)
            //    VideoCapture.Stop();
        }
    }
}
