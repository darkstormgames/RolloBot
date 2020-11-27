using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RolloBot.Client.Configuration
{
    public static class Points
    {
        public static Dictionary<string, Point> GetPoints(Resolutions resolution)
        {
            Dictionary<string, Point> points = new Dictionary<string, Point>();
            foreach (var point in GetAllPoints().Where(p => p.Key == resolution).First().Value)
            {
                points.Add(point.Key, point.Value);
            }
            return points;
        }

        public static Dictionary<Resolutions, Dictionary<string, Point>> GetAllPoints()
        {
            Dictionary<Resolutions, Dictionary<string, Point>> points = new Dictionary<Resolutions, Dictionary<string, Point>>();

            points.Add(Resolutions._480p, new Dictionary<string, Point>());
            points.Add(Resolutions._720p, new Dictionary<string, Point>());
            points.Add(Resolutions._1080p, new Dictionary<string, Point>());


            ////    480p Positions    ////
            points[Resolutions._480p].Add("previewContainer", new Point(1778, 205));

            points[Resolutions._480p].Add("btnA", new Point(1778, 205));
            points[Resolutions._480p].Add("btnB", new Point(1720, 262));
            points[Resolutions._480p].Add("btnX", new Point(1720, 147));
            points[Resolutions._480p].Add("btnY", new Point(1662, 205));
            points[Resolutions._480p].Add("btnDUp", new Point(112, 365));
            points[Resolutions._480p].Add("btnDDown", new Point(112, 480));
            points[Resolutions._480p].Add("btnDLeft", new Point(54, 422));
            points[Resolutions._480p].Add("btnDRight", new Point(170, 422));
            points[Resolutions._480p].Add("btnHome", new Point(1688, 574));
            points[Resolutions._480p].Add("btnCapture", new Point(158, 574));
            points[Resolutions._480p].Add("btnMinus", new Point(195, 90));
            points[Resolutions._480p].Add("btnPlus", new Point(1648, 90));
            points[Resolutions._480p].Add("btnL", new Point(0, 30));
            points[Resolutions._480p].Add("btnR", new Point(1847, 30));
            points[Resolutions._480p].Add("btnZL", new Point(0, 0));
            points[Resolutions._480p].Add("btnZR", new Point(0, 0));
            points[Resolutions._480p].Add("btnL3", new Point(0, 0));
            points[Resolutions._480p].Add("btnR3", new Point(0, 0));


            ////    720p Positions    ////
            points[Resolutions._720p].Add("previewContainer", new Point(1778, 205));

            points[Resolutions._720p].Add("btnA", new Point(1778, 205));
            points[Resolutions._720p].Add("btnB", new Point(1720, 262));
            points[Resolutions._720p].Add("btnX", new Point(1720, 147));
            points[Resolutions._720p].Add("btnY", new Point(1662, 205));
            points[Resolutions._720p].Add("btnDUp", new Point(112, 365));
            points[Resolutions._720p].Add("btnDDown", new Point(112, 480));
            points[Resolutions._720p].Add("btnDLeft", new Point(54, 422));
            points[Resolutions._720p].Add("btnDRight", new Point(170, 422));
            points[Resolutions._720p].Add("btnHome", new Point(1688, 574));
            points[Resolutions._720p].Add("btnCapture", new Point(158, 574));
            points[Resolutions._720p].Add("btnMinus", new Point(195, 90));
            points[Resolutions._720p].Add("btnPlus", new Point(1648, 90));
            points[Resolutions._720p].Add("btnL", new Point(0, 30));
            points[Resolutions._720p].Add("btnR", new Point(1847, 30));
            points[Resolutions._720p].Add("btnZL", new Point(0, 0));
            points[Resolutions._720p].Add("btnZR", new Point(0, 0));
            points[Resolutions._720p].Add("btnL3", new Point(0, 0));
            points[Resolutions._720p].Add("btnR3", new Point(0, 0));


            ////    1080p Positions    ////
            points[Resolutions._1080p].Add("previewContainer", new Point(1778, 205));

            points[Resolutions._1080p].Add("btnA", new Point(1778, 205));
            points[Resolutions._1080p].Add("btnB", new Point(1720, 262));
            points[Resolutions._1080p].Add("btnX", new Point(1720, 147));
            points[Resolutions._1080p].Add("btnY", new Point(1662, 205));
            points[Resolutions._1080p].Add("btnDUp", new Point(112, 365));
            points[Resolutions._1080p].Add("btnDDown", new Point(112, 480));
            points[Resolutions._1080p].Add("btnDLeft", new Point(54, 422));
            points[Resolutions._1080p].Add("btnDRight", new Point(170, 422));
            points[Resolutions._1080p].Add("btnHome", new Point(1688, 574));
            points[Resolutions._1080p].Add("btnCapture", new Point(158, 574));
            points[Resolutions._1080p].Add("btnMinus", new Point(195, 90));
            points[Resolutions._1080p].Add("btnPlus", new Point(1648, 90));
            points[Resolutions._1080p].Add("btnL", new Point(0, 30));
            points[Resolutions._1080p].Add("btnR", new Point(1847, 30));
            points[Resolutions._1080p].Add("btnZL", new Point(0, 0));
            points[Resolutions._1080p].Add("btnZR", new Point(0, 0));
            points[Resolutions._1080p].Add("btnL3", new Point(0, 0));
            points[Resolutions._1080p].Add("btnR3", new Point(0, 0));

            
            return points;
        }
    }
}
