using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

using MyoSharp.Internal;
using MyoSharp.Math;
using MyoSharp.Poses;

namespace MyoSharp.Device
{
    public sealed class MyoDeviceDriver : IMyoDeviceDriver
    {
        #region Fields
        private readonly IntPtr _handle;
        private readonly IMyoDeviceBridge _myoDeviceBridge;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MyoDeviceDriver"/> class.
        /// </summary>
        /// <param name="handle">The handle to the Myo.</param>
        /// <param name="myoDeviceBridge">An instance of <see cref="IMyoDeviceBridge"/> for communicating with the device. Cannot be null.</param>
        /// <exception cref="System.ArgumentException">The exception that is thrown when the handle is not set.</exception>
        /// <exception cref="System.ArgumentNullException">The exception that is thrown when the device bridge is <c>null</c>.</exception>
        private MyoDeviceDriver(IntPtr handle, IMyoDeviceBridge myoDeviceBridge)
        {
            Contract.Requires<ArgumentException>(handle != IntPtr.Zero, "The handle must be set.");
            Contract.Requires<ArgumentNullException>(myoDeviceBridge != null, "myoDeviceBridge");

            _handle = handle;
            _myoDeviceBridge = myoDeviceBridge;
        }
        #endregion

        #region Properties
        /// <inheritdoc />
        public IntPtr Handle
        {
            get { return _handle; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new <see cref="IMyoDeviceDriver"/> instance.
        /// </summary>
        /// <param name="handle">The handle to the Myo.</param>
        /// <param name="myoDeviceBridge">An instance of <see cref="IMyoDeviceBridge"/> for communicating with the device. Cannot be null.</param>
        /// <exception cref="System.ArgumentException">The exception that is thrown when the handle is not set.</exception>
        /// <exception cref="System.ArgumentNullException">The exception that is thrown when the device bridge is <c>null</c>.</exception>
        public static IMyoDeviceDriver Create(IntPtr handle, IMyoDeviceBridge myoDeviceBridge)
        {
            Contract.Requires<ArgumentException>(handle != IntPtr.Zero, "The handle must be set.");
            Contract.Requires<ArgumentNullException>(myoDeviceBridge != null, "myoDeviceBridge");
            Contract.Ensures(Contract.Result<IMyoDeviceDriver>() != null);

            return new MyoDeviceDriver(handle, myoDeviceBridge);
        }

        /// <inheritdoc />
        public void Vibrate(VibrationType type)
        {
            if (PlatformInvocation.Running32Bit)
            {
                _myoDeviceBridge.Vibrate32(_handle, type, IntPtr.Zero);
            }
            else
            {
                _myoDeviceBridge.Vibrate64(_handle, type, IntPtr.Zero);
            }
        }

        /// <inheritdoc />
        public void RequestRssi()
        {
            if (PlatformInvocation.Running32Bit)
            {
                _myoDeviceBridge.RequestRssi32(_handle, IntPtr.Zero);
            }
            else
            {
                _myoDeviceBridge.RequestRssi64(_handle, IntPtr.Zero);
            }
        }

        /// <inheritdoc />
        public void Lock()
        {
            if (PlatformInvocation.Running32Bit)
            {
                _myoDeviceBridge.Lock32(_handle, IntPtr.Zero);
            }
            else
            {
                _myoDeviceBridge.Lock64(_handle, IntPtr.Zero);
            }
        }

        /// <inheritdoc />
        public void Unlock(UnlockType type)
        {
            if (PlatformInvocation.Running32Bit)
            {
                _myoDeviceBridge.Unlock32(_handle, type, IntPtr.Zero);
            }
            else
            {
                _myoDeviceBridge.Unlock64(_handle, type, IntPtr.Zero);
            }
        }

        /// <inheritdoc />
        public sbyte GetEventRssi(IntPtr evt)
        {
            return PlatformInvocation.Running32Bit
                ? _myoDeviceBridge.EventGetRssi32(evt)
                : _myoDeviceBridge.EventGetRssi64(evt);
        }

        /// <inheritdoc />
        public Vector3F GetEventAccelerometer(IntPtr evt)
        {
            var x = GetEventAccelerometer(evt, 0);
            var y = GetEventAccelerometer(evt, 1);
            var z = GetEventAccelerometer(evt, 2);

            return new Vector3F(x, y, z);
        }

        /// <inheritdoc />
        public float GetEventAccelerometer(IntPtr evt, uint index)
        {
            return PlatformInvocation.Running32Bit
                ? _myoDeviceBridge.EventGetAccelerometer32(evt, index)
                : _myoDeviceBridge.EventGetAccelerometer64(evt, index);
        }

        /// <inheritdoc />
        public XDirection GetEventDirectionX(IntPtr evt)
        {
            return PlatformInvocation.Running32Bit
                ? _myoDeviceBridge.EventGetXDirection32(evt)
                : _myoDeviceBridge.EventGetXDirection64(evt);
        }

        /// <inheritdoc />
        public QuaternionF GetEventOrientation(IntPtr evt)
        {
            var x = GetEventOrientation(evt, OrientationIndex.X);
            var y = GetEventOrientation(evt, OrientationIndex.Y);
            var z = GetEventOrientation(evt, OrientationIndex.Z);
            var w = GetEventOrientation(evt, OrientationIndex.W);

            return new QuaternionF(x, y, z, w);
        }

        /// <inheritdoc />
        public float GetEventOrientation(IntPtr evt, OrientationIndex index)
        {
            return PlatformInvocation.Running32Bit
                ? _myoDeviceBridge.EventGetOrientation32(evt, index)
                : _myoDeviceBridge.EventGetOrientation64(evt, index);
        }

        /// <inheritdoc />
        public float GetFirmwareVersion(IntPtr evt, VersionComponent component)
        {
            return PlatformInvocation.Running32Bit
                ? _myoDeviceBridge.EventGetFirmwareVersion32(evt, component)
                : _myoDeviceBridge.EventGetFirmwareVersion64(evt, component);
        }

        /// <inheritdoc />
        public Arm GetArm(IntPtr evt)
        {
            return PlatformInvocation.Running32Bit
                ? _myoDeviceBridge.EventGetArm32(evt)
                : _myoDeviceBridge.EventGetArm64(evt);
        }

        /// <inheritdoc />
        public Vector3F GetGyroscope(IntPtr evt)
        {
            var x = GetGyroscope(evt, 0);
            var y = GetGyroscope(evt, 1);
            var z = GetGyroscope(evt, 2);

            return new Vector3F(x, y, z);
        }

        /// <inheritdoc />
        public float GetGyroscope(IntPtr evt, uint index)
        {
            return PlatformInvocation.Running32Bit
                ? _myoDeviceBridge.EventGetGyroscope32(evt, index)
                : _myoDeviceBridge.EventGetGyroscope64(evt, index);
        }

        /// <inheritdoc />
        public Pose GetEventPose(IntPtr evt)
        {
            return PlatformInvocation.Running32Bit
                ? _myoDeviceBridge.EventGetPose32(evt)
                : _myoDeviceBridge.EventGetPose64(evt);
        }

        /// <inheritdoc />
        public void SetEmgStreaming(bool enabled)
        {
            var streamEmgType = enabled
                ? StreamEmgType.Enabled
                : StreamEmgType.Disabled;

            if (PlatformInvocation.Running32Bit)
            {
                _myoDeviceBridge.StreamEmg32(_handle, streamEmgType, IntPtr.Zero);
            }
            else
            {
                _myoDeviceBridge.StreamEmg64(_handle, streamEmgType, IntPtr.Zero);
            }
        }

        /// <inheritdoc />
        public sbyte GetEventEmg(IntPtr evt, int sensor)
        {
            return PlatformInvocation.Running32Bit
                ? _myoDeviceBridge.EventGetEmg32(evt, sensor)
                : _myoDeviceBridge.EventGetEmg64(evt, sensor);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_handle != IntPtr.Zero);
            Contract.Invariant(_myoDeviceBridge != null);
        }
        #endregion
    }
}
