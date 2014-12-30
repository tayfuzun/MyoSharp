using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace MyoSharp.Device
{
    /// <summary>
    /// An interface that defines a hub for managing Myos
    /// </summary>
    [ContractClass(typeof(IHubContract))]
    public interface IHub : IDisposable
    {
        #region Events
        /// <summary>
        /// The event that is triggered when a Myo has connected.
        /// </summary>
        event EventHandler<MyoEventArgs> MyoConnected;

        /// <summary>
        /// The event that is triggered when a Myo has disconnected.
        /// </summary>
        event EventHandler<MyoEventArgs> MyoDisconnected;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the collection of Myos being managed by this hub.
        /// </summary>
        IReadOnlyMyoCollection Myos { get; }
        #endregion
    }

    [ContractClassFor(typeof(IHub))]
    internal abstract class IHubContract : IHub
    {
        #region Events
        public abstract event EventHandler<MyoEventArgs> MyoConnected;

        public abstract event EventHandler<MyoEventArgs> MyoDisconnected;
        #endregion

        #region Properties
        public IReadOnlyMyoCollection Myos
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyMyoCollection>() != null);

                return null;
            }
        }
        #endregion

        #region Methods
        public abstract void Dispose();
        #endregion
    }
}
