using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

using MyoSharp.Discovery;
using MyoSharp.Internal;
using MyoSharp.Math;

namespace MyoSharp.Device
{
    public class Myo
    {
        #region Fields
        private readonly IHub _hub;
        private IntPtr _handle;
        #endregion

        #region Constructors
        internal Myo(IHub hub, IntPtr handle)
        {
            Debug.Assert(handle != IntPtr.Zero, "Cannot construct Myo instance with null pointer.");

            _hub = hub;
            _handle = handle;
        }
        #endregion

        #region Properties
        internal IHub Hub
        {
            get { return _hub; }
        }

        internal IntPtr Handle
        {
            get { return _handle; }
        }
        #endregion

        #region Events
        public event EventHandler<MyoEventArgs> Connected;

        public event EventHandler<MyoEventArgs> Disconnected;

        public event EventHandler<ArmRecognizedEventArgs> ArmRecognized;

        public event EventHandler<MyoEventArgs> ArmLost;

        public event EventHandler<PoseEventArgs> PoseChange;

        public event EventHandler<OrientationDataEventArgs> OrientationData;

        public event EventHandler<AccelerometerDataEventArgs> AccelerometerData;

        public event EventHandler<GyroscopeDataEventArgs> GyroscopeData;

        public event EventHandler<RssiEventArgs> Rssi;
        #endregion

        #region Methods
        public void Vibrate(VibrationType type)
        {
            if (PlatformInvocation.Running32Bit)
            {
                vibrate_32(_handle, type, IntPtr.Zero);
            }
            else
            {
                vibrate_64(_handle, type, IntPtr.Zero);
            }
        }

        public void RequestRssi()
        {
            if (PlatformInvocation.Running32Bit)
            {
                request_rssi_32(_handle, IntPtr.Zero);
            }
            else
            {
                request_rssi_64(_handle, IntPtr.Zero);
            }
        }

        internal void HandleEvent(MyoEventType type, DateTime timestamp, IntPtr evt)
        {
            switch (type)
            {
                case MyoEventType.Connected:
                    {
                        var handler = Connected;
                        if (handler != null)
                        {
                            var args = new MyoEventArgs(this, timestamp);
                            handler(this, args);
                        }
                    }

                    break;

                case MyoEventType.Disconnected:
                    {
                        var handler = Disconnected;
                        if (handler != null)
                        {
                            var args = new MyoEventArgs(this, timestamp);
                            handler(this, args);
                        }
                    }

                    break;

                case MyoEventType.ArmRecognized:
                    {
                        var handler = ArmRecognized;
                        if (handler != null)
                        {
                            var arm = GetArm(evt);
                            var xDirection = GetEventDirectionX(evt);
                            var args = new ArmRecognizedEventArgs(
                                this,
                                timestamp,
                                arm,
                                xDirection);
                            handler.Invoke(this, args);
                        }
                    }

                    break;

                case MyoEventType.ArmLost:
                    if (ArmLost != null)
                    {
                        ArmLost(this, new MyoEventArgs(this, timestamp));
                    }
                    break;

                case MyoEventType.Orientation:
                    if (AccelerometerData != null)
                    {
                        float x = GetEventAccelerometer(evt, 0);
                        float y = GetEventAccelerometer(evt, 1);
                        float z = GetEventAccelerometer(evt, 2);

                        var accelerometer = new Vector3F(x, y, z);
                        AccelerometerData(this, new AccelerometerDataEventArgs(this, timestamp, accelerometer));
                    }
                    if (GyroscopeData != null)
                    {
                        float x = GetGyroscope(evt, 0);
                        float y = GetGyroscope(evt, 1);
                        float z = GetGyroscope(evt, 2);

                        var gyroscope = new Vector3F(x, y, z);
                        GyroscopeData(this, new GyroscopeDataEventArgs(this, timestamp, gyroscope));
                    }
                    if (OrientationData != null)
                    {
                        float x = GetEventOrientation(evt, OrientationIndex.X);
                        float y = GetEventOrientation(evt, OrientationIndex.Y);
                        float z = GetEventOrientation(evt, OrientationIndex.Z);
                        float w = GetEventOrientation(evt, OrientationIndex.W);

                        var orientation = new QuaternionF(x, y, z, w);
                        OrientationData(this, new OrientationDataEventArgs(this, timestamp, orientation));
                    }
                    break;

                case MyoEventType.Pose:
                    if (PoseChange != null)
                    {
                        var pose = GetEventPose(evt);
                        var args = new PoseEventArgs(this, timestamp, pose);
                        PoseChange(this, args);
                    }
                    break;

                case MyoEventType.Rssi:
                    if (Rssi != null)
                    {
                        var rssi = GetEventRssi(evt);
                        Rssi(this, new RssiEventArgs(this, timestamp, rssi));
                    }
                    break;
            }
        }

        private static sbyte GetEventRssi(IntPtr evt)
        {
            return PlatformInvocation.Running32Bit
                ? GetEventRssi(evt)
                : GetEventRssi(evt);
        }
        
        private static float GetEventAccelerometer(IntPtr evt, uint index)
        {
            return PlatformInvocation.Running32Bit
                ? event_get_accelerometer_32(evt, index)
                : event_get_accelerometer_64(evt, index);
        }

        private static XDirection GetEventDirectionX(IntPtr evt)
        {
            return PlatformInvocation.Running32Bit
                ? event_get_x_direction_32(evt)
                : event_get_x_direction_64(evt);
        }

        private static float GetEventOrientation(IntPtr evt, OrientationIndex index)
        {
            return PlatformInvocation.Running32Bit
                ? event_get_orientation_32(evt, index)
                : event_get_orientation_64(evt, index);
        }
        
        private static float GetFirmwareVersion(IntPtr evt, VersionComponent component)
        {
            return PlatformInvocation.Running32Bit
                ? event_get_firmware_version_32(evt, component)
                : event_get_firmware_version_64(evt, component);
        }

        private static Arm GetArm(IntPtr evt)
        {
            return PlatformInvocation.Running32Bit
                ? event_get_arm_32(evt)
                : event_get_arm_64(evt);
        }

        private static float GetGyroscope(IntPtr evt, uint index)
        {
            return PlatformInvocation.Running32Bit
                ? event_get_gyroscope_32(evt, index)
                : event_get_gyroscope_64(evt, index);
        }

        private static Pose GetEventPose(IntPtr evt)
        {
            return PlatformInvocation.Running32Bit
                ? event_get_pose_32(evt)
                : event_get_pose_64(evt);
        }
        #endregion

        #region PInvokes
        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_vibrate", CallingConvention = CallingConvention.Cdecl)]
        public static extern void vibrate_32(IntPtr myo, VibrationType type, IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_vibrate", CallingConvention = CallingConvention.Cdecl)]
        public static extern void vibrate_64(IntPtr myo, VibrationType type, IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_request_rssi", CallingConvention = CallingConvention.Cdecl)]
        public static extern void request_rssi_32(IntPtr myo, IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_request_rssi", CallingConvention = CallingConvention.Cdecl)]
        public static extern void request_rssi_64(IntPtr myo, IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_event_get_firmware_version", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint event_get_firmware_version_32(IntPtr evt, VersionComponent component);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_event_get_firmware_version", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint event_get_firmware_version_64(IntPtr evt, VersionComponent component);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_event_get_arm", CallingConvention = CallingConvention.Cdecl)]
        public static extern Arm event_get_arm_32(IntPtr evt);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_event_get_arm", CallingConvention = CallingConvention.Cdecl)]
        public static extern Arm event_get_arm_64(IntPtr evt);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_event_get_x_direction", CallingConvention = CallingConvention.Cdecl)]
        public static extern XDirection event_get_x_direction_32(IntPtr evt);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_event_get_x_direction", CallingConvention = CallingConvention.Cdecl)]
        public static extern XDirection event_get_x_direction_64(IntPtr evt);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_event_get_orientation", CallingConvention = CallingConvention.Cdecl)]
        public static extern float event_get_orientation_32(IntPtr evt, OrientationIndex index);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_event_get_orientation", CallingConvention = CallingConvention.Cdecl)]
        public static extern float event_get_orientation_64(IntPtr evt, OrientationIndex index);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_event_get_accelerometer", CallingConvention = CallingConvention.Cdecl)]
        public static extern float event_get_accelerometer_32(IntPtr evt, uint index);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_event_get_accelerometer", CallingConvention = CallingConvention.Cdecl)]
        public static extern float event_get_accelerometer_64(IntPtr evt, uint index);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_event_get_gyroscope", CallingConvention = CallingConvention.Cdecl)]
        public static extern float event_get_gyroscope_32(IntPtr evt, uint index);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_event_get_gyroscope", CallingConvention = CallingConvention.Cdecl)]
        public static extern float event_get_gyroscope_64(IntPtr evt, uint index);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_event_get_pose", CallingConvention = CallingConvention.Cdecl)]
        public static extern Pose event_get_pose_32(IntPtr evt);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_event_get_pose", CallingConvention = CallingConvention.Cdecl)]
        public static extern Pose event_get_pose_64(IntPtr evt);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_event_get_rssi", CallingConvention = CallingConvention.Cdecl)]
        public static extern sbyte event_get_rssi_32(IntPtr evt);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_event_get_rssi", CallingConvention = CallingConvention.Cdecl)]
        public static extern sbyte event_get_rssi_64(IntPtr evt);
        #endregion  
    }
}
