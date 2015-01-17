using System;
using System.Diagnostics.Contracts;

using MyoSharp.Communication;

namespace MyoSharp.Discovery
{
    /// <summary>
    /// An interface that defines functionality for listening to device connectivity.
    /// </summary>
    [ContractClass(typeof(IDeviceListenerContract))]
    public interface IDeviceListener : IDisposable
    {
        #region Events
        /// <summary>
        /// The event that is triggered when a device has paired.
        /// </summary>
        event EventHandler<PairedEventArgs> Paired;

        /// <summary>
        /// The event that is triggered when a device has been unpaired.
        /// </summary>
        event EventHandler<PairedEventArgs> Unpaired;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the <see cref="IChannelListener"/> that this 
        /// <see cref="IDeviceListener"/> is listening to events with.
        /// </summary>
        IChannelListener ChannelListener { get; }
        #endregion
    }

    [ContractClassFor(typeof(IDeviceListener))]
    internal abstract class IDeviceListenerContract : IDeviceListener
    {
        #region Events
        public abstract event EventHandler<PairedEventArgs> Paired;

        public abstract event EventHandler<PairedEventArgs> Unpaired;
        #endregion

        #region Properties
        public IChannelListener ChannelListener
        {
            get
            {
                Contract.Ensures(Contract.Result<IChannelListener>() != null);

                return null;
            }
        }
        #endregion

        #region Methods
        public abstract void Dispose();
        #endregion
    }
}
