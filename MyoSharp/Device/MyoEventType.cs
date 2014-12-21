using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Device
{
    /// <summary>
    /// An enumeration of known Myo event types.
    /// </summary>
    public enum MyoEventType
    {
        /// <summary>
        /// Raised when the Myo has paired.
        /// </summary>
        Paired,

        /// <summary>
        /// Raised when the Myo is no longer paired.
        /// </summary>
        Unpaired,

        /// <summary>
        /// Raised when the Myo has connected.
        /// </summary>
        Connected,

        /// <summary>
        /// Raised when the Myo has disconnected.
        /// </summary>
        Disconnected,

        /// <summary>
        /// Raised when the Myo has detected which arm it is on.
        /// </summary>
        ArmRecognized,

        /// <summary>
        /// Raised when the Myo fails to recognize which arm it is on.
        /// </summary>
        ArmLost,

        /// <summary>
        /// Raised when the Myo is sending orientation information.
        /// </summary>
        Orientation,

        /// <summary>
        /// Raised when the Myo is sending pose information.
        /// </summary>
        Pose,

        /// <summary>
        /// Raised when the Myo is sending RSSI information.
        /// </summary>
        Rssi,

        /// <summary>
        /// Raised when the Myo has entered the unlocked state.
        /// </summary>
        Unlocked,

        /// <summary>
        /// Raised when the Myo has entered the locked state.
        /// </summary>
        Locked,

        /// <summary>
        /// Raised when the Myo is sending EMG information.
        /// </summary>
        Emg,
    }
}
