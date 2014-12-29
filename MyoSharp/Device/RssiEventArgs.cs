using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace MyoSharp.Device
{
    /// <summary>
    /// A class that contains information about RSSI from a Myo event.
    /// </summary>
    public class RssiEventArgs : MyoEventArgs
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RssiEventArgs"/> class.
        /// </summary>
        /// <param name="myo">The Myo that raised the event. Cannot be <c>null</c>.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        /// <param name="rssi">The received signal strength indicator (RSSI).</param>
        /// <exception cref="System.ArgumentNullException">
        /// The exception that is thrown when <paramref name="myo"/> is null.
        /// </exception>
        public RssiEventArgs(IMyo myo, DateTime timestamp, sbyte rssi)
            : base(myo, timestamp)
        {
            Contract.Requires<ArgumentNullException>(myo != null, "myo");

            this.Rssi = rssi;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the received signal strength indicator (RSSI).
        /// </summary>
        public sbyte Rssi { get; private set; }
        #endregion
    }
}
