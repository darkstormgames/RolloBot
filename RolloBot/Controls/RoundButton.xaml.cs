using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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

using RolloBot.Client.Communication.Serial;

namespace RolloBot.Controls
{
    /// <summary>
    /// Interaktionslogik für RoundButton.xaml
    /// </summary>
    public partial class RoundButton : UserControl
    {
        public Pins Pin { get; set; }
        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set
            {
                if (visible != value)
                {
                    visible = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public RoundButton()
        {
            InitializeComponent();

            button.Background = new SolidColorBrush(Colors.White);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
