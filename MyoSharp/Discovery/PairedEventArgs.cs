using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Device
{
    public class PairedEventArgs : EventArgs
    {
        #region Constructors
        public PairedEventArgs(IntPtr myohandle, DateTime timestamp)
        {
            this.MyoHandle = myohandle;
            this.Timestamp = timestamp;
        }
        #endregion

        #region Properties
        public IntPtr MyoHandle { get; private set; }

        public DateTime Timestamp { get; private set; }
        #endregion
    }
}
