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
    public partial class MainWindow : Window, INotifyPropertyChanged, ICommunicationsOwner
    {
        private Dictionary<string, System.Windows.Point> positions;
        private List<RoundButton> buttons;
        
        public VideoCapture VideoCapture { get; private set; }
        public SerialCommunication SerialCommunication { get; private set; }
        public XInputPoller XInputPoller { get; private set; }



        #region Constructor & Init
        public MainWindow()
        {
            //this.DataContext = this;
            InitializeComponent();
        }

        private void window_Initialized(object sender, EventArgs e)
        {
            App.Config = Client.Configuration.Configuration.LoadDefaultConfig();
            positions = Points.GetPoints(App.Config.CaptureResolution);
            this.createButtons();
        }

        private void createButtons()
        {
            buttons = new List<RoundButton>();

            RoundButton btnA = new RoundButton() { Name = "btnA", Pin = Pins.buttonA, Width = 66, Height = 66, Content = "A", Visible = App.Config.ButtonsOnStartup };
            buttons.Add(btnA);
            RoundButton btnB = new RoundButton() { Name = "btnB", Pin = Pins.buttonB, Width = 66, Height = 66, Content = "B", Visible = App.Config.ButtonsOnStartup };
            buttons.Add(btnB);
            RoundButton btnX = new RoundButton() { Name = "btnX", Pin = Pins.buttonX, Width = 66, Height = 66, Content = "X", Visible = App.Config.ButtonsOnStartup };
            buttons.Add(btnX);
            RoundButton btnY = new RoundButton() { Name = "btnY", Pin = Pins.buttonY, Width = 66, Height = 66, Content = "Y", Visible = App.Config.ButtonsOnStartup };
            buttons.Add(btnY);

            RoundButton btnDUp = new RoundButton() { Name = "btnDUp", Pin = Pins.buttonDUp, Width = 66, Height = 66, Content = "D-Pad\r\nUP", Visible = App.Config.ButtonsOnStartup };
            buttons.Add(btnDUp);
            RoundButton btnDDown = new RoundButton() { Name = "btnDDown", Pin = Pins.buttonDDown, Width = 66, Height = 66, Content = "D-Pad\r\nDOWN", Visible = App.Config.ButtonsOnStartup };
            buttons.Add(btnDDown);
            RoundButton btnDLeft = new RoundButton() { Name = "btnDLeft", Pin = Pins.buttonDLeft, Width = 66, Height = 66, Content = "D-Pad\r\nLEFT", Visible = App.Config.ButtonsOnStartup };
            buttons.Add(btnDLeft);
            RoundButton btnDRight = new RoundButton() { Name = "btnDRight", Pin = Pins.buttonDRight, Width = 66, Height = 66, Content = "D-Pad\r\nRIGHT", Visible = App.Config.ButtonsOnStartup };
            buttons.Add(btnDRight);

            RoundButton btnHome = new RoundButton() { Name = "btnHome", Pin = Pins.buttonHome, Width = 52, Height = 52, Content = "Home", Visible = App.Config.ButtonsOnStartup };
            buttons.Add(btnHome);
            RoundButton btnCapture = new RoundButton() { Name = "btnCapture", Pin = Pins.buttonCapture, Width = 52, Height = 52, Content = "Capture", Visible = App.Config.ButtonsOnStartup };
            buttons.Add(btnCapture);

            RoundButton btnMinus = new RoundButton() { Name = "btnMinus", Pin = Pins.buttonMinus, Width = 52, Height = 52, Content = "Minus", Visible = App.Config.ButtonsOnStartup };
            buttons.Add(btnMinus);
            RoundButton btnPlus = new RoundButton() { Name = "btnPlus", Pin = Pins.buttonPlus, Width = 52, Height = 52, Content = "Plus", Visible = App.Config.ButtonsOnStartup };
            buttons.Add(btnPlus);

            RoundButton btnL = new RoundButton() { Name = "btnL", Pin = Pins.buttonL, Width = 52, Height = 52, Content = "L", Visible = App.Config.ButtonsOnStartup };
            btnL.button.Background = new SolidColorBrush(Colors.Transparent);
            //btnL.button.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 255, 255, 255);
            buttons.Add(btnL);
            RoundButton btnR = new RoundButton() { Name = "btnR", Pin = Pins.buttonR, Width = 52, Height = 52, Content = "R", Visible = App.Config.ButtonsOnStartup };
            btnR.button.Background = new SolidColorBrush(Colors.Transparent);
            //btnR.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 255, 255, 255);
            buttons.Add(btnR);
            
            foreach (var btn in buttons)
            {
                btn.button.Click += pinbtn_Click;
                //canvas1.Children.Add(btn);
                //btn.button.BringToFront();
                Canvas.SetTop(btn, positions[btn.Name].Y);
                Canvas.SetLeft(btn, positions[btn.Name].X);
            }
        }

        private void pinbtn_Click(object sender, EventArgs e)
        {
            if (sender is RoundButton)
            {
                //Serial.SendCommand((sender as RoundButton).Pin, PinState.HIGH);
                Thread.Sleep(40);
                //Serial.SendCommand((sender as RoundButton).Pin, PinState.LOW);
            }
        }
        #endregion

        #region Events

        #region ToolsTab
        private void menuMK8DXTools_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window
            {
                Title = "MK8D Tools",
                Content = new Tools.MK8DX.ToolWindow(this),
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            window.Closing += (object s, CancelEventArgs ev) =>
            {
                if (window.Content is Tools.ToolWindowBase)
                {
                    (window.Content as Tools.ToolWindowBase).StopFunctionOnClosing();
                    this.IsPreviewPaused = false;
                }
            };

            window.Show();
        }


        #endregion

        #region CommunicationTab



        #region VideoCapture
        private bool isPreviewActive = false;
        public bool IsPreviewPaused { get; set; } = false;
        private void menuCommunicationPreview_Click(object sender, RoutedEventArgs e)
        {
            if (isPreviewActive)
            {
                VideoCapture.NewFrame -= videoRec_NewFrame;
                VideoCapture = null;
                //menuCommunicationPreview.Header = "Enable Preview";
                isPreviewActive = false;
            }
            else
            {
                VideoCapture = new VideoCapture(App.Config.CaptureDevice, FPS._30);
                VideoCapture.NewFrame += videoRec_NewFrame;
                //menuCommunicationPreview.Header = "Stop Preview...";
                isPreviewActive = true;
            }
        }

        
        private Bitmap _currentImg;
        public Bitmap CurrentImg
        {
            get { return _currentImg; }
            set
            {
                if (_currentImg != value)
                {
                    _currentImg = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private void videoRec_NewFrame(object sender, CaptureFrameEventArgs e)
        {
            if (!IsPreviewPaused)
            {
                CurrentImg = (Bitmap)e.Frame.Clone();
                GC.Collect();
            }
        }
        #endregion

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void window_Closing(object sender, CancelEventArgs e)
        {
            if (VideoCapture != null)
                VideoCapture.Stop();
        }
        #endregion


    }
}
