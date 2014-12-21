using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Discovery
{
    /// <summary>
    /// A class that contains information about a paired event for a device.
    /// </summary>
    public class PairedEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PairedEventArgs"/> class.
        /// </summary>
        /// <param name="myoHandle">The myo Handle.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        public PairedEventArgs(IntPtr myoHandle, DateTime timestamp)
        {
            this.MyoHandle = myoHandle;
            this.Timestamp = timestamp;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the Myo handle.
        /// </summary>
        public IntPtr MyoHandle { get; private set; }

        /// <summary>
        /// Gets the timestamp of the event.
        /// </summary>
        public DateTime Timestamp { get; private set; }
        #endregion
    }
}
