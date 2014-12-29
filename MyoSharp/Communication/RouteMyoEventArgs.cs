using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

using MyoSharp.Device;

namespace MyoSharp.Communication
{
    /// <summary>
    /// A class that contains information about an event for routing Myo events.
    /// </summary>
    public class RouteMyoEventArgs : EventArgs
    {
        #region Fields
        private readonly IntPtr _myoHandle;
        private readonly IntPtr _eventHandle;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RouteMyoEventArgs"/> class.
        /// </summary>
        /// <param name="myoHandle">The Myo handle.</param>
        /// <param name="evt">The event handle.</param>
        /// <param name="eventType">The type of the event.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        /// <exception cref="ArgumentException">
        /// The exception that is thrown when <paramref name="myoHandle"/> or <paramref name="evt"/> are <see cref="IntPtr.Zero"/>.
        /// </exception>
        public RouteMyoEventArgs(
            IntPtr myoHandle, 
            IntPtr evt, 
            MyoEventType eventType, 
            DateTime timestamp)
        {
            Contract.Requires<ArgumentException>(myoHandle != IntPtr.Zero, "myoHandle");
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "evt");
            
            _myoHandle = myoHandle;
            _eventHandle = evt;
            this.EventType = eventType;
            this.Timestamp = timestamp;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the Myo handle.
        /// </summary>
        public IntPtr MyoHandle
        {
            get
            {
                Contract.Ensures(Contract.Result<IntPtr>() != IntPtr.Zero);

                return _myoHandle;
            }
        }

        /// <summary>
        /// Gets the event handle.
        /// </summary>
        public IntPtr Event
        {
            get
            {
                Contract.Ensures(Contract.Result<IntPtr>() != IntPtr.Zero);

                return _eventHandle;
            }
        }

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        public MyoEventType EventType { get; private set; }

        /// <summary>
        /// Gets the timestamp of the event.
        /// </summary>
        public DateTime Timestamp { get; private set; }
        #endregion

        #region Methods
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_myoHandle != IntPtr.Zero);
            Contract.Invariant(_eventHandle != IntPtr.Zero);
        }
        #endregion
    }
}
