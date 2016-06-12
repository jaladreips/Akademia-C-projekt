using System.Collections.Generic;
using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Hardware;

namespace AccelTest
{
    [Activity(Label = "Acceleration Sensor Test", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {

        enum Axis { AxisX, AxisY, AxisZ };
        Axis ChosenAxis = Axis.AxisX;

        Accelerometer accelerometer;
        CurrentCoordinates currentCoords;


        TextView posText, velText;
        TextView accText, gravText;
        TextView linText;
        RadioButton RadioX, RadioY, RadioZ;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);


            Button button = FindViewById<Button>(Resource.Id.button1);

            velText = FindViewById<TextView>(Resource.Id.velocitytext);
            posText = FindViewById<TextView>(Resource.Id.positiontext);

            accText = FindViewById<TextView>(Resource.Id.accelerationtext);
            gravText = FindViewById<TextView>(Resource.Id.gravitytext);

            linText = FindViewById<TextView>(Resource.Id.linearaccelerationtext);

            RadioX = FindViewById<RadioButton>(Resource.Id.radioAxisX);
            RadioY = FindViewById<RadioButton>(Resource.Id.radioAxisY);
            RadioZ = FindViewById<RadioButton>(Resource.Id.radioAxisZ);

            SensorManager sensor = (SensorManager)GetSystemService(SensorService);
            accelerometer = new Accelerometer(this, sensor);
            if (accelerometer.isLinearAccelerationAvaliable)
                linText.Text = "Linear acceleration is ON";
            else
                linText.Text = "Linear aceleration is OFF";



            currentCoords = new CurrentCoordinates();
            accelerometer.sensorChanged = new SensorChanged(currentCoords.IntegrateCoordinates);
            accelerometer.sensorChanged += UpdateTexts;

            currentCoords.CoordsChanged = new CoordinatesChanged(UpdateTexts);

            button.Text = "Reset position";
            button.Click += (object sender, EventArgs e) =>
            {
                currentCoords.Reset();
            };

            RadioX.Checked = true;
            RadioX.Click += (object sender, EventArgs e) => { ChosenAxis = Axis.AxisX; };
            RadioY.Click += (object sender, EventArgs e) => { ChosenAxis = Axis.AxisY; };
            RadioZ.Click += (object sender, EventArgs e) => { ChosenAxis = Axis.AxisZ; };



        }

        void UpdateTexts(Coordinates args)
        {
            switch (ChosenAxis)
            {
                case (Axis.AxisX):
                        posText.Text = string.Format("X = {0}", args.X);
                        velText.Text = string.Format("V.x = {0}", args.Vx);
                        break;
                case (Axis.AxisY):
                        posText.Text = string.Format("Y = {0}", args.Y);
                        velText.Text = string.Format("V.y = {0}", args.Vx);
                        break;
                case (Axis.AxisZ):
                        posText.Text = string.Format("Z = {0}", args.Z);
                        velText.Text = string.Format("V.z = {0}", args.Vx);
                        break;
            }
        }

        void UpdateTexts(OnSensorChangedArgs args)
        {
            switch (ChosenAxis)
            {
                case (Axis.AxisX):
                        accText.Text = string.Format("A.x = {0}", args.AccelerationX);
                        gravText.Text = string.Format("G.x = {0}", args.GravityX);
                        break;
                case (Axis.AxisY):
                        accText.Text = string.Format("A.y = {0}", args.AccelerationY);
                        gravText.Text = string.Format("G.y = {0}", args.GravityX);
                        break;
                case (Axis.AxisZ):
                        accText.Text = string.Format("A.z = {0}", args.AccelerationZ);
                        gravText.Text = string.Format("G.z = {0}", args.GravityX);
                        break;
            }
        }


    }
}

