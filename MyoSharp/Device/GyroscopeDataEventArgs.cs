using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using MyoSharp.Math;

namespace MyoSharp.Device
{
    /// <summary>
    /// A class that contains information about a gyroscope data event from the Myo.
    /// </summary>
    public class GyroscopeDataEventArgs : MyoEventArgs
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GyroscopeDataEventArgs"/> class.
        /// </summary>
        /// <param name="myo">The Myo that raised the event. Cannot be <c>null</c>.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        /// <param name="gyroscopeData">The gyroscope data. Cannot be <c>null</c>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The exception that is thrown when <paramref name="myo"/> or <paramref name="gyroscopeData"/> is null.
        /// </exception>
        public GyroscopeDataEventArgs(IMyo myo, DateTime timestamp, Vector3F gyroscopeData)
            : base(myo, timestamp)
        {
            Contract.Requires<ArgumentNullException>(myo != null, "myo");
            Contract.Requires<ArgumentNullException>(gyroscopeData != null, "gyroscopeData");
            
            this.Gyroscope = gyroscopeData;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the gyroscope data.
        /// </summary>
        public Vector3F Gyroscope { get; private set; }
        #endregion
    }
}