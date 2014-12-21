using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Device
{
    /// <summary>
    /// A class that contains information about an event raised by the Myo.
    /// </summary>
    public class MyoEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MyoEventArgs" /> class.
        /// </summary>
        /// <param name="myo">The Myo that raised the event. Cannot be <c>null</c>.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The exception that is thrown when <paramref name="myo"/> is <c>null</c>.
        /// </exception>
        public MyoEventArgs(IMyo myo, DateTime timestamp)
        {
            if (myo == null)
            {
                throw new ArgumentNullException("myo", "The Myo cannot be null.");
            }

            this.Myo = myo;
            this.Timestamp = timestamp;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the Myo that raised the event.
        /// </summary>
        public IMyo Myo { get; private set; }

        /// <summary>
        /// Gets the timestamp of the event.
        /// </summary>
        public DateTime Timestamp { get; private set; }
        #endregion
    }
}