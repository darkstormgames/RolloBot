using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RolloBot.Client.Helper.OCR
{
    public class CachedBitmap
    {
        private readonly ImageConverter ic;

        public byte[] Binary { get; private set; }
        public Bitmap Bitmap
        {
            get
            {
                BitmapCache = (Bitmap)this.ic.ConvertFrom(this.Binary);
                return BitmapCache;
            }
        }
        public Bitmap BitmapCache { get; private set; }
        public int Width { get; }
        public int Height { get; }
        /// <summary>
        /// DO NOT SET VALUES!!!
        /// This is just to read and iterate through HeaderData
        /// Changes to this will not be saved!
        /// </summary>
        public byte[] BinaryOverhead
        {
            get
            {
                if (Binary.Length == 0)
                    return new byte[0];

                byte[] data = new byte[54];
                for (int i = 0; i < 54; i++)
                {
                    data[i] = Binary[i];
                }
                return data;
            }
        }
        /// <summary>
        /// DO NOT SET VALUES!!!
        /// This is just to read and iterate through ImageData
        /// Changes to this will not be saved!
        /// </summary>
        public byte[] BinaryData
        {
            get
            {
                if (Binary.Length == 0)
                    return new byte[0];

                byte[] data = new byte[Binary.Length - 54];
                for (int i = 54; i < Binary.Length; i++)
                {
                    data[i - 54] = Binary[i];
                }
                return data;
            }
        }


        public List<List<int>> CacheNearestBinaryPixel { get; private set; }
        public int[] CacheNextFilledColumn { get; private set; }
        public int[] CacheNextEmptyColumn { get; private set; }
        public int[] CachePrevFilledColumn { get; private set; }
        public List<double> CacheColumnFilling { get; private set; }

        #region Constructors
        /// <summary>
        /// Creates a CachedBitmap from a Bitmap object
        /// </summary>
        /// <param name="bitmap">The Bitmap object to use</param>
        public CachedBitmap(Bitmap bitmap)
        {
            this.ic = new ImageConverter();
            this.Binary = bitmap.ToByte();
            this.Width = bitmap.Width;
            this.Height = bitmap.Height;

            this.createCache();
        }

        /// <summary>
        /// Creates a CachedBitmap from ImageData (without header)
        /// </summary>
        /// <param name="imageData">The ImageData without header</param>
        /// <param name="Width">Width of the target image</param>
        /// <param name="Height">Height of the target image</param>
        public CachedBitmap(byte[] imageData, int Width, int Height)
        {
            this.ic = new ImageConverter();
            byte[] width = BitConverter.GetBytes(Width);
            byte[] height = BitConverter.GetBytes(Height);
            byte[] size = BitConverter.GetBytes(imageData.Length);
            byte[] overhead = new byte[]
            {
                66, 77, size[0], size[1], size[2], size[3], 0, 0, 0, 0,
                54, 0, 0, 0, 40, 0, 0, 0, width[0], width[1],
                width[2], width[3], height[0], height[1], height[2], height[3], 1, 0, 24, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, /*196, 14,*/
                0, 0, 0, 0 /*196, 14*/, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0
            };

            this.Binary = (byte[])overhead.Concat(imageData).ToArray().Clone();
            this.Width = Width;
            this.Height = Height;

            this.createCache();
        }

        /// <summary>
        /// Creates a CachedBitmap from raw ImageData (with header)
        /// </summary>
        /// <param name="rawImageData">The ImageData with header</param>
        public CachedBitmap(byte[] rawImageData)
        {
            this.ic = new ImageConverter();

            this.Binary = (byte[])rawImageData.Clone();

            this.Width = this.Bitmap.Width;
            this.Height = this.Bitmap.Height;

            this.createCache();
        }

        #endregion

        private void createCache()
        {
            if (this.CacheNearestBinaryPixel != null)
                return;

            this.CacheNearestBinaryPixel = new List<List<int>>();
            for (int y = 0; y < this.Height; y++)
            {
                CacheNearestBinaryPixel.Add(new List<int>());
                for (int x = 0; x < this.Width; x++)
                {
                    CacheNearestBinaryPixel[y].Add(getNearestBinaryPixel(x, y, 0, this.Width, 0, this.Height));
                }
            }

            this.CacheNextFilledColumn = new int[this.Width];
            this.CacheNextEmptyColumn = new int[this.Width];
            this.CachePrevFilledColumn = new int[this.Width];
            this.CacheColumnFilling = new List<double>();

            for (int x = 0; x < this.Width; x++)
            {
                CacheNextFilledColumn[x] = FindNextBinaryColumn(x, true);
                CacheNextEmptyColumn[x] = FindNextBinaryColumn(x, false);
                CachePrevFilledColumn[x] = findPreviousBinaryColumn(x, true);
                CacheColumnFilling.Add(getColumnFilling(x));
            }
        }

        public void ReloadCache()
        {
            this.CacheNearestBinaryPixel = null;
            this.createCache();
        }

        public int FindNextBinaryColumn(int x, bool filled)
        {
            if (x < 0)
                x = 0;

            while (x < this.Width)
            {
                bool columnFilled = false;
                for (int y = 0; y < this.Height; y++)
                {
                    if (GetBinaryPixel(x, y))
                    {
                        columnFilled = true;
                        break;
                    }
                }

                if (filled == columnFilled)
                {
                    return x;
                }

                x++;
            }

            return -1;
        }

        private int findPreviousBinaryColumn(int x, bool filled)
        {
            while (x >= 0)
            {
                bool columnFilled = false;

                for (int y = 0; y < this.Height; y++)
                {
                    if (GetBinaryPixel(x, y))
                    {
                        columnFilled = true;
                        break;
                    }
                }

                if (filled == columnFilled)
                {
                    return x;
                }

                x--;
            }

            return -1;
        }

        private double getColumnFilling(int x)
        {
            int filling = 0;
            for (int y = 0; y < this.Height; y++)
            {
                if (GetBinaryPixel(x, y))
                {
                    filling++;
                }
            }

            return (double)filling / (double)this.Height;
        }

        private int getNearestBinaryPixel(int x, int y, int xMin, int xMax, int yMin, int yMax)
        {
            for (int layer = 0; layer <= 4; layer++)
            {
                for (int step = 0; step <= layer; step++)
                {
                    if (testPixel(x - layer, y - step, xMin, xMax, yMin, yMax) ||
                        testPixel(x - layer, y + step, xMin, xMax, yMin, yMax) ||
                        testPixel(x + layer, y - step, xMin, xMax, yMin, yMax) ||
                        testPixel(x + layer, y + step, xMin, xMax, yMin, yMax) ||
                        testPixel(x - layer, y - step, xMin, xMax, yMin, yMax) ||
                        testPixel(x + layer, y - step, xMin, xMax, yMin, yMax) ||
                        testPixel(x - layer, y + step, xMin, xMax, yMin, yMax) ||
                        testPixel(x + layer, y + step, xMin, xMax, yMin, yMax))
                    {
                        return layer + (layer > 0 ? step / layer : 0);
                    }
                }
            }

            return 100;
        }

        private bool testPixel(int x, int y, int xMin, int xMax, int yMin, int yMax)
        {
            if (x < xMin || x >= xMax || y < yMin || y >= yMax)
            {
                return false;
            }
            return GetBinaryPixel(x, y);
        }

        public bool GetBinaryPixel(int x, int y)
        {
            if (x >= this.Width || y >= this.Height)
                return false;

            return this.Binary[((y * this.Width + x) * 3) + 54] != 0 && 
                   this.Binary[((y * this.Width + x) * 3) + 55] != 0 && 
                   this.Binary[((y * this.Width + x) * 3) + 56] != 0;
        }
    }
}
