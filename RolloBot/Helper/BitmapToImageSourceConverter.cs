using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RolloBot.Helper
{
    [ValueConversion(typeof(Bitmap), typeof(ImageSource))]
    public class BitmapToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Bitmap bitmap = value as Bitmap;
            //if (bitmap == null)
            //    return null;
            if (!(value is Bitmap bitmap))
                return null;

            try
            {
                Bitmap temp = (Bitmap)bitmap.Clone();
                var bitmapData = temp.LockBits(
                    new System.Drawing.Rectangle(0, 0, temp.Width, temp.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly, temp.PixelFormat);

                var bitmapSource = BitmapSource.Create(
                    bitmapData.Width, bitmapData.Height, 96, 96, convertPixelFormat(temp.PixelFormat), null,
                    bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

                temp.UnlockBits(bitmapData);
                return bitmapSource;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static System.Windows.Media.PixelFormat convertPixelFormat(System.Drawing.Imaging.PixelFormat sourceFormat)
        {
            switch (sourceFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    return PixelFormats.Bgr24;

                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                    return PixelFormats.Bgra32;

                case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
                    return PixelFormats.Bgr32;

                    // .. as many as you need...
            }
            return new System.Windows.Media.PixelFormat();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
