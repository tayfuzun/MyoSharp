using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using MyoSharp.Communication;
using MyoSharp.Discovery;
using MyoSharp.Exceptions;

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
        /// Initializes a new instance of the <see cref="Hub"/> class.
        /// </summary>
        /// <param name="channelListener">The channel listener.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the channel listener is null.</exception>
        protected Hub(IChannelListener channelListener)
            : this(DeviceListener.Create(channelListener))
        {
            Contract.Requires<ArgumentNullException>(channelListener != null, "channelListener");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hub" /> class.
        /// </summary>
        /// <param name="deviceListener">The device listener.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the device listener is null.</exception>
        protected Hub(IDeviceListener deviceListener)
        {
            Contract.Requires<ArgumentNullException>(deviceListener != null, "deviceListener");

            _myos = new Dictionary<IntPtr, IMyo>();
            _readonlyMyos = new ReadOnlyMyoCollection(_myos);

            _deviceListener = deviceListener;
            _deviceListener.Paired += DeviceListener_Paired;
            _deviceListener.Unpaired += DeviceListener_Unpaired;
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
        /// Creates a new <see cref="IHub"/> instance.
        /// </summary>
        /// <param name="channelListener">The channel listener.</param>
        /// <returns>A new <see cref="IHub"/> instance.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the channel listener is null.</exception>
        public static IHub Create(IChannelListener channelListener)
        {
            Contract.Requires<ArgumentNullException>(channelListener != null, "channelListener");
            Contract.Ensures(Contract.Result<IHub>() != null);

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
            Contract.Requires<ArgumentNullException>(deviceListener != null, "deviceListener");
            Contract.Ensures(Contract.Result<IHub>() != null);

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
            Contract.Requires<ArgumentNullException>(channelListener != null, "channelListener");
            Contract.Requires<ArgumentNullException>(myoDeviceDriver != null, "myoDeviceDriver");
            Contract.Ensures(Contract.Result<IMyo>() != null);

            return Myo.Create(channelListener, myoDeviceDriver);
        }

        /// <summary>
        /// Creates a new <see cref="IMyoDeviceDriver" />.
        /// </summary>
        /// <param name="myoHandle">The Myo handle.</param>
        /// <param name="myoDeviceBridge">The Myo device bridge. Cannot be <c>null</c>.</param>
        /// <returns>
        /// Returns a new <see cref="IMyoDeviceDriver" /> instance.
        /// </returns>
        [Obsolete("Please use the CreateMyoDeviceDriver method that accepts an IMyoErrorHandlerDriver reference.")]
        protected virtual IMyoDeviceDriver CreateMyoDeviceDriver(IntPtr myoHandle, IMyoDeviceBridge myoDeviceBridge)
        {
            Contract.Requires<ArgumentException>(myoHandle != IntPtr.Zero, "The handle to the Myo must be set.");
            Contract.Requires<ArgumentNullException>(myoDeviceBridge != null, "myoDeviceBridge");
            Contract.Ensures(Contract.Result<IMyoDeviceDriver>() != null);

            return CreateMyoDeviceDriver(
                myoHandle, 
                myoDeviceBridge, 
                MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create()));
        }

        /// <summary>
        /// Creates a new <see cref="IMyoDeviceDriver" />.
        /// </summary>
        /// <param name="myoHandle">The Myo handle.</param>
        /// <param name="myoDeviceBridge">The Myo device bridge. Cannot be <c>null</c>.</param>
        /// <param name="myoErrorHandlerDriver">The myo error handler driver. Cannot be <c>null</c>.</param>
        /// <returns>
        /// Returns a new <see cref="IMyoDeviceDriver" /> instance.
        /// </returns>
        protected virtual IMyoDeviceDriver CreateMyoDeviceDriver(IntPtr myoHandle, IMyoDeviceBridge myoDeviceBridge, IMyoErrorHandlerDriver myoErrorHandlerDriver)
        {
            Contract.Requires<ArgumentException>(myoHandle != IntPtr.Zero, "The handle to the Myo must be set.");
            Contract.Requires<ArgumentNullException>(myoDeviceBridge != null, "myoDeviceBridge");
            Contract.Requires<ArgumentNullException>(myoErrorHandlerDriver != null, "myoErrorHandlerDriver");
            Contract.Ensures(Contract.Result<IMyoDeviceDriver>() != null);

            return MyoDeviceDriver.Create(myoHandle, myoDeviceBridge, myoErrorHandlerDriver);
        }

        /// <summary>
        /// Creates a new <see cref="IMyoDeviceBridge"/>.
        /// </summary>
        /// <returns>Returns a new <see cref="IMyoDeviceBridge"/> instance.</returns>
        protected virtual IMyoDeviceBridge CreateMyoDeviceBridge()
        {
            Contract.Ensures(Contract.Result<IMyoDeviceBridge>() != null);

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
                        Contract.Assume(myo != null);

                        UnhookMyoEvents(myo);
                        myo.Dispose();
                    }

                    _myos.Clear();

                    _deviceListener.Paired -= DeviceListener_Paired;
                    _deviceListener.Unpaired -= DeviceListener_Unpaired;
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
            Contract.Requires<ArgumentNullException>(myo != null, "myo");

            myo.Connected += Myo_Connected;
            myo.Disconnected += Myo_Disconnected;
        }

        /// <summary>
        /// Unhooks the Myo events.
        /// </summary>
        /// <param name="myo">The myo to hook onto.</param>
        protected virtual void UnhookMyoEvents(IMyoEventGenerator myo)
        {
            Contract.Requires<ArgumentNullException>(myo != null, "myo");

            myo.Connected -= Myo_Connected;
            myo.Disconnected -= Myo_Disconnected;
        }

        /// <summary>
        /// Raises the <see cref="E:MyoConnected" /> event.
        /// </summary>
        /// <param name="e">The <see cref="MyoEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMyoConnected(MyoEventArgs e)
        {
            Contract.Requires<ArgumentNullException>(e != null, "e");

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
            Contract.Requires<ArgumentNullException>(e != null, "e");

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
            Contract.Requires<ArgumentNullException>(sender != null, "sender");

            if (_myos.ContainsKey(e.MyoHandle))
            {
                return;
            }

            var myoDeviceBridge = CreateMyoDeviceBridge();

            // TODO: replace this obsolete call with the one below
#pragma warning disable 0618
            var myoDeviceDriver = CreateMyoDeviceDriver(
                e.MyoHandle,
                myoDeviceBridge);
#pragma warning restore 0618

            // TODO: use this call once the obsolete method is removed
            ////var myoErrorHandlerDriver = MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create());
            ////var myoDeviceDriver = CreateMyoDeviceDriver(
            ////    e.MyoHandle,
            ////    myoDeviceBridge, 
            ////    myoErrorHandlerDriver);

            var myo = CreateMyo(
                ((IDeviceListener)sender).ChannelListener,
                myoDeviceDriver);

            _myos[myo.Handle] = myo;
            HookMyoEvents(myo);
        }

        private void DeviceListener_Unpaired(object sender, PairedEventArgs e)
        {
            Contract.Requires<ArgumentNullException>(sender != null, "sender");

            IMyo myo;
            if (!_myos.TryGetValue(e.MyoHandle, out myo) || myo == null)
            {
                return;
            }

            UnhookMyoEvents(myo);
            _myos.Remove(myo.Handle);
            myo.Dispose();
        }

        private void Myo_Disconnected(object sender, MyoEventArgs e)
        {
            Contract.Requires<ArgumentNullException>(sender != null, "sender");

            OnMyoDisconnected(e);
        }

        private void Myo_Connected(object sender, MyoEventArgs e)
        {
            Contract.Requires<ArgumentNullException>(sender != null, "sender");

            OnMyoConnected(e);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_myos != null);
            Contract.Invariant(_readonlyMyos != null);
            Contract.Invariant(_deviceListener != null);
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
                Contract.Requires<ArgumentNullException>(myos != null);

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

            [ContractInvariantMethod]
            private void ObjectInvariant()
            {
                Contract.Invariant(_myos != null);
            }
            #endregion
        }
        #endregion
    }
}
