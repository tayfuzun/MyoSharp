using System;
using System.Collections.Generic;
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
        /// <param name="myo">The Myo that raised this event.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        /// <param name="emgData">The EMG data that corresponds to this event. Cannot be <c>null</c>.</param>
        public EmgDataEventArgs(IMyo myo, DateTime timestamp, IEmgData emgData)
            : base(myo, timestamp)
        {
            if (emgData == null)
            {
                throw new ArgumentNullException("emgData", "The EMG data cannot be null.");
            }

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