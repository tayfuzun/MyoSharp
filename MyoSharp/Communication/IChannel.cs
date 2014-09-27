using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Communication
{
    /// <summary>
    /// An interface that defines functionality for listening on a 
    /// communication channel.
    /// </summary>
    public interface IChannel : IChannelListener, IDisposable
    {
        #region Methods
        /// <summary>
        /// Starts listening on the communication channel.
        /// </summary>
        void StartListening();

        /// <summary>
        /// Stops listening on the communication channel.
        /// </summary>
        void StopListening();
        #endregion
    }
}
