using System;

using MyoSharp.Device;
using MyoSharp.Communication;

namespace MyoSharp.Discovery
{
    public class DeviceListener : IDeviceListener
    {
        #region Constants
        private static readonly DateTime TIMESTAMP_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        #endregion

        #region Fields
        private readonly IChannelListener _channelListener;
        private bool _disposed;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceListener"/> class.
        /// </summary>
        /// <param name="channelListener">The channel listener.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the channel listener is null.</exception>
        protected DeviceListener(IChannelListener channelListener)
        {
            if (channelListener == null)
            {
                throw new ArgumentNullException("channelListener", "The channel listener cannot be null.");
            }

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
        /// <summary>
        /// The event that is triggered when a device has paired.
        /// </summary>
        public event EventHandler<PairedEventArgs> Paired;
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
        /// <exception cref="System.ArgumentNullException">Thrown when the channel listener is null.</exception>
        public static IDeviceListener Create(IChannelListener channelListener)
        {
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
                    if (_channelListener != null)
                    {
                        _channelListener.EventReceived -= Channel_EventReceived;
                    }
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
        protected virtual void OnPaired(IntPtr myoHandle)
        {
            var handler = Paired;
            if (handler != null)
            {
                var args = new PairedEventArgs(myoHandle, DateTime.Now);
                handler.Invoke(this, args);
            }
        }
        #endregion

        #region Event Handlers
        private void Channel_EventReceived(object sender, RouteMyoEventArgs e)
        {
            if (e.EventType == MyoEventType.Paired)
            {
                OnPaired(e.MyoHandle);
            }
        }
        #endregion
    }
}