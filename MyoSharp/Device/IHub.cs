using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Device
{
    /// <summary>
    /// An interface that defines a hub for managing Myos
    /// </summary>
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
}
