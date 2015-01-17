using System;

using MyoSharp.Device;
using MyoSharp.Communication;
using System.Diagnostics.Contracts;

namespace MyoSharp.Discovery
{
    /// <summary>
    /// A class that can listen for new devices on a specified channel.
    /// </summary>
    public class DeviceListener : IDeviceListener
    {
        #region Fields
        private readonly IChannelListener _channelListener;

        private bool _disposed;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceListener"/> class.
        /// </summary>
        /// <param name="channelListener">The channel listener.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="channelListener"/> is <c>null</c>.</exception>
        protected DeviceListener(IChannelListener channelListener)
        {
            Contract.Requires<ArgumentNullException>(channelListener != null, "channelListener");

            _channelListener = channelListener;
            _channelListener.EventReceived += Channel_EventReceived;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DeviceListener"/> class.
        /// </summary>
        ~DeviceListener()
        {
            Dispose(false);
        }
        #endregion

        #region Events
        /// <inheritdoc />
        public event EventHandler<PairedEventArgs> Paired;

        /// <inheritdoc />
        public event EventHandler<PairedEventArgs> Unpaired;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the <see cref="IChannelListener" /> that this
        /// <see cref="IDeviceListener" /> is listening to events with.
        /// </summary>
        public IChannelListener ChannelListener
        {
            get { return _channelListener; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new <see cref="IDeviceListener"/> instance.
        /// </summary>
        /// <param name="channelListener">The channel listener that will be used to listen for events.</param>
        /// <returns>A new <see cref="IDeviceListener"/> instance.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="channelListener"/> is <c>null</c>.</exception>
        public static IDeviceListener Create(IChannelListener channelListener)
        {
            Contract.Requires<ArgumentNullException>(channelListener != null, "channelListener");
            Contract.Ensures(Contract.Result<IDeviceListener>() != null);

            return new DeviceListener(channelListener);
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
        /// Called when a device has paired.
        /// </summary>
        /// <param name="myoHandle">The Myo handle.</param>
        /// <param name="eventTimeUtc">The event time in UTC.</param>
        protected virtual void OnPaired(IntPtr myoHandle, DateTime eventTimeUtc)
        {
            Contract.Requires<ArgumentException>(myoHandle != IntPtr.Zero, "The handle to the Myo must be set.");

            var handler = Paired;
            if (handler != null)
            {
                var args = new PairedEventArgs(myoHandle, eventTimeUtc);
                handler.Invoke(this, args);
            }
        }

        /// <summary>
        /// Called when a device has been unpaired.
        /// </summary>
        /// <param name="myoHandle">The Myo handle.</param>
        /// <param name="eventTimeUtc">The event time in UTC.</param>
        protected virtual void OnUnpaired(IntPtr myoHandle, DateTime eventTimeUtc)
        {
            Contract.Requires<ArgumentException>(myoHandle != IntPtr.Zero, "The handle to the Myo must be set.");

            var handler = Unpaired;
            if (handler != null)
            {
                var args = new PairedEventArgs(myoHandle, eventTimeUtc);
                handler.Invoke(this, args);
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_channelListener != null);
        }
        #endregion

        #region Event Handlers
        private void Channel_EventReceived(object sender, RouteMyoEventArgs e)
        {
            Contract.Requires<ArgumentNullException>(sender != null, "sender");

            switch (e.EventType)
            {
                case MyoEventType.Paired:
                    OnPaired(e.MyoHandle, e.Timestamp);
                    break;

                case MyoEventType.Unpaired:
                    OnUnpaired(e.MyoHandle, e.Timestamp);
                    break;
            }
        }
        #endregion
    }
}