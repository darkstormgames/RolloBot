using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolloBot.Client.Helper.GameObjects
{
    public class PlayerScore
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public Bitmap Flag { get; set; }

        private string flagString;
        public string FlagString
        {
            get { return flagString; }
            set
            {
                if (flagString != value)
                {
                    flagString = value;
                    DrawFlag();
                }
            }
        }

        public PlayerScore()
        {
            this.Name = string.Empty;
            this.Score = 0;
            this.FlagString = string.Empty;
        }

        public Bitmap DrawFlag()
        {
            Image resultImage = new Bitmap(100, 100, PixelFormat.Format24bppRgb);
            if (string.IsNullOrEmpty(this.flagString))
            {
                using (Graphics g = Graphics.FromImage(resultImage))
                {
                    g.FillRectangle(
                        Brushes.Transparent, 0, 0, 100, 100);
                }
            }
            else
            {
                // Get flag-images for country-codes
            }

            this.Flag = (Bitmap)resultImage.Clone();
            return Flag;
        }
    }
}
