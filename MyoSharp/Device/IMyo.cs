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
        #endregion
    }
}