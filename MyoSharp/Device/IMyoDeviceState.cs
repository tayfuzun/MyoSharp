using System;
using System.Collections.Generic;
using System.Text;

using MyoSharp.Math;
using MyoSharp.Poses;

namespace MyoSharp.Device
{
    /// <summary>
    /// An interface that defines functionality for Myo state.
    /// </summary>
    public interface IMyoDeviceState
    {
        #region Properties
        /// <summary>
        /// Gets a value indicating whether or not the Myo is unlocked.
        /// </summary>
        bool IsUnlocked { get; }

        /// <summary>
        /// Gets a value indicating whether or not the Myo is connected.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Gets the arm that the Myo is on.
        /// </summary>
        Arm Arm { get; }

        /// <summary>
        /// Gets last known pose from the Myo.
        /// </summary>
        Pose Pose { get; }

        /// <summary>
        /// Gets the X-direction of the Myo on the arm.
        /// </summary>
        XDirection XDirectionOnArm { get; }

        /// <summary>
        /// Gets the last known orientation data from the Myo.
        /// </summary>
        QuaternionF Orientation { get; }

        /// <summary>
        /// Gets the last known accelerometer data from the Myo.
        /// </summary>
        Vector3F Accelerometer { get; }

        /// <summary>
        /// Gets the last known gyroscope data from the Myo.
        /// </summary>
        Vector3F Gyroscope { get; }

        /// <summary>
        /// Gets the last known EMG data from the Myo.
        /// </summary>
        IEmgData EmgData { get; }
        #endregion
    }
}
