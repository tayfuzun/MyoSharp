using System;

namespace MyoSharp.Device
{
    public interface IMyo : IMyoEventGenerator, IMyoDeviceState, IDisposable
    {
        #region Properties
        IntPtr Handle { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Causes the Myo to vibrate.
        /// </summary>
        /// <param name="type">The type of vibration.</param>
        void Vibrate(VibrationType type);

        /// <summary>
        /// Requests RSSI from the Myo.
        /// </summary>
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