using System;

namespace MyoSharp.Communication
{
    /// <summary>
    /// An interface that defines functionality for listening for events on a 
    /// communication channel.
    /// </summary>
    public interface IChannelListener
    {
        #region Events
        /// <summary>
        /// The event that is triggered when an event is received on a communication channel.
        /// </summary>
        event EventHandler<RouteMyoEventArgs> EventReceived;
        #endregion
    }
}
