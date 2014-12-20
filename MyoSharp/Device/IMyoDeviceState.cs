using System;
using System.Collections.Generic;
using System.Text;

using MyoSharp.Math;
using MyoSharp.Poses;

namespace MyoSharp.Device
{
    public interface IMyoDeviceState
    {
        #region Properties
        bool IsUnlocked { get; }

        bool IsConnected { get; }

        Arm Arm { get; }

        Pose Pose { get; }

        QuaternionF Orientation { get; }

        Vector3F Accelerometer { get; }

        Vector3F Gyroscope { get; }
        #endregion
    }
}
