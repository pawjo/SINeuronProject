using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SINeuronWPFApp
{
    public static class Configuration
    {
        public static double PointSize = 20;

        public static double PointOffset = 10;

        public static Brush PointBorderBrush = Brushes.Black;

        public static Brush PointBackgroundBrush = Brushes.Gray;

        public static Brush ActivePointBorderBrush = Brushes.DarkGray;

        public static Brush ActivePointBackgroundBrush = Brushes.Blue;

        public static Brush AxesBrush = Brushes.Red;

        public static double SpaceCanvasWidth = 700;

        public static double SpaceCanvasHeight = 400;

        public static double SpaceCanvasXOffset = 350;

        public static double SpaceCanvasYOffset = 200;

        public static double LineLength = 1500;

        public static double LineOffset = SpaceCanvasWidth / 2 - LineLength / 2;
    }
}
