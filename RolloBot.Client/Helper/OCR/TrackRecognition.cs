using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Tesseract;

using RolloBot.Client;
using RolloBot.Client.Helper.GameObjects;

namespace RolloBot.Client.Helper.OCR
{
    public class TrackRecognition : DisposableBase
    {
        private TesseractEngine engine;
        private TesseractEngine consoleEngine;

        public List<float> MeanConfidencesTrack = new List<float>();
        public List<float> MeanConfidencesConsole = new List<float>();
        public float AvgConfidenceName => MeanConfidencesTrack.Average();
        public float AvgConfidenceConsole => MeanConfidencesConsole.Average();

        public TrackRecognition()
        {
            this.engine = new TesseractEngine(@".\tessdata", "deu", EngineMode.Default, @".\tessdata\config\configTracks.cfg");
            this.consoleEngine = new TesseractEngine(@".\tessdata", "deu", EngineMode.Default, @".\tessdata\config\configConsole.cfg");
        }

        public MK8DTrack GetTrack(Bitmap img)
        {
            string result = string.Empty;
            string resultConsole = string.Empty;
            Bitmap trackImg = img.extractRegion(410, 630, 515, 60);
            Bitmap consoleImg = img.extractRegion(320, 630, 105, 60);

            using (var page = engine.Process(trackImg, PageSegMode.Auto))
            {
                result = page.GetText().Replace("\n", "").Replace("\r", "").Trim();
                MeanConfidencesTrack.Add(page.GetMeanConfidence());

                if (result.Contains("Maria")) result = result.Replace("Maria", "Mario");
                if (result.Contains("YOshi")) result = result.Replace("YOshi", "Yoshi");
                if (result.Contains("lnstrumental")) result = result.Replace("lnstrumental", "Instrumental");
                if (result.Contains("l3")) result = result.Replace("l3", "ß");
                if (result.Contains("GroB")) result = result.Replace("GroB", "Groß");
            }

            using (var page = consoleEngine.Process(consoleImg, PageSegMode.Auto))
            {
                resultConsole = page.GetText().Replace("\n", "").Replace("\r", "").Trim();
                MeanConfidencesConsole.Add(page.GetMeanConfidence());

                if (resultConsole == "SDS") resultConsole = resultConsole = "3DS";
            }

            var tracks = MK8DTrack.GetAllTracks();
            var selection = tracks.Where(p => p.GetName("deu").ToLower() == result.ToLower()).ToList();
            if (selection.Count() == 1)
                return selection[0];
            else if (selection.Count() > 1)
            {
                GameObjects.Console console = GameObjects.Console.Switch;
                switch (resultConsole)
                {
                    case "SNES": console = GameObjects.Console.SNES; break;
                    case "N64": console = GameObjects.Console.N64; break;
                    case "GBA": console = GameObjects.Console.GBA; break;
                    case "GCN": console = GameObjects.Console.GCN; break;
                    case "DS": console = GameObjects.Console.DS; break;
                    case "Wii": console = GameObjects.Console.Wii; break;
                    case "3DS": console = GameObjects.Console._3DS; break;
                }
                var selectConsole = selection.Where(p => p.Console == console).ToList();
                return selectConsole[0];
            }
            else
                return null;
        }

        public bool IsTrack(Bitmap img)
        {
            Bitmap source = img.extractRegion(215, 610, 90, 95);
            Bitmap target = (Bitmap)Bitmap.FromFile(@".\tessdata\config\trackIdentifier.bmp");

            return source.CompareImages(target, 80, 0.7d);
        }

        protected override void Cleanup()
        {
            engine.Dispose();
            consoleEngine.Dispose();
            MeanConfidencesConsole = null;
            MeanConfidencesTrack = null;
        }
    }
}
