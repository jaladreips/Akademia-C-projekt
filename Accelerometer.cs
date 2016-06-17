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
using Android.Hardware;

namespace AccelTest
{
    delegate void SensorChanged(OnSensorChangedArgs args); //Update everything when sensor's readings change
    class Accelerometer : View, ISensorEventListener
    {
        private SensorManager sensor;
        private Sensor accelSensor, gravSensor;
        public SensorChanged sensorChanged;
        static readonly object _syncLock = new object(); //No idea what that is right now

        private double lastTimeUpdated = 0;

        private int numberOfSamplesTaken = 500;
        private double[] weights;

        public List<double> AccelerationX { get; private set; }
        public List<double> AccelerationY { get; private set; }
        public List<double> AccelerationZ { get; private set; }

        public double GravityX { get; private set; } = 0.0;
        public double GravityY { get; private set; } = 0.0;
        public double GravityZ { get; private set; } = 0.0;

        public double filteredAccelerationX { get; private set; } = 0.0;
        public double filteredAccelerationY { get; private set; } = 0.0;
        public double filteredAccelerationZ { get; private set; } = 0.0;

        public bool isLinearAccelerationAvaliable { get; private set; }



        public Accelerometer(Context in_context, SensorManager in_sensor) : base(in_context)
        {
            double[] initValues = new double[numberOfSamplesTaken];
            weights = new double[numberOfSamplesTaken];

            for (int i = 0; i < numberOfSamplesTaken; i++)
            {
                initValues[i] = 0.0;
                weights[i] = (i + 1) * 2.0/((numberOfSamplesTaken + 1.0) * numberOfSamplesTaken);

            }

            AccelerationX = new List<double>(initValues);
            AccelerationY = new List<double>(initValues);
            AccelerationZ = new List<double>(initValues);

            sensor = in_sensor;
            accelSensor = sensor.GetDefaultSensor(SensorType.LinearAcceleration);
            gravSensor = sensor.GetDefaultSensor(SensorType.Gravity);
            if(accelSensor == null || gravSensor == null)
            {
                isLinearAccelerationAvaliable = false;
            }
            accelSensor = sensor.GetDefaultSensor(SensorType.Accelerometer);
            TurnSensorOn();
        }

        public void TurnSensorOn()
        {
            sensor.RegisterListener(this, accelSensor, SensorDelay.Fastest);
        }

        public void TurnSensorOff()
        {
            sensor.UnregisterListener(this, accelSensor);
        }
        
        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            // We don't want to do anything here. Just remember that something like that exists
        }

        public void OnSensorChanged(SensorEvent e)
        {
            lock (_syncLock)
            {
                double alpha = 0.8;

                double newAccelerationX, newAccelerationY, newAccelerationZ;

                AccelerationX.Add(e.Values[0]);
                AccelerationX.RemoveAt(0);
                AccelerationY.Add(e.Values[1]);
                AccelerationY.RemoveAt(0);
                AccelerationZ.Add(e.Values[2]);
                AccelerationZ.RemoveAt(0);

                newAccelerationX = 0.0;
                newAccelerationY = 0.0;
                newAccelerationZ = 0.0;

                for (int i = 0; i < numberOfSamplesTaken; i++)
                {
                    newAccelerationX += weights[i] * AccelerationX[i];
                    newAccelerationY += weights[i] * AccelerationY[i];
                    newAccelerationZ += weights[i] * AccelerationZ[i];
                }

                GravityX = alpha * GravityX + (1 - alpha) * newAccelerationX;
                GravityY = alpha * GravityY + (1 - alpha) * newAccelerationY;
                GravityZ = alpha * GravityZ + (1 - alpha) * newAccelerationZ;

                filteredAccelerationX = newAccelerationX - GravityX;
                filteredAccelerationY = newAccelerationY - GravityY;
                filteredAccelerationZ = newAccelerationZ - GravityZ;





                if (lastTimeUpdated == 0) lastTimeUpdated = e.Timestamp;

                sensorChanged(new OnSensorChangedArgs(filteredAccelerationX, filteredAccelerationY, filteredAccelerationZ,
                GravityX, GravityY, GravityZ, e.Timestamp - lastTimeUpdated));

                lastTimeUpdated = e.Timestamp;

            }
        
        }

        
    }
}