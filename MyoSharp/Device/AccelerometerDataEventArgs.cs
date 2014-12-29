using System;
using System.Collections.Generic;
using System.Text;

using MyoSharp.Math;

namespace MyoSharp.Device
{
    /// <summary>
    /// A class that contains information about an accelerometer event.
    /// </summary>
    public class AccelerometerDataEventArgs : MyoEventArgs
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AccelerometerDataEventArgs"/> class.
        /// </summary>
        /// <param name="myo">The Myo that raised the event. Cannot be <c>null</c>.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        /// <param name="accelerometerData">The accelerometer data. Cannot be <c>null</c>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The exception that is thrown when <paramref name="myo"/> or <paramref name="accelerometerData"/> is null.
        /// </exception>
        public AccelerometerDataEventArgs(IMyo myo, DateTime timestamp, Vector3F accelerometerData)
            : base(myo, timestamp)
        {
            // TODO: replace with contracts
            if (myo == null)
            {
                throw new ArgumentNullException("myo", "The Myo cannot be null.");
            }

            if (accelerometerData == null)
            {
                throw new ArgumentNullException("accelerometerData", "The accelerometer data cannot be null.");
            }

            this.Accelerometer = accelerometerData;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the accelerometer data.
        /// </summary>
        public Vector3F Accelerometer { get; private set; }
        #endregion
    }
}
