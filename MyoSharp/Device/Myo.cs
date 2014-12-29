using System;
using System.Diagnostics.Contracts;

using MyoSharp.Math;
using MyoSharp.Poses;
using MyoSharp.Communication;

namespace MyoSharp.Device
{
    /// <summary>
    /// A class that contains functionality for controlling a Myo and 
    /// interacting with the device.
    /// </summary>
    public class Myo : IMyo
    {
        #region Fields
        private readonly IChannelListener _channelListener;
        private readonly IMyoDeviceDriver _myoDeviceDriver;

        private bool _disposed;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Myo"/> class.
        /// </summary>
        /// <param name="channelListener">The channel listener. Cannot be <c>null</c>.</param>
        /// <param name="myoDeviceDriver">The Myo device driver. Cannot be <c>null</c>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The exception that is thrown when <paramref name="channelListener"/> or <paramref name="myoDeviceDriver"/> is <c>null</c>.
        /// </exception>
        protected Myo(IChannelListener channelListener, IMyoDeviceDriver myoDeviceDriver)
        {
            Contract.Requires<ArgumentNullException>(channelListener != null, "channelListener");
            Contract.Requires<ArgumentNullException>(myoDeviceDriver != null, "myoDeviceDriver");

            _channelListener = channelListener;
            _channelListener.EventReceived += Channel_EventReceived;

            _myoDeviceDriver = myoDeviceDriver;

            this.Pose = Pose.Unknown;
            this.Arm = Arm.Unknown;
            this.XDirectionOnArm = XDirection.Unknown;
            this.EmgData = Device.EmgData.Create(new int[0]);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Myo"/> class.
        /// </summary>
        ~Myo()
        {
            Dispose(false);
        }
        #endregion

        #region Properties
        /// <inheritdoc />
        public IntPtr Handle
        {
            get { return _myoDeviceDriver.Handle; }
        }

        /// <inheritdoc />
        public bool IsConnected
        {
            get;
            private set;
        }

        /// <inheritdoc />
        public bool IsUnlocked
        {
            get;
            private set;
        }

        /// <inheritdoc />
        public Arm Arm
        {
            get;
            private set;
        }

        /// <inheritdoc />
        public Pose Pose
        {
            get;
            private set;
        }

        /// <inheritdoc />
        public QuaternionF Orientation
        {
            get;
            private set;
        }

        /// <inheritdoc />
        public Vector3F Accelerometer
        {
            get;
            private set;
        }

        /// <inheritdoc />
        public Vector3F Gyroscope
        {
            get;
            private set;
        }

        /// <inheritdoc />
        public XDirection XDirectionOnArm
        {
            get;
            private set;
        }

        /// <inheritdoc />
        public IEmgData EmgData
        {
            get; private set;
        }
        #endregion

        #region Events
        /// <inheritdoc />
        public event EventHandler<MyoEventArgs> Connected;

        /// <inheritdoc />
        public event EventHandler<MyoEventArgs> Disconnected;

        /// <inheritdoc />
        public event EventHandler<ArmRecognizedEventArgs> ArmRecognized;

        /// <inheritdoc />
        public event EventHandler<MyoEventArgs> ArmLost;

        /// <inheritdoc />
        public event EventHandler<PoseEventArgs> PoseChanged;

        /// <inheritdoc />
        public event EventHandler<OrientationDataEventArgs> OrientationDataAcquired;

        /// <inheritdoc />
        public event EventHandler<AccelerometerDataEventArgs> AccelerometerDataAcquired;

        /// <inheritdoc />
        public event EventHandler<GyroscopeDataEventArgs> GyroscopeDataAcquired;

        /// <inheritdoc />
        public event EventHandler<RssiEventArgs> Rssi;

        /// <inheritdoc />
        public event EventHandler<MyoEventArgs> Locked;

        /// <inheritdoc />
        public event EventHandler<MyoEventArgs> Unlocked;

        /// <inheritdoc />
        public event EventHandler<EmgDataEventArgs> EmgDataAcquired;
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new <see cref="IMyo"/> instance.
        /// </summary>
        /// <param name="channelListener">The channel listener.</param>
        /// <param name="myoDeviceDriver">The myo device driver.</param>
        /// <returns>
        /// A new <see cref="IMyo"/> instance.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">The exception that is thrown when <paramref name="channelListener"/> or <paramref name="myoDeviceDriver"/> is <c>null</c>.</exception>
        public static IMyo Create(IChannelListener channelListener, IMyoDeviceDriver myoDeviceDriver)
        {
            Contract.Requires<ArgumentNullException>(channelListener != null, "channelListener");
            Contract.Requires<ArgumentNullException>(myoDeviceDriver != null, "myoDeviceDriver");
            Contract.Ensures(Contract.Result<IMyo>() != null);

            return new Myo(channelListener, myoDeviceDriver);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public void Vibrate(VibrationType type)
        {
            _myoDeviceDriver.Vibrate(type);
        }

        /// <inheritdoc />
        public void RequestRssi()
        {
            _myoDeviceDriver.RequestRssi();
        }

        /// <inheritdoc />
        public void Unlock(UnlockType type)
        {
            _myoDeviceDriver.Unlock(type);
        }

        /// <inheritdoc />
        public void Lock()
        {
            _myoDeviceDriver.Lock();
        }

        /// <inheritdoc />
        public void SetEmgStreaming(bool enabled)
        {
            _myoDeviceDriver.SetEmgStreaming(enabled);
        }

        /// <summary>
        /// Handles an event that was received for this device.
        /// </summary>
        /// <param name="type">The type of the event.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        /// <param name="evt">The pointer to the event.</param>
        protected virtual void HandleEvent(MyoEventType type, DateTime timestamp, IntPtr evt)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The event handle must be set.");

            switch (type)
            {
                case MyoEventType.Connected:
                    OnConnected(timestamp);
                    break;

                case MyoEventType.Disconnected:
                    OnDisconnected(timestamp);
                    break;

                case MyoEventType.ArmRecognized:
                    OnArmRecognized(evt, timestamp);
                    break;

                case MyoEventType.ArmLost:
                    OnArmLost(timestamp);
                    break;

                case MyoEventType.Orientation:
                    OnOrientationChanged(evt, timestamp);
                    break;

                case MyoEventType.Pose:
                    OnPoseChanged(evt, timestamp);
                    break;

                case MyoEventType.Rssi:
                    OnRssi(evt, timestamp);
                    break;

                case MyoEventType.Locked:
                    OnLock(timestamp);
                    break;

                case MyoEventType.Unlocked:
                    OnUnlock(timestamp);
                    break;

                case MyoEventType.Emg:
                    OnEmgData(evt, timestamp);
                    break;
            }
        }

        /// <summary>
        /// Called when the Myo sends a <see cref="MyoEventType.Emg"/> event.
        /// </summary>
        /// <param name="evt">The pointer to the event.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        protected virtual void OnEmgData(IntPtr evt, DateTime timestamp)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The event handle must be set.");

            const int NUMBER_OF_SENSORS = 8;
            var rawEmgData = new int[NUMBER_OF_SENSORS];
            for (int i = 0; i < rawEmgData.Length; i++)
            {
                rawEmgData[i] = _myoDeviceDriver.GetEventEmg(evt, i);
            }

            var emgData = Device.EmgData.Create(rawEmgData);
            this.EmgData = emgData;

            var handler = EmgDataAcquired;
            if (handler != null)
            {
                var args = new EmgDataEventArgs(
                    this,
                    timestamp,
                    emgData);
                handler.Invoke(this, args);
            }
        }

        /// <summary>
        /// Called when the Myo sends a <see cref="MyoEventType.Rssi"/> event.
        /// </summary>
        /// <param name="evt">The pointer to the event.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        protected virtual void OnRssi(IntPtr evt, DateTime timestamp)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The event handle must be set.");

            var handler = Rssi;
            if (handler != null)
            {
                var rssi = _myoDeviceDriver.GetEventRssi(evt);
                var args = new RssiEventArgs(
                    this, 
                    timestamp, 
                    rssi);
                handler.Invoke(this, args);
            }
        }

        /// <summary>
        /// Called when the Myo has detected a change in the user's pose.
        /// </summary>
        /// <param name="evt">The pointer to the event.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        protected virtual void OnPoseChanged(IntPtr evt, DateTime timestamp)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The event handle must be set.");

            var pose = _myoDeviceDriver.GetEventPose(evt);
            this.Pose = pose;

            var handler = PoseChanged;
            if (handler != null)
            {
                var args = new PoseEventArgs(this, timestamp, pose);
                handler.Invoke(this, args);
            }
        }

        /// <summary>
        /// Called when the Myo has detected an orientation change.
        /// </summary>
        /// <param name="evt">The pointer to the event.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        protected virtual void OnOrientationChanged(IntPtr evt, DateTime timestamp)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The event handle must be set.");

            OnAcquiredOrientationData(evt, timestamp);
            OnAcquiredAccelerometerData(evt, timestamp);
            OnAcquiredGyroscopeData(evt, timestamp);
        }

        /// <summary>
        /// Called when gyroscope data has been acquired from the Myo.
        /// </summary>
        /// <param name="evt">The pointer to the event.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        protected virtual void OnAcquiredGyroscopeData(IntPtr evt, DateTime timestamp)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The event handle must be set.");

            var gyroscope = _myoDeviceDriver.GetGyroscope(evt);
            this.Gyroscope = gyroscope;

            var handler = GyroscopeDataAcquired;
            if (handler != null)
            {
                var args = new GyroscopeDataEventArgs(
                    this,
                    timestamp,
                    gyroscope);
                handler.Invoke(this, args);
            }
        }

        /// <summary>
        /// Called when accelerometer data has been acquired from the Myo.
        /// </summary>
        /// <param name="evt">The pointer to the event.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        protected virtual void OnAcquiredAccelerometerData(IntPtr evt, DateTime timestamp)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The event handle must be set.");

            var accelerometer = _myoDeviceDriver.GetEventAccelerometer(evt);
            this.Accelerometer = accelerometer;

            var handler = AccelerometerDataAcquired;
            if (handler != null)
            {
                var args = new AccelerometerDataEventArgs(
                    this, 
                    timestamp,
                    accelerometer);
                handler.Invoke(this, args);
            }
        }

        /// <summary>
        /// Called when orientation data has been acquired from the Myo.
        /// </summary>
        /// <param name="evt">The pointer to the event.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        protected virtual void OnAcquiredOrientationData(IntPtr evt, DateTime timestamp)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The event handle must be set.");

            var orientation = _myoDeviceDriver.GetEventOrientation(evt);
            this.Orientation = orientation;

            var handler = OrientationDataAcquired;
            if (handler != null)
            {
                
                var args = new OrientationDataEventArgs(
                    this,
                    timestamp,
                    orientation,
                    CalculateRoll(orientation),
                    CalculatePitch(orientation),
                    CalculateYaw(orientation)
                );
                
                handler.Invoke(this, args);
            }
        }

        /// <summary>
        /// Called when the Myo can no longer recognize which arm it is on.
        /// </summary>
        /// <param name="timestamp">The timestamp of the event.</param>
        protected virtual void OnArmLost(DateTime timestamp)
        {
            this.Arm = Arm.Unknown;
            this.XDirectionOnArm = XDirection.Unknown;

            var handler = ArmLost;
            if (handler != null)
            {
                var args = new MyoEventArgs(this, timestamp);
                handler.Invoke(this, args);
            }
        }

        /// <summary>
        /// Called when the Myo has recognized which arm it is on.
        /// </summary>
        /// <param name="evt">The pointer to the event.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        protected virtual void OnArmRecognized(IntPtr evt, DateTime timestamp)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The event handle must be set.");

            var arm = _myoDeviceDriver.GetArm(evt);
            this.Arm = arm;

            var xDirection = _myoDeviceDriver.GetEventDirectionX(evt);
            this.XDirectionOnArm = xDirection;

            var handler = ArmRecognized;
            if (handler != null)
            {
                var args = new ArmRecognizedEventArgs(
                    this,
                    timestamp,
                    arm,
                    xDirection);
                handler.Invoke(this, args);
            }
        }

        /// <summary>
        /// Called when the Myo has disconnected.
        /// </summary>
        /// <param name="timestamp">The timestamp of the event.</param>
        protected virtual void OnDisconnected(DateTime timestamp)
        {
            this.IsConnected = false;
            
            var handler = Disconnected;
            if (handler != null)
            {
                var args = new MyoEventArgs(this, timestamp);
                handler.Invoke(this, args);
            }
        }

        /// <summary>
        /// Called when the Myo has connected..
        /// </summary>
        /// <param name="timestamp">The timestamp of the event.</param>
        protected virtual void OnConnected(DateTime timestamp)
        {
            this.IsConnected = true;

            var handler = Connected;
            if (handler != null)
            {
                var args = new MyoEventArgs(this, timestamp);
                handler.Invoke(this, args);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            try
            {
                // free managed objects
                if (disposing)
                {
                    _channelListener.EventReceived -= Channel_EventReceived;
                }
            }
            finally
            {
                _disposed = true;
            }
        }

        /// <summary>
        /// Called when the Myo has locked.
        /// </summary>
        /// <param name="timestamp">The timestamp of the event.</param>
        protected virtual void OnLock(DateTime timestamp)
        {
            this.IsUnlocked = false;

            var handler = Locked;
            if (handler != null)
            {
                var args = new MyoEventArgs(this, timestamp);
                handler.Invoke(this, args);
            }
        }
        
        /// <summary>
        /// Called when the Myo has unlocked.
        /// </summary>
        /// <param name="timestamp">The timestamp of the event.</param>
        protected virtual void OnUnlock(DateTime timestamp)
        {
            this.IsUnlocked = true;

            var handler = Unlocked;
            if (handler != null)
            {
                var args = new MyoEventArgs(this, timestamp);
                handler.Invoke(this, args);
            }
        }

        protected static double CalculateRoll(QuaternionF orientation)
        {
            Contract.Requires<ArgumentNullException>(orientation != null, "orientation");

            return System.Math.Atan2(2.0f * (orientation.W * orientation.X + orientation.Y * orientation.Z), 1.0f - 2.0f * (orientation.X * orientation.X + orientation.Y * orientation.Y));
        }

        protected static double CalculatePitch(QuaternionF orientation)
        {
            Contract.Requires<ArgumentNullException>(orientation != null, "orientation");

            return System.Math.Asin(2.0f * (orientation.W * orientation.Y - orientation.Z * orientation.X));
        }

        protected static double CalculateYaw(QuaternionF orientation)
        {
            Contract.Requires<ArgumentNullException>(orientation != null, "orientation");

            return System.Math.Atan2(2.0f * (orientation.W * orientation.Z + orientation.X * orientation.Y), 1.0f - 2.0f * (orientation.Y * orientation.Y + orientation.Z * orientation.Z));
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_channelListener != null);
            Contract.Invariant(_myoDeviceDriver != null);
        }
        #endregion

        #region Event handlers
        private void Channel_EventReceived(object sender, RouteMyoEventArgs e)
        {
            // check if this event is for us
            if (e.MyoHandle != this.Handle)
            {
                return;
            }

            HandleEvent(e.EventType, e.Timestamp, e.Event);
        }
        #endregion
    }
}
