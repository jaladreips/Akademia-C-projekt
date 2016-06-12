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
    class CurrentCoordinates
    {

        public Coordinates Coords;
        public CoordinatesChanged CoordsChanged;


        public void Reset()
        {
            Coords.X = 0;
            Coords.Y = 0;
            Coords.Z = 0;
            Coords.Theta = 0;
            Coords.Phi = 0;
            Coords.Vx = 0;
            Coords.Vy = 0;
            Coords.Vz = 0;
            Coords.OmegaP = 0;
            Coords.OmegaT = 0;
        }

        public CurrentCoordinates()
        {
            Coords = new Coordinates();
            Reset();
        }
        
        public void IntegrateCoordinates(OnSensorChangedArgs args)
        {
            Coords.X += Coords.Vx * args.TimeStep;
            Coords.Y += Coords.Vy * args.TimeStep;
            Coords.Z += Coords.Vz * args.TimeStep;

            Coords.Vx += args.AccelerationX * args.TimeStep;
            Coords.Vy += args.AccelerationY * args.TimeStep;
            Coords.Vz += args.AccelerationZ * args.TimeStep;

            CoordsChanged(Coords);
        }

    }
}