using System;
using System.Collections.Generic;
using System.Text;

using MyoSharp.Device;

namespace MyoSharp.Discovery
{
    public interface IHub : IDisposable
    {
        #region Events
        event EventHandler<MyoEventArgs> Paired;
        #endregion

        #region Methods
        void StartListening();

        void StopListening();
        #endregion
    }
}
