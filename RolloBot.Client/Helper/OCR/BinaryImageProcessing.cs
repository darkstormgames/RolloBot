using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolloBot.Client.Helper.OCR
{
    public static class BinaryImageProcessing
    {
        private static ImageConverter ic = new ImageConverter();

        public static CachedBitmap[] ExtractPlayerTags(this Bitmap img)
        {
            if (img == null)
                throw new ArgumentException("The given screenshot does not exist!");
            if (img.Width != 1280 || img.Height != 720)
                throw new ArgumentException("The size of the given screenshot is not valid!");

            Bitmap[] tagImages = new Bitmap[12];
            for (int i = 0; i < 12; i++)
            {
                tagImages[i] = img.extractRegion(680, 52 + 52 * i, 72, 43);
            }

            CachedBitmap[] cachedPlayerTags = new CachedBitmap[12];
            for (int i = 0; i < 12; i++)
            {
                float isYellow = tagImages[i].RegionProximity(0, 0, tagImages[i].Width, 5, 15, 220, 241);

                byte[] processedImg;
                if (isYellow > 0.8f)
                {
                    processedImg = tagImages[i].Binarize(77, 85, 64, 0.8f);
                }
                else
                {
                    processedImg = tagImages[i].Binarize(255, 255, 255, 0.7f);
                }

                cachedPlayerTags[i] = new CachedBitmap((Bitmap)ic.ConvertFrom(processedImg));
            }


            return cachedPlayerTags;
        }


        public static CachedBitmap[] ExtractPlayerNames(this Bitmap img, bool isTrophyScreen = false)
        {
            if (img == null)
                throw new ArgumentException("The given screenshot does not exist!");
            if (img.Width != 1280 || img.Height != 720)
                throw new ArgumentException("The size of the given screenshot is not valid!");

            Bitmap[] playerImages = new Bitmap[12];
            CachedBitmap[] cachedPlayerImages = new CachedBitmap[12];
            if (isTrophyScreen) // try detect trophy screen later...
            {

            }
            else
            {
                for (int i = 0; i < 12; i++)
                {
                    playerImages[i] = img.extractRegion(680, 52 + 52 * i, 276, 43);
                }

                for (int i = 0; i < 12; i++)
                {
                    float isYellow = playerImages[i].RegionProximity(0, 0, playerImages[i].Width, 5, 15, 220, 241);

                    byte[] processedImg;
                    if (isYellow > 0.8f)
                    {
                        processedImg = playerImages[i].Binarize(77, 85, 64, 0.8f);
                    }
                    else
                    {
                        processedImg = playerImages[i].Binarize(255, 255, 255, 0.7f);
                    }

                    cachedPlayerImages[i] = new CachedBitmap((Bitmap)ic.ConvertFrom(processedImg));
                }
            }

            return cachedPlayerImages;
        }

        public static CachedBitmap[] ExtractPlayerFlags(this Bitmap img)
        {
            if (img == null)
                throw new ArgumentException("The given screenshot does not exist!");
            if (img.Width != 1280 || img.Height != 720)
                throw new ArgumentException("The size of the given screenshot is not valid!");

            CachedBitmap[] cachedPlayerFlags = new CachedBitmap[12];

            for (int i = 0; i < 12; i++)
            {
                cachedPlayerFlags[i] = new CachedBitmap(img.extractRegion(958, 60 + 52 * i, 42, 28));
            }

            return cachedPlayerFlags;
        }

        public static CachedBitmap[] ExtractPlayerScores(this Bitmap img, bool isTrophyScreen = false)
        {
            if (img == null)
                throw new ArgumentException("The given screenshot does not exist!");
            if (img.Width != 1280 || img.Height != 720)
                throw new ArgumentException("The size of the given screenshot is not valid!");

            Bitmap[] playerScores = new Bitmap[12];
            CachedBitmap[] cachedPlayerScores = new CachedBitmap[12];
            if (isTrophyScreen) // try detect trophy screen later...
            {

            }
            else
            {
                for (int i = 0; i < 12; i++)
                {
                    playerScores[i] = img.extractRegion(1126, 52 + 52 * i, 92, 43);
                }

                for (int i = 0; i < 12; i++)
                {
                    float isYellow = playerScores[i].RegionProximity(0, 0, playerScores[i].Width, 5, 15, 220, 241);

                    byte[] processedImg;
                    if (isYellow > 0.8f)
                    {
                        processedImg = playerScores[i].Binarize(77, 85, 64, 0.8f);
                    }
                    else
                    {
                        processedImg = playerScores[i].Binarize(255, 255, 255, 0.7f);
                    }

                    cachedPlayerScores[i] = new CachedBitmap((Bitmap)ic.ConvertFrom(processedImg));
                }
            }
            return cachedPlayerScores;
        }

    }
}
