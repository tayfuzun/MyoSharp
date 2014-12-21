using System;
using System.Collections.Generic;

using MyoSharp.Communication;
using MyoSharp.Discovery;

namespace MyoSharp.Device
{
    /// <summary>
    /// A class that manages a collection of Myos.
    /// </summary>
    public class Hub : IHub
    {
        #region Fields
        private readonly Dictionary<IntPtr, IMyo> _myos;
        private readonly IDeviceListener _deviceListener;
        private readonly ReadOnlyMyoCollection _readonlyMyos;
        private bool _disposed;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Hub"/> class and 
        /// immediately starts listening for Myo activity.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when there is a failure to connect to the Bluetooth hub.
        /// </exception>
        protected Hub()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hub"/> class and 
        /// immediately starts listening for Myo activity.
        /// </summary>
        /// <param name="applicationIdentifier">The application identifier.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the specified application identifier is invalid.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when there is a failure to connect to the Bluetooth hub.
        /// </exception>
        protected Hub(string applicationIdentifier)
            : this(Channel.Create(applicationIdentifier, true))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hub"/> class.
        /// </summary>
        /// <param name="channelListener">The channel listener.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the channel listener is null.</exception>
        protected Hub(IChannelListener channelListener)
            : this(DeviceListener.Create(channelListener))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hub" /> class.
        /// </summary>
        /// <param name="deviceListener">The device listener.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the device listener is null.</exception>
        protected Hub(IDeviceListener deviceListener)
        {
            if (deviceListener == null)
            {
                throw new ArgumentNullException("deviceListener", "The device listener cannot be null.");
            }

            _myos = new Dictionary<IntPtr, IMyo>();
            _readonlyMyos = new ReadOnlyMyoCollection(_myos);

            _deviceListener = deviceListener;
            _deviceListener.Paired += DeviceListener_Paired;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Hub"/> class.
        /// </summary>
        ~Hub()
        {
            Dispose(false);
        }
        #endregion

        #region Events
        /// <summary>
        /// The event that is triggered when a Myo has connected.
        /// </summary>
        public event EventHandler<MyoEventArgs> MyoConnected;

        /// <summary>
        /// The event that is triggered when a Myo has disconnected.
        /// </summary>
        public event EventHandler<MyoEventArgs> MyoDisconnected;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the collection of Myos being managed by this hub.
        /// </summary>
        public IReadOnlyMyoCollection Myos
        {
            get { return _readonlyMyos; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new <see cref="IHub" /> instance.
        /// </summary>
        /// <param name="applicationIdentifier">The application identifier.</param>
        /// <returns>
        /// A new <see cref="IHub" /> instance.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the specified application identifier is invalid.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when there is a failure to connect to the Bluetooth hub.
        /// </exception>
        public static IHub Create(string applicationIdentifier = "")
        {
            return new Hub(Channel.Create(applicationIdentifier, true));
        }

        /// <summary>
        /// Creates a new <see cref="IHub"/> instance.
        /// </summary>
        /// <param name="channelListener">The channel listener.</param>
        /// <returns>A new <see cref="IHub"/> instance.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the channel listener is null.</exception>
        public static IHub Create(IChannelListener channelListener)
        {
            return new Hub(DeviceListener.Create(channelListener));
        }

        /// <summary>
        /// Creates a new <see cref="IHub"/> instance.
        /// </summary>
        /// <param name="deviceListener">The device listener.</param>
        /// <returns>A new <see cref="IHub"/> instance.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the device listener is null.</exception>
        public static IHub Create(IDeviceListener deviceListener)
        {
            return new Hub(deviceListener);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Creates a new <see cref="IMyo" /> instance.
        /// </summary>
        /// <param name="channelListener">The channel listener.</param>
        /// <param name="myoDeviceDriver">The Myo device driver.</param>
        /// <returns>
        /// Returns a new <see cref="IMyo" /> instance.
        /// </returns>
        protected virtual IMyo CreateMyo(IChannelListener channelListener, IMyoDeviceDriver myoDeviceDriver)
        {
            return Myo.Create(channelListener, myoDeviceDriver);
        }

        /// <summary>
        /// Creates a new <see cref="IMyoDeviceDriver"/>.
        /// </summary>
        /// <param name="myoHandle">The Myo handle.</param>
        /// <param name="myoDeviceBridge">The Myo device bridge.</param>
        /// <returns>Returns a new <see cref="IMyoDeviceDriver"/> instance.</returns>
        protected virtual IMyoDeviceDriver CreateMyoDeviceDriver(IntPtr myoHandle, IMyoDeviceBridge myoDeviceBridge)
        {
            return MyoDeviceDriver.Create(myoHandle, myoDeviceBridge);
        }

        /// <summary>
        /// Creates a new <see cref="IMyoDeviceBridge"/>.
        /// </summary>
        /// <returns>Returns a new <see cref="IMyoDeviceBridge"/> instance.</returns>
        protected virtual IMyoDeviceBridge CreateMyoDeviceBridge()
        {
            return MyoDeviceBridge.Create();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; 
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            try
            {
                if (disposing)
                {
                    foreach (var myo in _myos.Values)
                    {
                        UnhookMyoEvents(myo);
                        myo.Dispose();
                    }

                    _myos.Clear();

                    _deviceListener.Paired -= DeviceListener_Paired;
                }
            }
            finally
            {
                _disposed = true;
            }
        }

        /// <summary>
        /// Hooks the Myo events.
        /// </summary>
        /// <param name="myo">The Myo to hook onto.</param>
        protected virtual void HookMyoEvents(IMyoEventGenerator myo)
        {
            myo.Connected += Myo_Connected;
            myo.Disconnected += Myo_Disconnected;
        }

        /// <summary>
        /// Unhooks the Myo events.
        /// </summary>
        /// <param name="myo">The myo to hook onto.</param>
        protected virtual void UnhookMyoEvents(IMyoEventGenerator myo)
        {
            myo.Connected -= Myo_Connected;
            myo.Disconnected -= Myo_Disconnected;
        }

        /// <summary>
        /// Raises the <see cref="E:MyoConnected" /> event.
        /// </summary>
        /// <param name="e">The <see cref="MyoEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMyoConnected(MyoEventArgs e)
        {
            var handler = MyoConnected;
            if (handler != null)
            {
                handler.Invoke(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:MyoDisconnected" /> event.
        /// </summary>
        /// <param name="e">The <see cref="MyoEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMyoDisconnected(MyoEventArgs e)
        {
            var handler = MyoDisconnected;
            if (handler != null)
            {
                handler.Invoke(this, e);
            }
        }
        #endregion

        #region Event Handlers
        private void DeviceListener_Paired(object sender, PairedEventArgs e)
        {
            var myoDeviceBridge = CreateMyoDeviceBridge();

            var myoDeviceDriver = CreateMyoDeviceDriver(
                e.MyoHandle,
                myoDeviceBridge);

            var myo = CreateMyo(
                ((IDeviceListener)sender).ChannelListener,
                myoDeviceDriver);

            HookMyoEvents(myo);
        }

        private void Myo_Disconnected(object sender, MyoEventArgs e)
        {
            OnMyoDisconnected(e);
            UnhookMyoEvents(e.Myo);
            _myos.Remove(e.Myo.Handle);
            e.Myo.Dispose();
        }

        private void Myo_Connected(object sender, MyoEventArgs e)
        {
            _myos[e.Myo.Handle] = e.Myo;
            OnMyoConnected(e);
        }
        #endregion

        #region Classes
        private class ReadOnlyMyoCollection : IReadOnlyMyoCollection
        {
            #region Fields
            private readonly Dictionary<IntPtr, IMyo> _myos;
            #endregion

            #region Constructors
            internal ReadOnlyMyoCollection(Dictionary<IntPtr, IMyo> myos)
            {
                _myos = myos;
            }
            #endregion

            #region Properties
            public int Count
            {
                get { return _myos.Count; }
            }
            #endregion

            #region Methods
            public IEnumerator<IMyo> GetEnumerator()
            {
                return _myos.Values.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return _myos.Values.GetEnumerator();
            }
            #endregion
        }
        #endregion
    }
}
