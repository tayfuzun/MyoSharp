using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Device
{
    public class RouteMyoEventArgs : EventArgs
    {
        #region Constructors
        public RouteMyoEventArgs(IntPtr myohandle, IntPtr evt, MyoEventType eventType, DateTime timestamp)
        {
            this.MyoHandle = myohandle;
            this.Event = evt;
            this.EventType = eventType;
            this.Timestamp = timestamp;
        }
        #endregion

        #region Properties
        public IntPtr MyoHandle { get; private set; }

        public IntPtr Event { get; private set; }

        public MyoEventType EventType { get; private set; }

        public DateTime Timestamp { get; private set; }
        #endregion
    }
}
