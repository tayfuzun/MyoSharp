using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace MyoSharp.Device
{
    /// <summary>
    /// A class that contains information about an arm recognition event from 
    /// the Myo.
    /// </summary>
    public class ArmRecognizedEventArgs : MyoEventArgs
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ArmRecognizedEventArgs"/> class.
        /// </summary>
        /// <param name="myo">The Myo that raised the event. Cannot be <c>null</c>.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        /// <param name="arm">The arm.</param>
        /// <param name="directionX">The direction x.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The exception that is thrown when <paramref name="myo"/> is null.
        /// </exception>
        public ArmRecognizedEventArgs(IMyo myo, DateTime timestamp, Arm arm, XDirection directionX)
            : base(myo, timestamp)
        {
            Contract.Requires<ArgumentNullException>(myo != null, "myo");

            this.Arm = arm;
            this.XDirection = directionX;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the arm that the Myo detected itself on.
        /// </summary>
        public Arm Arm { get; private set; }

        /// <summary>
        /// Gets the x direction.
        /// </summary>
        public XDirection XDirection { get; private set; }
        #endregion
    }
}