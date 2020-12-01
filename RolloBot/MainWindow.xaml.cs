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

using Microsoft.Win32;

using RolloBot.Client.Communication;
using RolloBot.Client.Communication.Capture;
using RolloBot.Client.Communication.Serial;
using RolloBot.Client.Communication.XInput;
using RolloBot.Client.Configuration;
using RolloBot.Styles;
using RolloBot.ViewModel;

namespace RolloBot
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel mainViewModel;
        private string keyPath = @"SOFTWARE\RolloBot";

        public MainWindow()
        {
            InitializeComponent();

            App.Config = Client.Configuration.Configuration.LoadDefaultConfig();

            this.mainViewModel = new MainViewModel();
            this.DataContext = mainViewModel;
        }

        #region Layout saving/loading
        private void saveLayout()
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            if (dialog == null)
            {
                return;
            }

            dialog.Filter = "Layout Files (*.xml)|*.xml";
            dialog.CheckFileExists = false;
            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            try
            {
                //_layoutManager.SaveLayoutToFile(dialog.FileName);
            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show("Unable to save layout: " + exception.Message);
            }
        }

        private void loadLayout()
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            if (dialog == null)
            {
                return;
            }

            dialog.Filter = "Layout Files (*.xml)|*.xml";
            dialog.CheckFileExists = true;
            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            //ExampleDockManagerViews.ViewModel.MainViewModel mainViewModel = DataContext as ExampleDockManagerViews.ViewModel.MainViewModel;
            //System.Diagnostics.Trace.Assert(mainViewModel != null);

            try
            {
                //_layoutManager.LoadLayoutFromFile(dialog.FileName);
                //mainViewModel.LayoutLoaded = true;
            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show("Unable to load layout: " + exception.Message);
            }
        }
        #endregion

        #region Window Loading/Closing/etc.

        private void window_Closing(object sender, CancelEventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(this.keyPath, true);
            if (key == null)
            {
                key = Registry.CurrentUser.CreateSubKey(this.keyPath, true);
            }

            key.SetValue("Height", ActualHeight);
            key.SetValue("Width", ActualWidth);
            key.SetValue("Top", Top);
            key.SetValue("Left", Left);
            key.SetValue("WindowState", WindowState.ToString());

            // Cleaning up communication/tools
            if (mainViewModel.CaptureState == CaptureState.Running || mainViewModel.CaptureState == CaptureState.Paused)
            {
                mainViewModel.VideoCapture.Stop();
                //mainViewModel.VideoCapture = null;
            }
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(this.keyPath);
            if (key == null)
            {
                key = Registry.CurrentUser.CreateSubKey(this.keyPath);
            }
            else
            {
                object obj = key.GetValue("Height");
                if (obj != null)
                {
                    Height = Convert.ToDouble(obj);
                }
                obj = key.GetValue("Width");
                if (obj != null)
                {
                    Width = Convert.ToDouble(obj);
                }
                obj = key.GetValue("Top");
                if (obj != null)
                {
                    Top = Convert.ToDouble(obj);
                }
                obj = key.GetValue("Left");
                if (obj != null)
                {
                    Left = Convert.ToDouble(obj);
                }
                obj = key.GetValue("WindowState");
                if (obj != null)
                {
                    if (Enum.TryParse((string)obj, out WindowState windowState))
                    {
                        WindowState = windowState;
                    }
                }
            }

            if (App.Config.CaptureOnStartup)
            {
                mainViewModel.ToggleVideoCapture();
            }
            
            btnCapturePreview_Click(this, new RoutedEventArgs());
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case WindowState.Maximized:
                    btnRestore.Visibility = Visibility.Visible;
                    btnMaximize.Visibility = Visibility.Collapsed;
                    break;
                case WindowState.Normal:
                    btnRestore.Visibility = Visibility.Collapsed;
                    btnMaximize.Visibility = Visibility.Visible;
                    break;
            }
        }

        #endregion

        private void btnTitle_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu menu = new ContextMenu();
            MenuItem menuItem = null;

            menuItem = new MenuItem
            {
                Header = "Save Screenshot",
                IsCheckable = false,
                Command = new Command(
                delegate {
                    saveLayout();
                }, delegate { return true; })
            };
            menu.Items.Add(menuItem);

            menuItem = new MenuItem
            {
                Header = "Toggle Controls",
                IsCheckable = false,
                Command = new Command(delegate 
                {
                    loadLayout();
                }, delegate { return true; })
            };
            menu.Items.Add(menuItem);

            menuItem = new MenuItem
            {
                Header = "Settings",
                IsCheckable = false,
                Command = new Command(delegate 
                {
                    loadLayout();
                }, delegate { return true; })
            };
            menu.Items.Add(menuItem);

            menu.Items.Add(new Separator());

            menuItem = new MenuItem
            {
                Header = "Exit",
                IsCheckable = false,
                Command = new Command(delegate
                {
                    btnClose_Click(this, new RoutedEventArgs());
                }, p => true)
            };
            menu.Items.Add(menuItem);

            menu.IsOpen = true;
        }

        #region CommunicationButtonsControl

        /// <summary>
        /// Starts/Stops the preview from the capture card
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            mainViewModel.ToggleVideoCapture();
        }

        /// <summary>
        /// Starts/Stops XInput
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnController_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Starts/Stops the serial connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSerial_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region SidePanelControl

        private void clearSelection()
        {
            mainViewModel.PanelCaptureSelected = false;
            mainViewModel.PanelTASDesignerSelected = false;
            mainViewModel.PanelTASRecorderSelected = false;
            mainViewModel.PanelMK8DXSelected = false;
            mainViewModel.PanelPokemonSwShSelected = false;
            mainViewModel.PanelSettingsSelected = false;
        }

        private void btnCapturePreview_Click(object sender, RoutedEventArgs e)
        {
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new View.CapturePreviewView(mainViewModel));
            this.clearSelection();
            mainViewModel.PanelCaptureSelected = true;
        }

        private void btnTASDesigner_Click(object sender, RoutedEventArgs e)
        {
            ContentGrid.Children.Clear();

            this.clearSelection();
            mainViewModel.PanelTASDesignerSelected = true;
        }

        private void btnTASRecorder_Click(object sender, RoutedEventArgs e)
        {
            ContentGrid.Children.Clear();

            this.clearSelection();
            mainViewModel.PanelTASRecorderSelected = true;
        }

        private void btnMK8DX_Click(object sender, RoutedEventArgs e)
        {
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new Client.Tools.MK8DX.ToolPanel(mainViewModel));
            this.clearSelection();
            mainViewModel.PanelMK8DXSelected = true;
        }

        private void btnPokemonSwSh_Click(object sender, RoutedEventArgs e)
        {
            ContentGrid.Children.Clear();

            this.clearSelection();
            mainViewModel.PanelPokemonSwShSelected = true;
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            ContentGrid.Children.Clear();

            this.clearSelection();
            mainViewModel.PanelSettingsSelected = true;
        }

        #endregion

        #region Close/Minimize/Maximize/Restore window

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        #endregion
    }
}
