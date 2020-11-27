using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

using RolloBot.Client.Helper.OCR;

namespace RolloBot.Client.Helper
{
    public static class BitmapExtensions
    {
        public static Bitmap Resize(this Bitmap img, int width, int height)
        {
            if (img.Width == width && img.Height == height)
                return img;
            Bitmap b = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.DrawImage(img, 0, 0, width, height);
            }
            return b;
        }
        public static CachedBitmap ResizeToCached(this Bitmap img, int width, int height)
        {
            if (img.Width == width && img.Height == height)
                return new CachedBitmap(img);
            Bitmap b = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.DrawImage(img, 0, 0, width, height);
            }
            return new CachedBitmap(b);
        }
        public static CachedBitmap Resize(this CachedBitmap img, int width, int height)
        {
            if (img.Width == width && img.Height == height)
                return img;
            Bitmap b = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.DrawImage(img.Bitmap, 0, 0, width, height);
            }
            return new CachedBitmap(b);
        }
        public static Bitmap ResizeToBitmap(this CachedBitmap img, int width, int height)
        {
            if (img.Width == width && img.Height == height)
                return img.Bitmap;
            Bitmap b = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.DrawImage(img.Bitmap, 0, 0, width, height);
            }
            return b;
        }

        public static byte[] ToByte(this Bitmap img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                return stream.ToArray();
            }
        }
        public static byte[] ToByteHeader(this Bitmap img)
        {
            return img.ToByte().Take(54).ToArray();
        }
        public static byte[] ToByteData(this Bitmap img)
        {
            byte[] source = img.ToByte();
            byte[] imgData = new byte[source.Length - 54];
            Array.Copy(source, 54, imgData, 0, source.Length - 54);
            return imgData;
        }

        public static Bitmap extractRegion(this Bitmap source, int x, int y, int w, int h)
        {
            Image canvas = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            Rectangle rect = new Rectangle(x, y, w, h);
            Bitmap partSrc = source.Clone(rect, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(canvas))
            {
                g.DrawImage(partSrc, 0, 0, w, h);
            }

            return (Bitmap)canvas.Clone();
        }
        public static CachedBitmap extractRegion(this CachedBitmap source, int x, int y, int w, int h)
        {
            Image canvas = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            Rectangle rect = new Rectangle(x, y, w, h);
            Bitmap partSrc = source.Bitmap.Clone(rect, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(canvas))
            {
                g.DrawImage(partSrc, 0, 0, w, h);
            }

            return new CachedBitmap((Bitmap)canvas.Clone());
        }

        public static void SetPixel(this CachedBitmap img, int x, int y, byte r, byte g, byte b)
        {
            if (x < 0 || y < 0 || x >= img.Width || y >= img.Height)
                return;

            int index = (y * img.Width + x);
            if (index * 3 > img.Binary.Length)
                return;

            img.Binary[index * 3 + 54] = b;
            img.Binary[index * 3 + 55] = g;
            img.Binary[index * 3 + 56] = r;
        }

        public static bool CompareImages(this Bitmap source, Bitmap target, int colorTolerance, double minConfidence)
        {
            List<bool> samePixels = new List<bool>();
            for (int x = 0; x < source.Width; x++)
            {
                for (int y = 0; y < source.Height; y++)
                {
                    Color sourceColor = source.GetPixel(x, y);
                    Color targetColor = target.GetPixel(x, y);

                    samePixels.Add(Math.Abs(sourceColor.GetHue() - targetColor.GetHue()) <= colorTolerance);
                }
            }
            return (samePixels.Where(p => p).Count() / (double)samePixels.Count()) > minConfidence;
        }

        public static byte[] Binarize(this Bitmap img, int r, int g, int b, float threshold)
        {
            byte[] imgArray = img.ToByte();
            byte[] binarized = new byte[imgArray.Length];

            for (int i = 0; i < 54; i++)
            {
                binarized[i] = imgArray[i];
            }

            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    int index = ((y * img.Width) + x) * 3;

                    float factor = colorProximity(
                    b, imgArray[index + 54],
                    g, imgArray[index + 55],
                    r, imgArray[index + 56]);
                    byte binary = factor > threshold ? Convert.ToByte(255) : Convert.ToByte(0);

                    binarized[index + 54] = binary;
                    binarized[index + 55] = binary;
                    binarized[index + 56] = binary;
                }
            }

            return binarized;
        }

        public  static float RegionProximity(this Bitmap source, int x1, int y1, int x2, int y2, int b, int g, int r)
        {
            float result = 0f;

            for (int yy = y1; yy < y2; yy++)
                for (int xx = x1; xx < x2; xx++)
                {
                    int i = (yy * source.Width + xx);
                    if (i * 3 + 2 < 54)
                        continue;
                    byte[] imgArray = source.ToByte();

                    result += colorProximity(b, imgArray[i * 3 + 0], g, imgArray[i * 3 + 1], r, imgArray[i * 3 + 2]);
                }

            return result / ((x2 - x1) * (y2 - y1));
        }

        private static float colorProximity(int b1, int b2, int g1, int g2, int r1, int r2)
        {
            float rFactor = Math.Abs(r1 - r2) / 255f;
            float gFactor = Math.Abs(g1 - g2) / 255f;
            float bFactor = Math.Abs(b1 - b2) / 255f;

            return 1f - Math.Max(0, Math.Min(1f, ((rFactor + gFactor + bFactor) / 3f)));
        }
    }
}
