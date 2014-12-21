using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Device
{
    public class EmgDataEventArgs : MyoEventArgs
    {
        #region Constructors
        public EmgDataEventArgs(IMyo myo, DateTime timestamp, IEmgData emgData)
            : base(myo, timestamp)
        {
            this.EmgData = emgData;
        }
        #endregion

        #region Properties
        public IEmgData EmgData { get; private set; }
        #endregion
    }
}