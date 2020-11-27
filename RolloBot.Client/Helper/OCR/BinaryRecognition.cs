using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolloBot.Client.Helper.OCR
{
    public struct GlyphScore
    {
        public int X { get; set; }
        public BinaryChar Glyph { get; set; }
        public double Score { get; set; }
    }

    public class BinaryRecognition
    {
        private readonly BinaryChar[] charCache;

        public BinaryRecognition()
        {
            this.charCache = BinaryChar.GetBinaryChars().ToArray();
        }

        public string RecognizePlayer(CachedBitmap img)
        {
            string str = string.Empty;
            int x = 0;
            double confidence = 0f;

            while (true)
            {
                List<GlyphScore> scores = new List<GlyphScore>();

                for (int skip = -1; skip <= 1; skip++)
                {
                    int xBegin = img.FindNextBinaryColumn(x + skip, true);
                    if (xBegin == -1)
                        break;

                    foreach (BinaryChar glyph in charCache.Where(p => p.Type == CharType.Text))
                    {
                        double score = scoreGlyph(img, glyph, xBegin + skip);
                        if (score == -9999 || double.IsNaN(score))
                            continue;
                        scores.Add(new GlyphScore() { X = xBegin + skip, Glyph = glyph, Score = score });
                    }
                }
                if (scores.Count == 0)
                    break;

                scores = scores.OrderByDescending(p => p.Score).ToList();
                GlyphScore chosen = scores[0];
                confidence += chosen.Score;

                if (chosen.X - x > 6)
                    str += " ";

                string s = chosen.Glyph.Data;
                if (s == "l" || s == "i" || s == "I" || s == "!" || s == "ı")
                {
                    // Disambiguate All I´s
                }
                str += s;

                for (int y = 0; y < img.Height; y++)
                    for (int xp = 0; xp < chosen.Glyph.Bitmap.Width; xp++)
                    {
                        if (chosen.Glyph.Bitmap.GetBinaryPixel(xp, y))
                        {
                            if (img.GetBinaryPixel(chosen.X + xp, y))
                                img.SetPixel(chosen.X + xp, y, 0, 0, 255);
                            else
                                img.SetPixel(chosen.X + xp, y, 255, 0, 255);
                        }
                        else if (img.GetBinaryPixel(chosen.X + xp, y))
                        {
                            img.SetPixel(chosen.X + xp, y, 255, 0, 0);
                        }
                    }

                for (int y = 0; y < img.Height; y++)
                {
                    if (!img.GetBinaryPixel(chosen.X, y))
                        img.SetPixel(chosen.X, y, 0, 0, 255);
                }

                x = chosen.X + chosen.Glyph.Bitmap.Width + 1;
            }

            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (c != 'l' && c != 'I')
                    continue;

                char prev = (i > 0 ? str[i - 1] : ' ');
                char next = (i < str.Length - 1 ? str[i + 1] : ' ');

                if ((c == 'l' && prev == ' ') ||
                    (isUppercase(prev) && isUppercase(next)))
                    str = replaceChar(str, i, 'I');
            }

            return str;
        }

        private bool isUppercase(char c)
        {
            return c >= 'A' && c <= 'Z';
        }
        private string replaceChar(string str, int index, char c)
        {
            StringBuilder sb = new StringBuilder(str);
            sb[index] = c;
            return sb.ToString();
        }

        private double scoreGlyph(CachedBitmap img, BinaryChar glyph, int xPen)
        {
            int nextColumn = img.CacheNextFilledColumn.ElementAtOrDefault(xPen + 2);
            int nextEmptyColumn = img.CacheNextEmptyColumn.ElementAtOrDefault(xPen + glyph.Bitmap.Width);
            int endColumn = img.CacheNextFilledColumn.ElementAtOrDefault(xPen + glyph.Bitmap.Width + 4);
            int prevColumn = img.CachePrevFilledColumn.ElementAtOrDefault(xPen + glyph.Bitmap.Width + 1);

            if (nextColumn == -1 || prevColumn == -1)
                return -9999d;
            if (nextColumn - xPen < 2 || (endColumn == -1 && prevColumn - xPen < 2))
                return -9999d;
            if (nextEmptyColumn == -1 || nextEmptyColumn - xPen > 80)
                return -9999d;

            int estimatedWidthDiff = Math.Abs((nextEmptyColumn - xPen) - glyph.Bitmap.Width);
            double estimatedWidthBonus = 1d / (estimatedWidthDiff + 1d);

            double wrongColumns = 0d;
            for (int x = xPen; x < (xPen + glyph.Bitmap.Width); x++)
            {
                wrongColumns += Math.Abs(img.CacheColumnFilling.ElementAtOrDefault(x) - glyph.Bitmap.CacheColumnFilling.ElementAtOrDefault(x - xPen));
            }

            if (glyph.Bitmap.Width < 5)
                wrongColumns = 0d;
            else
                wrongColumns /= glyph.Bitmap.Width;

            double nonMatchingPixels = 0d;
            double glyphDistanceSum = 0d;
            double glyphDistanceMax = 0d;
            double glyphPixelNum = 0d;
            double targetDistanceSum = 0d;
            double targetDistanceNum = 0d;
            double targetDistanceMax = 0d;
            double targetPixelNum = 0d;

            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < glyph.Bitmap.Width; x++)
                {
                    if (glyph.Bitmap.CacheNearestBinaryPixel[y].ElementAtOrDefault(x) == 0)
                    {
                        glyphPixelNum++;

                        double distance = img.CacheNearestBinaryPixel[y].ElementAtOrDefault(x + xPen);
                        glyphDistanceSum += distance;
                        glyphDistanceMax = Math.Max(glyphDistanceMax, distance);
                    }

                    if (img.CacheNearestBinaryPixel[y].ElementAtOrDefault(x + xPen) == 0)
                    {
                        targetPixelNum++;

                        double distance = glyph.Bitmap.CacheNearestBinaryPixel[y][x];
                        targetDistanceSum += distance;
                        targetDistanceNum++;
                        targetDistanceMax = Math.Max(targetDistanceMax, distance);
                    }

                    if ((img.CacheNearestBinaryPixel[y].ElementAtOrDefault(x + xPen) <= 1) ^ (glyph.Bitmap.CacheNearestBinaryPixel[y].ElementAtOrDefault(x) <= 1))
                    {
                        nonMatchingPixels++;
                    }
                }
            }

            List<double> scoreParts = new List<double>()
            {
                -Math.Pow(glyphDistanceMax <= 1 ? 0d : glyphDistanceMax, 1.5d) * 5,
                -glyphDistanceSum / (glyphPixelNum * 0.1d),
                -glyphDistanceSum * 0.05d,
                -Math.Pow(targetDistanceMax <= 1 ? 0d : targetDistanceMax, 1.5d) * 10d,
                -(targetDistanceSum / targetDistanceNum) * 25d,
                -targetDistanceSum * 0.1d,
                -nonMatchingPixels * 0.05d,
                +(glyph.Bitmap.Width < 8 ? -5d + (glyph.Bitmap.Width * 0.5d) : 20 + (glyph.Bitmap.Width - 8) * 0.5),
                +estimatedWidthBonus * 19d,
                (glyph.Bitmap.Width < 8 && estimatedWidthBonus < 0.5) ? -10 : 0,
                (glyph.Bitmap.Width < 8 && estimatedWidthBonus > 0.5) ? 10 : 0,
                glyphPixelNum * 0.075,
            };

            double score = 0f;
            foreach (double scorePart in scoreParts)
            {
                if (!Double.IsNaN(scorePart))
                {
                    score += scorePart;
                }
                else
                {

                }
            }
            
            return score;
        }
    }
}
