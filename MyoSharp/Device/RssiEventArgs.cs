using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Device
{
    public class RssiEventArgs : MyoEventArgs
    {
        #region Constructors
        public RssiEventArgs(Myo myo, DateTime timestamp, sbyte rssi)
            : base(myo, timestamp)
        {
            this.Rssi = rssi;
        }
        #endregion

        #region Properties
        public sbyte Rssi { get; private set; }
        #endregion
    }
}
