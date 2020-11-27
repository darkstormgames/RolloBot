using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Tesseract;

namespace RolloBot.Client.Helper.OCR
{
    public class ScreenshotRecognition : DisposableBase
    {
        private TesseractEngine engine;

        public List<float> MeanConfidences = new List<float>();
        public float AvgConfidence => MeanConfidences.Average();

        public ScreenshotRecognition()
        {
            this.engine = new TesseractEngine(@".\tessdata", "deu", EngineMode.Default, @".\tessdata\config\configScreenshot.cfg");
        }
        
        public bool IsScreenshot(Bitmap img)
        {
            string result1 = string.Empty;
            string result2 = string.Empty;
            Bitmap line1 = img.extractRegion(115, 15, 100, 20);
            Bitmap line2 = img.extractRegion(115, 40, 115, 25);

            using (var page = engine.Process(line1, PageSegMode.Auto))
            {
                result1 = page.GetText().Replace("\n", "").Replace("\r", "").Trim();
                MeanConfidences.Add(page.GetMeanConfidence());
            }

            using (var page = engine.Process(line2, PageSegMode.Auto))
            {
                result2 = page.GetText().Replace("\n", "").Replace("\r", "").Trim();
                MeanConfidences.Add(page.GetMeanConfidence());
            }

            return result1.Contains("Aufnahme") || result2.Contains("gespeichert");
        }

        protected override void Cleanup()
        {
            engine.Dispose();
            MeanConfidences = null;
        }
    }
}
