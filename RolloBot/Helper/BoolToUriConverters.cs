using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RolloBot.Helper
{
    public class BoolToControllerImageUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is bool state))
                return null;

            if (state)
            {
                return "/Resources/controller_enabled.png";
            }
            else
            {
                return "/Resources/controller_disabled.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("Backwards conversion of controller images is not needed...");
        }
    }

    public class BoolToSerialImageUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is bool state))
                return null;

            if (state)
            {
                return "/Resources/serial_enabled.png";
            }
            else
            {
                return "/Resources/serial_disabled.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("Backwards conversion of serial images is not needed...");
        }
    }
}
