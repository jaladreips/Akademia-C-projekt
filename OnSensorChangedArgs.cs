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
    class OnSensorChangedArgs : EventArgs
    {
        private const double nanoToSI = 10e-9;

        public double AccelerationX { get; private set; }
        public double AccelerationY { get; private set; }
        public double AccelerationZ { get; private set; }

        public double GravityX { get; private set; }
        public double GravityY { get; private set; }
        public double GravityZ { get; private set; }

        public double TimeStep { get; private set; }

        public OnSensorChangedArgs(double in_AccelerationX, double in_AccelerationY, double in_AccelerationZ,
            double in_GravityX, double in_GravityY, double in_GravityZ, double in_TimeStep)
        {
            AccelerationX = in_AccelerationX;
            AccelerationY = in_AccelerationY;
            AccelerationZ = in_AccelerationZ;
            
            GravityX = in_GravityX;
            GravityY = in_GravityY;
            GravityZ = in_GravityZ;
            
            TimeStep = in_TimeStep * nanoToSI;

        }
    }
}