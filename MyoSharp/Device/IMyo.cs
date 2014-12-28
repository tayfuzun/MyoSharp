using System;

namespace MyoSharp.Device
{
    /// <summary>
    /// An interface that defines functionality for a Myo device.
    /// </summary>
    public interface IMyo : IMyoEventGenerator, IMyoDeviceState, IDisposable
    {
        #region Properties
        /// <summary>
        /// Gets the handle for the Myo device.
        /// </summary>
        IntPtr Handle { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Causes the Myo to vibrate.
        /// </summary>
        /// <param name="type">The type of vibration.</param>
        void Vibrate(VibrationType type);

        /// <summary>
        /// Requests the received signal strength indication (RSSI) from the device.
        /// </summary>
        /// <remarks>
        /// For more information on RSSI, see:
        /// http://en.wikipedia.org/wiki/Received_signal_strength_indication
        /// </remarks>
        void RequestRssi();

        /// <summary>
        /// Causes the Myo to unlock.
        /// </summary>
        /// <param name="type">The type of unlock.</param>
        void Unlock(UnlockType type);

        /// <summary>
        /// Causes the Myo to lock.
        /// </summary>
        void Lock();

        /// <summary>
        /// Sets whether or not this Myo will have EMG streaming enabled or not.
        /// </summary>
        /// <param name="enabled">If set to <c>true</c>, EMG streaming will be enabled; Otherwise, it will be disabled.</param>
        void SetEmgStreaming(bool enabled);
        #endregion
    }
}