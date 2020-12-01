using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using RolloBot.Client.Communication;
using RolloBot.Client.Communication.Capture;
using RolloBot.Client.Communication.Serial;
using RolloBot.Client.Communication.XInput;

namespace RolloBot.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged, ICommunicationsOwner
    {
        public MainViewModel()
        {

        }

        public VideoCapture VideoCapture { get; private set; }
        public SerialCommunication SerialCommunication { get; private set; }
        public XInputPoller XInputPoller { get; private set; }

        #region ICommunicationsOwner

        private CaptureState captureState = CaptureState.Stopped;
        public CaptureState CaptureState
        {
            get { return this.captureState; }
            set
            {
                if (value != captureState)
                {
                    captureState = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private Bitmap currentImg;
        public Bitmap CurrentImg
        {
            get { return currentImg; }
            set
            {
                if (currentImg != value)
                {
                    currentImg = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isControllerEnabled = false;
        public bool IsControllerEnabled
        {
            get { return isControllerEnabled; }
            set
            {
                if (value != isControllerEnabled)
                {
                    isControllerEnabled = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private bool isSerialEnabled = false;
        public bool IsSerialEnabled
        {
            get { return isSerialEnabled; }
            set
            {
                if (value != isSerialEnabled)
                {
                    isSerialEnabled = value;
                    this.OnPropertyChanged();
                }
            }
        }

        #endregion

        #region PanelSelectedFields

        private bool panelCaptureSelected = false;
        public bool PanelCaptureSelected
        {
            get { return panelCaptureSelected; }
            set
            {
                if (value != panelCaptureSelected)
                {
                    panelCaptureSelected = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private bool panelTASDesignerSelected = false;
        public bool PanelTASDesignerSelected
        {
            get { return panelTASDesignerSelected; }
            set
            {
                if (value != panelTASDesignerSelected)
                {
                    panelTASDesignerSelected = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private bool panelTASRecorderSelected = false;
        public bool PanelTASRecorderSelected
        {
            get { return panelTASRecorderSelected; }
            set
            {
                if (value != panelTASRecorderSelected)
                {
                    panelTASRecorderSelected = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private bool panelMK8DXSelected = false;
        public bool PanelMK8DXSelected
        {
            get { return panelMK8DXSelected; }
            set
            {
                if (value != panelMK8DXSelected)
                {
                    panelMK8DXSelected = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private bool panelPokemonSwShSelected = false;
        public bool PanelPokemonSwShSelected
        {
            get { return panelPokemonSwShSelected; }
            set
            {
                if (value != panelPokemonSwShSelected)
                {
                    panelPokemonSwShSelected = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private bool panelSettingsSelected = false;
        public bool PanelSettingsSelected
        {
            get { return panelSettingsSelected; }
            set
            {
                if (value != panelSettingsSelected)
                {
                    panelSettingsSelected = value;
                    this.OnPropertyChanged();
                }
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state">Overrides the current state to the given one</param>
        public void ToggleVideoCapture(CaptureState state = CaptureState.None)
        {
            if (state == CaptureState.None)
            {
                state = this.CaptureState;

                switch (this.CaptureState)
                {
                    case CaptureState.Stopped:
                        if (VideoCapture == null)
                        {
                            VideoCapture = new VideoCapture(App.Config.CaptureDevice, FPS._60);
                        }
                        else
                        {
                            VideoCapture.Start();
                        }
                        VideoCapture.NewFrame += VideoRec_NewFrame;
                        CaptureState = CaptureState.Running;
                        break;
                    case CaptureState.Paused:
                    case CaptureState.Running:
                        if (VideoCapture != null)
                        {
                            VideoCapture.NewFrame -= VideoRec_NewFrame;
                            VideoCapture.Stop();
                            VideoCapture = null;
                        }
                        CaptureState = CaptureState.Stopped;
                        break;
                }
            }
            else
            {
                switch (state)
                {
                    case CaptureState.Stopped:
                        if (VideoCapture != null)
                        {
                            VideoCapture.NewFrame -= VideoRec_NewFrame;
                            VideoCapture.Stop();
                            VideoCapture = null;
                        }

                        CaptureState = CaptureState.Stopped;
                        break;
                    case CaptureState.Paused:
                        if (VideoCapture == null)
                        {
                            VideoCapture = new VideoCapture(App.Config.CaptureDevice, FPS._60);
                        }
                        else if (this.CaptureState == CaptureState.Stopped)
                        {
                            VideoCapture.Start();
                        }

                        this.CaptureState = CaptureState.Paused;
                        break;
                    case CaptureState.Running:
                        if (VideoCapture == null)
                        {
                            VideoCapture = new VideoCapture(App.Config.CaptureDevice, FPS._60);
                        }
                        else
                        {
                            VideoCapture.Start();
                        }

                        VideoCapture.NewFrame += VideoRec_NewFrame;
                        CaptureState = CaptureState.Running;
                        break;
                }
            }
        }

        public void VideoRec_NewFrame(object sender, CaptureFrameEventArgs e)
        {
            if (CaptureState != CaptureState.Paused && CaptureState != CaptureState.Stopped)
            {
                CurrentImg = (Bitmap)e.Frame.Clone();
                //if (VideoCapture.FramesOutput % 15 == 0)
                //    GC.Collect();
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged
    }
}
