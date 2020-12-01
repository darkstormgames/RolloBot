using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using RolloBot.Client.Communication.Capture;

namespace RolloBot.Helper
{
    public class CaptureStateToImageUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is CaptureState state))
                return null;
            
            switch (state)
            {
                case CaptureState.Running:
                    return "/Resources/preview_enabled.png";
                case CaptureState.Paused:
                    return "/Resources/preview_paused.png";
                case CaptureState.Stopped:
                default:
                    return "/Resources/preview_disabled.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("Backwards conversion of preview images is not needed...");
        }
    }
}
