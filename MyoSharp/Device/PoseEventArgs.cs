using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

using MyoSharp.Poses;

namespace MyoSharp.Device
{
    /// <summary>
    /// A class that contains information about a pose recognition event from the Myo.
    /// </summary>
    public class PoseEventArgs : MyoEventArgs
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PoseEventArgs"/> class.
        /// </summary>
        /// <param name="myo">The Myo that raised the event. Cannot be <c>null</c>.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        /// <param name="pose">The pose that the Myo detected.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The exception that is thrown when <paramref name="myo"/> is null.
        /// </exception>
        public PoseEventArgs(IMyo myo, DateTime timestamp, Pose pose)
            : base(myo, timestamp)
        {
            Contract.Requires<ArgumentNullException>(myo != null, "myo");

            this.Pose = pose;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the pose that the Myo detected.
        /// </summary>
        public Pose Pose { get; private set; }
        #endregion
    }
}
