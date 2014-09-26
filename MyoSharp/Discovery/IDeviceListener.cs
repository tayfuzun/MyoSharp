using System;

using MyoSharp.Device;
using MyoSharp.Communication;

namespace MyoSharp.Discovery
{
    public interface IDeviceListener : IDisposable
    {
        #region Events
        event EventHandler<PairedEventArgs> Paired;
        #endregion

        #region Properties
        IChannelListener ChannelListener { get; }
        #endregion
    }
}
