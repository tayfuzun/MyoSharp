using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Device
{
    public interface IMyoEventGenerator
    {
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

        event EventHandler<MyoEventArgs> Locked;

        event EventHandler<MyoEventArgs> Unlocked;

        event EventHandler<EmgDataEventArgs> EmgDataAcquired;
        #endregion
    }
}
