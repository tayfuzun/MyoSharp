using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Communication
{
    public interface IChannelListener
    {
        event EventHandler<RouteMyoEventArgs> EventReceived;
    }
}
