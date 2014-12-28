using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Device
{
    /// <summary>
    /// An interface that defines functionality for events that can be generated from a Myo device.
    /// </summary>
    public interface IMyoEventGenerator
    {
        #region Events
        /// <summary>
        /// The event that is triggered when a Myo has connected.
        /// </summary>
        event EventHandler<MyoEventArgs> Connected;

        /// <summary>
        /// The event that is triggered when a Myo has disconnected.
        /// </summary>
        event EventHandler<MyoEventArgs> Disconnected;

        /// <summary>
        /// The event that is triggered when the arm a Myo is on is recognized.
        /// </summary>
        event EventHandler<ArmRecognizedEventArgs> ArmRecognized;

        /// <summary>
        /// The event that is triggered when the arm a Myo is on is no longer known.
        /// </summary>
        event EventHandler<MyoEventArgs> ArmLost;

        /// <summary>
        /// The event that is triggered when the Myo's pose has changed.
        /// </summary>
        event EventHandler<PoseEventArgs> PoseChanged;

        /// <summary>
        /// The event that is triggered when orientation data has been acquired from the Myo.
        /// </summary>
        event EventHandler<OrientationDataEventArgs> OrientationDataAcquired;

        /// <summary>
        /// The event that is triggered when accelerometer data has been acquired from the Myo.
        /// </summary>
        event EventHandler<AccelerometerDataEventArgs> AccelerometerDataAcquired;

        /// <summary>
        /// The event that is triggered when gyroscope data has been acquired from the Myo.
        /// </summary>
        event EventHandler<GyroscopeDataEventArgs> GyroscopeDataAcquired;

        /// <summary>
        /// The event that is triggered when received signal strength 
        /// indication information has been acquired from the Myo.
        /// </summary>
        /// <remarks>
        /// For more information on RSSI, see:
        /// http://en.wikipedia.org/wiki/Received_signal_strength_indication
        /// </remarks>
        event EventHandler<RssiEventArgs> Rssi;

        /// <summary>
        /// The event that is triggered when the Myo enters the lock state.
        /// </summary>
        event EventHandler<MyoEventArgs> Locked;

        /// <summary>
        /// The event that is triggered when the Myo enters the unlock state.
        /// </summary>
        event EventHandler<MyoEventArgs> Unlocked;

        /// <summary>
        /// The event that is triggered when EMG data is acquired from the Myo.
        /// </summary>
        event EventHandler<EmgDataEventArgs> EmgDataAcquired;
        #endregion
    }
}
