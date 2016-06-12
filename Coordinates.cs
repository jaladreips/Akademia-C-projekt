using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AccelTest
{
    delegate void CoordinatesChanged(Coordinates args);
    class Coordinates
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Theta { get; set; }
        public double Phi { get; set; }

        public double Vx { get; set; }
        public double Vy { get; set; }
        public double Vz { get; set; }
        public double OmegaP { get; set; }
        public double OmegaT { get; set; }
    }
}