using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Markup;
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

using RolloBot.Client;
using RolloBot.Client.Communication;

namespace RolloBot.Tools
{
    /// <summary>
    /// Interaktionslogik für ToolWindowBase.xaml
    /// </summary>
    public partial class ToolWindowBase : UserControl
    {
        public class ListItem
        {
            public ListItem(LogType type, string message)
            {
                this.LogType = type;
                this.Message = message;
            }
            public LogType LogType { get; private set; }
            public string Message { get; private set; }
        }

        public ObservableCollection<ListItem> ListItems = new ObservableCollection<ListItem>();

        protected IEnumerable<ToolBase> tools;

        public StackPanel OptionsPanel => optionsPanel;

        public ICommunicationsOwner Communication { get; protected set; }


        internal ToolWindowBase()
        {
            InitializeComponent();
        }

        internal ToolWindowBase(ICommunicationsOwner communication) : this()
        {
            this.Communication = communication;
        }

        protected void LoadTools(IEnumerable<ToolBase> tools)
        {
            this.tools = new List<ToolBase>(tools);

            listTools.ItemsSource = this.tools;
            // Do some UI-filling here...
        }

        public void AddLog(LogType type, string message)
        {
            if (ListItems == null)
                ListItems = new ObservableCollection<ListItem>();
            if (ListItems.Count == 0)
                lvLog.ItemsSource = ListItems;

            lvLog.Dispatcher.Invoke(() =>
            {
                this.ListItems.Add(new ListItem(type, message));
                
                lvLog.SelectedIndex = ListItems.Count - 1;
                lvLog.ScrollIntoView(lvLog.SelectedItem);

                if (double.IsNaN(messageColumn.Width))
                    messageColumn.Width = messageColumn.ActualWidth;
                messageColumn.Width = double.NaN;

                if (double.IsNaN(typeColumn.Width))
                    typeColumn.Width = typeColumn.ActualWidth;
                typeColumn.Width = double.NaN;
            });
        }




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool isRunning = false;
        private ToolBase currentFunction;
        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            if (!isRunning)
            {
                if (!listTools.HasItems || listTools.SelectedItem == null)
                {
                    this.AddLog(LogType.Warning, "No tool selected!");
                    return;
                }
                Communication.IsPreviewPaused = true;
                ToolBase toolFunction = (listTools.SelectedItem as ToolBase);

                toolFunction.Start();
                currentFunction = toolFunction;

                btnRun.Content = "Stop Tool";
                isRunning = true;
            }
            else
            {
                currentFunction.Stop();
                Communication.IsPreviewPaused = false;
                btnRun.Content = "Run Selected";
                isRunning = false;
            }
        }

        private void ListTools_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listTools.SelectedItem == null)
            {
                OptionsPanel.Children.Clear();
                OptionsPanel.InvalidateVisual();
                return;
            }

            ToolBase toolFunction = (listTools.SelectedItem as ToolBase);

            if (toolFunction.Config != null)
                toolFunction.Config.GetOptionsView();
        }

        public void StopFunctionOnClosing()
        {
            if (isRunning)
            {
                currentFunction.Stop();
            }
        }
    }
}
