using System;
using System.Collections.Generic;
using System.Text;

using MyoSharp.Math;

namespace MyoSharp.Device
{
    public class OrientationDataEventArgs : MyoEventArgs
    {
        #region Constructors
        public OrientationDataEventArgs(IMyo myo, DateTime timestamp, QuaternionF orientation, double roll, double pitch, double yaw)
            : base(myo, timestamp)
        {
            this.Orientation = orientation;
            this.Roll = roll;
            this.Pitch = pitch;
            this.Yaw = yaw;
        }
        #endregion

        #region Properties
        public double Roll { get; private set; }
        public double Yaw { get; private set; }
        public double Pitch { get; private set; }
        internal QuaternionF Orientation { get; private set; }
        #endregion
    }
}
