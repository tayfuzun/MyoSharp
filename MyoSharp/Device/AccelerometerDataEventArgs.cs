using System;
using System.Collections.Generic;
using System.Text;

using MyoSharp.Math;

namespace MyoSharp.Device
{
    public class AccelerometerDataEventArgs : MyoEventArgs
    {
        #region Constructors
        public AccelerometerDataEventArgs(IMyo myo, DateTime timestamp, Vector3F accelerometer)
            : base(myo, timestamp)
        {
            this.Accelerometer = accelerometer;
        }
        #endregion

        #region Properties
        public Vector3F Accelerometer { get; private set; }
        #endregion
    }
}
