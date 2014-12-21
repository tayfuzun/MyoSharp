using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using MyoSharp.Internal;
using MyoSharp.Poses;

namespace MyoSharp.Device
{
    public sealed class MyoDeviceBridge : IMyoDeviceBridge
    {
        #region Constructors
        /// <summary>
        /// Prevents a default instance of the <see cref="MyoDeviceBridge"/> class from being created.
        /// </summary>
        private MyoDeviceBridge()
        {
        }
        #endregion

        #region Methods
        public static IMyoDeviceBridge Create()
        {
            return new MyoDeviceBridge();
        }

        /// <inheritdoc />
        public void Vibrate32(IntPtr myo, VibrationType type, IntPtr error)
        {
            vibrate_32(myo, type, error);
        }

        /// <inheritdoc />
        public void Vibrate64(IntPtr myo, VibrationType type, IntPtr error)
        {
            vibrate_64(myo, type, error);
        }

        /// <inheritdoc />
        public void RequestRssi32(IntPtr myo, IntPtr error)
        {
            request_rssi_32(myo, error);
        }

        /// <inheritdoc />
        public void RequestRssi64(IntPtr myo, IntPtr error)
        {
            request_rssi_64(myo, error);
        }

        /// <inheritdoc />
        public uint EventGetFirmwareVersion32(IntPtr evt, VersionComponent component)
        {
            return event_get_firmware_version_32(evt, component);
        }

        /// <inheritdoc />
        public uint EventGetFirmwareVersion64(IntPtr evt, VersionComponent component)
        {
            return event_get_firmware_version_64(evt, component);
        }

        /// <inheritdoc />
        public Arm EventGetArm32(IntPtr evt)
        {
            return event_get_arm_32(evt);
        }

        /// <inheritdoc />
        public Arm EventGetArm64(IntPtr evt)
        {
            return event_get_arm_64(evt);
        }

        /// <inheritdoc />
        public XDirection EventGetXDirection32(IntPtr evt)
        {
            return event_get_x_direction_32(evt);
        }

        /// <inheritdoc />
        public XDirection EventGetXDirection64(IntPtr evt)
        {
            return event_get_x_direction_64(evt);
        }

        /// <inheritdoc />
        public float EventGetOrientation32(IntPtr evt, OrientationIndex index)
        {
            return event_get_orientation_32(evt, index);
        }

        /// <inheritdoc />
        public float EventGetOrientation64(IntPtr evt, OrientationIndex index)
        {
            return event_get_orientation_64(evt, index);
        }

        /// <inheritdoc />
        public float EventGetAccelerometer32(IntPtr evt, uint index)
        {
            return event_get_accelerometer_32(evt, index);
        }

        /// <inheritdoc />
        public float EventGetAccelerometer64(IntPtr evt, uint index)
        {
            return event_get_accelerometer_64(evt, index);
        }

        /// <inheritdoc />
        public float EventGetGyroscope32(IntPtr evt, uint index)
        {
            return event_get_gyroscope_32(evt, index);
        }

        /// <inheritdoc />
        public float EventGetGyroscope64(IntPtr evt, uint index)
        {
            return event_get_gyroscope_64(evt, index);
        }

        /// <inheritdoc />
        public Pose EventGetPose32(IntPtr evt)
        {
            return event_get_pose_32(evt);
        }

        /// <inheritdoc />
        public Pose EventGetPose64(IntPtr evt)
        {
            return event_get_pose_64(evt);
        }

        /// <inheritdoc />
        public sbyte EventGetRssi32(IntPtr evt)
        {
            return event_get_rssi_32(evt);
        }

        /// <inheritdoc />
        public sbyte EventGetRssi64(IntPtr evt)
        {
            return event_get_rssi_64(evt);
        }

        /// <inheritdoc />
        public void Unlock32(IntPtr myo, UnlockType type, IntPtr error)
        {
            unlock_32(myo, type, error);
        }

        /// <inheritdoc />
        public void Unlock64(IntPtr myo, UnlockType type, IntPtr error)
        {
            unlock_64(myo, type, error);
        }

        /// <inheritdoc />
        public void Lock32(IntPtr myo, IntPtr error)
        {
            lock_32(myo, error);
        }

        /// <inheritdoc />
        public void Lock64(IntPtr myo, IntPtr error)
        {
            lock_64(myo, error);
        }

        /// <inheritdoc />
        public void StreamEmg32(IntPtr myo, StreamEmgType streamEmgType, IntPtr error)
        {
            stream_emg_32(myo, streamEmgType, error);
        }

        /// <inheritdoc />
        public void StreamEmg64(IntPtr myo, StreamEmgType streamEmgType, IntPtr error)
        {
            stream_emg_64(myo, streamEmgType, error);
        }

        /// <inheritdoc />
        public sbyte EventGetEmg32(IntPtr evt, int sensor)
        {
            return event_get_emg_32(evt, sensor);
        }

        /// <inheritdoc />
        public sbyte EventGetEmg64(IntPtr evt, int sensor)
        {
            return event_get_emg_64(evt, sensor);
        }
        #endregion

        #region PInvokes
        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_vibrate", CallingConvention = CallingConvention.Cdecl)]
        private static extern void vibrate_32(IntPtr myo, VibrationType type, IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_vibrate", CallingConvention = CallingConvention.Cdecl)]
        private static extern void vibrate_64(IntPtr myo, VibrationType type, IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_request_rssi", CallingConvention = CallingConvention.Cdecl)]
        private static extern void request_rssi_32(IntPtr myo, IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_request_rssi", CallingConvention = CallingConvention.Cdecl)]
        private static extern void request_rssi_64(IntPtr myo, IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_event_get_firmware_version", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint event_get_firmware_version_32(IntPtr evt, VersionComponent component);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_event_get_firmware_version", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint event_get_firmware_version_64(IntPtr evt, VersionComponent component);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_event_get_arm", CallingConvention = CallingConvention.Cdecl)]
        private static extern Arm event_get_arm_32(IntPtr evt);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_event_get_arm", CallingConvention = CallingConvention.Cdecl)]
        private static extern Arm event_get_arm_64(IntPtr evt);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_event_get_x_direction", CallingConvention = CallingConvention.Cdecl)]
        private static extern XDirection event_get_x_direction_32(IntPtr evt);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_event_get_x_direction", CallingConvention = CallingConvention.Cdecl)]
        private static extern XDirection event_get_x_direction_64(IntPtr evt);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_event_get_orientation", CallingConvention = CallingConvention.Cdecl)]
        private static extern float event_get_orientation_32(IntPtr evt, OrientationIndex index);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_event_get_orientation", CallingConvention = CallingConvention.Cdecl)]
        private static extern float event_get_orientation_64(IntPtr evt, OrientationIndex index);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_event_get_accelerometer", CallingConvention = CallingConvention.Cdecl)]
        private static extern float event_get_accelerometer_32(IntPtr evt, uint index);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_event_get_accelerometer", CallingConvention = CallingConvention.Cdecl)]
        private static extern float event_get_accelerometer_64(IntPtr evt, uint index);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_event_get_gyroscope", CallingConvention = CallingConvention.Cdecl)]
        private static extern float event_get_gyroscope_32(IntPtr evt, uint index);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_event_get_gyroscope", CallingConvention = CallingConvention.Cdecl)]
        private static extern float event_get_gyroscope_64(IntPtr evt, uint index);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_event_get_pose", CallingConvention = CallingConvention.Cdecl)]
        private static extern Pose event_get_pose_32(IntPtr evt);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_event_get_pose", CallingConvention = CallingConvention.Cdecl)]
        private static extern Pose event_get_pose_64(IntPtr evt);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_event_get_rssi", CallingConvention = CallingConvention.Cdecl)]
        private static extern sbyte event_get_rssi_32(IntPtr evt);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_event_get_rssi", CallingConvention = CallingConvention.Cdecl)]
        private static extern sbyte event_get_rssi_64(IntPtr evt);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_myo_unlock", CallingConvention = CallingConvention.Cdecl)]
        private static extern void unlock_32(IntPtr myo, UnlockType type, IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_myo_unlock", CallingConvention = CallingConvention.Cdecl)]
        private static extern void unlock_64(IntPtr myo, UnlockType type, IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_myo_lock", CallingConvention = CallingConvention.Cdecl)]
        private static extern void lock_32(IntPtr myo, IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_myo_lock", CallingConvention = CallingConvention.Cdecl)]
        private static extern void lock_64(IntPtr myo, IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_set_stream_emg", CallingConvention = CallingConvention.Cdecl)]
        private static extern void stream_emg_32(IntPtr myo, StreamEmgType type, IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_set_stream_emg", CallingConvention = CallingConvention.Cdecl)]
        private static extern void stream_emg_64(IntPtr myo, StreamEmgType type, IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_event_get_emg", CallingConvention = CallingConvention.Cdecl)]
        private static extern sbyte event_get_emg_32(IntPtr evt, int sensor);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_event_get_emg", CallingConvention = CallingConvention.Cdecl)]
        private static extern sbyte event_get_emg_64(IntPtr evt, int sensor);
        #endregion  
    }
}
