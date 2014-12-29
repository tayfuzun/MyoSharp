using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

using MyoSharp.Math;

namespace MyoSharp.Device
{
    /// <summary>
    /// A class that contains information about a Myo orientation event.
    /// </summary>
    public class OrientationDataEventArgs : MyoEventArgs
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="OrientationDataEventArgs" /> class.
        /// </summary>
        /// <param name="myo">The Myo that raised the event. Cannot be <c>null</c>.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        /// <param name="orientationData">The orientation.</param>
        /// <param name="roll">The roll.</param>
        /// <param name="pitch">The pitch.</param>
        /// <param name="yaw">The yaw.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The exception that is thrown when <paramref name="myo"/> is <c>null</c>.
        /// </exception>
        public OrientationDataEventArgs(IMyo myo, DateTime timestamp, QuaternionF orientationData, double roll, double pitch, double yaw)
            : base(myo, timestamp)
        {
            Contract.Requires<ArgumentNullException>(myo != null, "myo");
            Contract.Requires<ArgumentNullException>(orientationData != null, "orientationData");

            this.Orientation = orientationData;
            this.Roll = roll;
            this.Pitch = pitch;
            this.Yaw = yaw;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the roll.
        /// </summary>
        public double Roll { get; private set; }

        /// <summary>
        /// Gets the yaw.
        /// </summary>
        public double Yaw { get; private set; }

        /// <summary>
        /// Gets the pitch.
        /// </summary>
        public double Pitch { get; private set; }

        /// <summary>
        /// Gets the orientation.
        /// </summary>
        public QuaternionF Orientation { get; private set; }
        #endregion
    }
}
