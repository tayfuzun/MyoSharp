using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace MyoSharp.Discovery
{
    /// <summary>
    /// A class that contains information about a paired event for a device.
    /// </summary>
    public class PairedEventArgs : EventArgs
    {
        #region Fields
        private readonly IntPtr _myoHandle;
        private readonly DateTime _timestamp;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PairedEventArgs"/> class.
        /// </summary>
        /// <param name="myoHandle">The myo Handle.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        /// <exception cref="System.ArgumentException">
        /// The exception that is thrown when <paramref name="myoHandle"/> is <see cref="IntPtr.Zero"/>.
        /// </exception>
        public PairedEventArgs(IntPtr myoHandle, DateTime timestamp)
        {
            Contract.Requires<ArgumentException>(myoHandle != IntPtr.Zero, "The handle to the Myp must be set.");

            _myoHandle = myoHandle;
            _timestamp = timestamp;
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
        /// Gets the timestamp of the event.
        /// </summary>
        public DateTime Timestamp
        {
            get { return _timestamp; }
        }
        #endregion

        #region Methods
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_myoHandle != IntPtr.Zero);
        }
        #endregion
    }
}
