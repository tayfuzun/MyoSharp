using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Device
{
    public class PoseEventArgs : MyoEventArgs
    {
        #region Constructors
        public PoseEventArgs(IMyo myo, DateTime timestamp, Pose pose)
            : base(myo, timestamp)
        {
            this.Pose = pose;
        }
        #endregion

        #region Properties
        public Pose Pose { get; private set; }
        #endregion
    }
}
