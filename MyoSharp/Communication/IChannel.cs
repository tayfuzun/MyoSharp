using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Communication
{
    public interface IChannel : IChannelListener, IDisposable
    {
        #region Methods
        void StartListening();

        void StopListening();
        #endregion
    }
}
