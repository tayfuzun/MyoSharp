using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

using MyoSharp.Poses;
using MyoSharp.Communication;

namespace MyoSharp.Device
{
    /// <summary>
    /// An interface that defines functionality for bridging the Myo interface between this library and another.
    /// </summary>
    [ContractClass(typeof(IMyoDeviceBridgeContract))]
    public interface IMyoDeviceBridge
    {
        #region Methods
        MyoResult Vibrate32(IntPtr myo, VibrationType type, out IntPtr error);

        MyoResult Vibrate64(IntPtr myo, VibrationType type, out IntPtr error);

        MyoResult RequestRssi32(IntPtr myo, out IntPtr error);

        MyoResult RequestRssi64(IntPtr myo, out IntPtr error);

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

        MyoResult Unlock32(IntPtr myo, UnlockType type, out IntPtr error);

        MyoResult Unlock64(IntPtr myo, UnlockType type, out IntPtr error);

        MyoResult Lock32(IntPtr myo, out IntPtr error);

        MyoResult Lock64(IntPtr myo, out IntPtr error);

        MyoResult StreamEmg32(IntPtr myo, StreamEmgType streamEmgType, out IntPtr error);

        MyoResult StreamEmg64(IntPtr myo, StreamEmgType streamEmgType, out IntPtr error);

        sbyte EventGetEmg32(IntPtr evt, int sensor);

        sbyte EventGetEmg64(IntPtr evt, int sensor);
        #endregion
    }

    [ContractClassFor(typeof(IMyoDeviceBridge))]
    internal abstract class IMyoDeviceBridgeContract : IMyoDeviceBridge
    {
        #region Methods
        public MyoResult Vibrate32(IntPtr myo, VibrationType type, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(myo != IntPtr.Zero, "The pointer to the Myo must be set.");

            error = default(IntPtr);
            return default(MyoResult);
        }

        public MyoResult Vibrate64(IntPtr myo, VibrationType type, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(myo != IntPtr.Zero, "The pointer to the Myo must be set.");

            error = default(IntPtr);
            return default(MyoResult);
        }

        public MyoResult RequestRssi32(IntPtr myo, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(myo != IntPtr.Zero, "The pointer to the Myo must be set.");

            error = default(IntPtr);
            return default(MyoResult);
        }

        public MyoResult RequestRssi64(IntPtr myo, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(myo != IntPtr.Zero, "The pointer to the Myo must be set.");

            error = default(IntPtr);
            return default(MyoResult);
        }

        public uint EventGetFirmwareVersion32(IntPtr evt, VersionComponent component)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The pointer to the event must be set.");

            return default(uint);
        }

        public uint EventGetFirmwareVersion64(IntPtr evt, VersionComponent component)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The pointer to the event must be set.");

            return default(uint);
        }

        public Arm EventGetArm32(IntPtr evt)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The pointer to the event must be set.");

            return default(Arm);
        }

        public Arm EventGetArm64(IntPtr evt)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The pointer to the event must be set.");

            return default(Arm);
        }

        public XDirection EventGetXDirection32(IntPtr evt)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The pointer to the event must be set.");

            return default(XDirection);
        }

        public XDirection EventGetXDirection64(IntPtr evt)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The pointer to the event must be set.");

            return default(XDirection);
        }

        public float EventGetOrientation32(IntPtr evt, OrientationIndex index)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The pointer to the event must be set.");

            return default(float);
        }

        public float EventGetOrientation64(IntPtr evt, OrientationIndex index)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The pointer to the event must be set.");

            return default(float);
        }

        public float EventGetAccelerometer32(IntPtr evt, uint index)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The pointer to the event must be set.");

            return default(float);
        }

        public float EventGetAccelerometer64(IntPtr evt, uint index)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The pointer to the event must be set.");

            return default(float);
        }

        public float EventGetGyroscope32(IntPtr evt, uint index)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The pointer to the event must be set.");

            return default(float);
        }

        public float EventGetGyroscope64(IntPtr evt, uint index)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The pointer to the event must be set.");

            return default(float);
        }

        public Pose EventGetPose32(IntPtr evt)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The pointer to the event must be set.");

            return default(Pose);
        }

        public Pose EventGetPose64(IntPtr evt)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The pointer to the event must be set.");

            return default(Pose);
        }

        public sbyte EventGetRssi32(IntPtr evt)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The pointer to the event must be set.");

            return default(sbyte);
        }

        public sbyte EventGetRssi64(IntPtr evt)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The pointer to the event must be set.");

            return default(sbyte);
        }

        public MyoResult Unlock32(IntPtr myo, UnlockType type, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(myo != IntPtr.Zero, "The pointer to Myo event must be set.");

            error = default(IntPtr);
            return default(MyoResult);
        }

        public MyoResult Unlock64(IntPtr myo, UnlockType type, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(myo != IntPtr.Zero, "The pointer to Myo event must be set.");

            error = default(IntPtr);
            return default(MyoResult);
        }

        public MyoResult Lock32(IntPtr myo, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(myo != IntPtr.Zero, "The pointer to Myo event must be set.");

            error = default(IntPtr);
            return default(MyoResult);
        }

        public MyoResult Lock64(IntPtr myo, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(myo != IntPtr.Zero, "The pointer to Myo event must be set.");

            error = default(IntPtr);
            return default(MyoResult);
        }

        public MyoResult StreamEmg32(IntPtr myo, StreamEmgType streamEmgType, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(myo != IntPtr.Zero, "The pointer to Myo event must be set.");

            error = default(IntPtr);
            return default(MyoResult);
        }

        public MyoResult StreamEmg64(IntPtr myo, StreamEmgType streamEmgType, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(myo != IntPtr.Zero, "The pointer to Myo event must be set.");

            error = default(IntPtr);
            return default(MyoResult);
        }

        public sbyte EventGetEmg32(IntPtr evt, int sensor)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The pointer to the event must be set.");
            Contract.Requires<ArgumentException>(sensor >= 0, "The sensor index must be greater than or equal to zero.");

            return default(sbyte);
        }

        public sbyte EventGetEmg64(IntPtr evt, int sensor)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The pointer to the event must be set.");
            Contract.Requires<ArgumentException>(sensor >= 0, "The sensor index must be greater than or equal to zero.");

            return default(sbyte);
        }
        #endregion
    }
}
