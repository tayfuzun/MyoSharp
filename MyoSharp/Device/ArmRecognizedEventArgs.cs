using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Device
{
    public class ArmRecognizedEventArgs : MyoEventArgs
    {
        #region Constructors
        public ArmRecognizedEventArgs(IMyo myo, DateTime timestamp, Arm arm, XDirection xDirection)
            : base(myo, timestamp)
        {
            this.Arm = arm;
            this.XDirection = xDirection;
        }
        #endregion

        #region Properties
        public Arm Arm { get; private set; }

        public XDirection XDirection { get; private set; }
        #endregion
    }
}