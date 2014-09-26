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
        protected DeviceListener(IChannelListener channelListener)
        {
            if (channelListener == null)
            {
                throw new ArgumentNullException("channelListener", "The channel cannot be null.");
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
        public event EventHandler<PairedEventArgs> Paired;
        #endregion

        #region Properties
        public IChannelListener ChannelListener
        {
            get { return _channelListener; }
        }
        #endregion

        #region Methods
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