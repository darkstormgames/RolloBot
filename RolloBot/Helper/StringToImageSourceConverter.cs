using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace RolloBot.Helper
{
    public class StringToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                value = "/Resources/resourcenotfound.png";

            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri("pack://application:,,,/RolloBot;component" + (value.ToString().StartsWith("/") ? value.ToString() : "/" + value.ToString()));
            img.EndInit();
            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
