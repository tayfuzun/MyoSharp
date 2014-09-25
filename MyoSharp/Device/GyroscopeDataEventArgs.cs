using System;
using System.Collections.Generic;
using System.Text;

using MyoSharp.Math;

namespace MyoSharp.Device
{
    public class GyroscopeDataEventArgs : MyoEventArgs
    {
        #region Constructors
        public GyroscopeDataEventArgs(IMyo myo, DateTime timestamp, Vector3F gyroscope)
            : base(myo, timestamp)
        {
            this.Gyroscope = gyroscope;
        }
        #endregion

        #region Properties
        public Vector3F Gyroscope { get; private set; }
        #endregion
    }
}