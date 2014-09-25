using System;
using System.Collections.Generic;
using System.Text;

using MyoSharp.Device;

namespace MyoSharp.Discovery
{
    public interface IHub : IDisposable
    {
        #region Events
        event EventHandler<PairedEventArgs> Paired;

        event EventHandler<RouteMyoEventArgs> RouteMyoEvent;
        #endregion

        #region Methods
        void StartListening();

        void StopListening();
        #endregion
    }
}
