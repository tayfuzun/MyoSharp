using System;

using MyoSharp.Discovery;

namespace MyoSharp.Device
{
    public interface IMyo : IDisposable
    {
        #region Properties
        IntPtr Handle { get; }
        #endregion

        #region Events
        event EventHandler<MyoEventArgs> Connected;

        event EventHandler<MyoEventArgs> Disconnected;

        event EventHandler<ArmRecognizedEventArgs> ArmRecognized;

        event EventHandler<MyoEventArgs> ArmLost;

        event EventHandler<PoseEventArgs> PoseChanged;

        event EventHandler<OrientationDataEventArgs> OrientationDataAcquired;

        event EventHandler<AccelerometerDataEventArgs> AccelerometerDataAcquired;

        event EventHandler<GyroscopeDataEventArgs> GyroscopeDataAcquired;

        event EventHandler<RssiEventArgs> Rssi;
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