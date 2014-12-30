using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

using MyoSharp.Poses;

namespace MyoSharp.Device
{
    /// <summary>
    /// An interface that defines functionality for bridging the Myo interface between this library and another.
    /// </summary>
    [ContractClass(typeof(IMyoDeviceBridgeContract))]
    public interface IMyoDeviceBridge
    {
        #region Methods
        void Vibrate32(IntPtr myo, VibrationType type, out IntPtr error);

        void Vibrate64(IntPtr myo, VibrationType type, out IntPtr error);

        void RequestRssi32(IntPtr myo, out IntPtr error);

        void RequestRssi64(IntPtr myo, out IntPtr error);

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

        void Unlock32(IntPtr myo, UnlockType type, out IntPtr error);

        void Unlock64(IntPtr myo, UnlockType type, out IntPtr error);

        void Lock32(IntPtr myo, out IntPtr error);

        void Lock64(IntPtr myo, out IntPtr error);

        void StreamEmg32(IntPtr myo, StreamEmgType streamEmgType, out IntPtr error);

        void StreamEmg64(IntPtr myo, StreamEmgType streamEmgType, out IntPtr error);

        sbyte EventGetEmg32(IntPtr evt, int sensor);

        sbyte EventGetEmg64(IntPtr evt, int sensor);
        #endregion
    }

    [ContractClassFor(typeof(IMyoDeviceBridge))]
    internal abstract class IMyoDeviceBridgeContract : IMyoDeviceBridge
    {
        #region Methods
        public void Vibrate32(IntPtr myo, VibrationType type, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(myo != IntPtr.Zero, "The pointer to the Myo must be set.");

            error = default(IntPtr);
        }

        public void Vibrate64(IntPtr myo, VibrationType type, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(myo != IntPtr.Zero, "The pointer to the Myo must be set.");

            error = default(IntPtr);
        }

        public void RequestRssi32(IntPtr myo, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(myo != IntPtr.Zero, "The pointer to the Myo must be set.");

            error = default(IntPtr);
        }

        public void RequestRssi64(IntPtr myo, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(myo != IntPtr.Zero, "The pointer to the Myo must be set.");

            error = default(IntPtr);
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

        public void Unlock32(IntPtr myo, UnlockType type, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(myo != IntPtr.Zero, "The pointer to Myo event must be set.");

            error = default(IntPtr);
        }

        public void Unlock64(IntPtr myo, UnlockType type, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(myo != IntPtr.Zero, "The pointer to Myo event must be set.");

            error = default(IntPtr);
        }

        public void Lock32(IntPtr myo, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(myo != IntPtr.Zero, "The pointer to Myo event must be set.");

            error = default(IntPtr);
        }

        public void Lock64(IntPtr myo, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(myo != IntPtr.Zero, "The pointer to Myo event must be set.");

            error = default(IntPtr);
        }

        public void StreamEmg32(IntPtr myo, StreamEmgType streamEmgType, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(myo != IntPtr.Zero, "The pointer to Myo event must be set.");

            error = default(IntPtr);
        }

        public void StreamEmg64(IntPtr myo, StreamEmgType streamEmgType, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(myo != IntPtr.Zero, "The pointer to Myo event must be set.");

            error = default(IntPtr);
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
