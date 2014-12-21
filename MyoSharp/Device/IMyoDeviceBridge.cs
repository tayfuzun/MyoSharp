using System;
using System.Collections.Generic;
using System.Text;

using MyoSharp.Poses;

namespace MyoSharp.Device
{
    /// <summary>
    /// An interface that defines functionality for bridging the Myo interface between this library and another.
    /// </summary>
    public interface IMyoDeviceBridge
    {
        #region Methods
        void Vibrate32(IntPtr myo, VibrationType type, IntPtr error);

        void Vibrate64(IntPtr myo, VibrationType type, IntPtr error);

        void RequestRssi32(IntPtr myo, IntPtr error);

        void RequestRssi64(IntPtr myo, IntPtr error);

        uint EventGetFirmwareVersion32(IntPtr evt, VersionComponent component);

        uint EventGetFirmwareVersion64(IntPtr evt, VersionComponent component);

        Arm EventGetArm32(IntPtr evt);

        Arm EventGetArm64(IntPtr evt);

        XDirection EventGetXDirection32(IntPtr evt);

        XDirection EventGetXDirection64(IntPtr evt);

        float EventGetOrientation32(IntPtr evt, OrientationIndex index);

        float EventGetOrientation64(IntPtr evt, OrientationIndex index);

        float EventGetAccelerometer32(IntPtr evt, uint index);

        float EventGetAccelerometer64(IntPtr evt, uint index);

        float EventGetGyroscope32(IntPtr evt, uint index);

        float EventGetGyroscope64(IntPtr evt, uint index);

        Pose EventGetPose32(IntPtr evt);

        Pose EventGetPose64(IntPtr evt);

        sbyte EventGetRssi32(IntPtr evt);

        sbyte EventGetRssi64(IntPtr evt);

        void Unlock32(IntPtr myo, UnlockType type, IntPtr error);

        void Unlock64(IntPtr myo, UnlockType type, IntPtr error);

        void Lock32(IntPtr myo, IntPtr error);

        void Lock64(IntPtr myo, IntPtr error);

        void StreamEmg32(IntPtr myo, StreamEmgType streamEmgType, IntPtr error);

        void StreamEmg64(IntPtr myo, StreamEmgType streamEmgType, IntPtr error);

        sbyte EventGetEmg32(IntPtr evt, int sensor);

        sbyte EventGetEmg64(IntPtr evt, int sensor);
        #endregion
    }
}
