using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace MyoSharp.Device
{
    /// <summary>
    /// A class that contains information about an EMG-related event.
    /// </summary>
    public class EmgDataEventArgs : MyoEventArgs
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="EmgDataEventArgs"/> class.
        /// </summary>
        /// <param name="myo">The Myo that raised this event. Cannot be <c>null</c>.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        /// <param name="emgData">The EMG data that corresponds to this event. Cannot be <c>null</c>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The exception that is thrown when <paramref name="myo"/> or <paramref name="emgData"/> is null.
        /// </exception>
        public EmgDataEventArgs(IMyo myo, DateTime timestamp, IEmgData emgData)
            : base(myo, timestamp)
        {
            Contract.Requires<ArgumentNullException>(myo != null, "myo");
            Contract.Requires<ArgumentNullException>(emgData != null, "emgData");

            this.EmgData = emgData;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the EMG data.
        /// </summary>
        public IEmgData EmgData { get; private set; }
        #endregion
    }
}