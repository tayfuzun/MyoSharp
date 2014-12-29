using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace MyoSharp.Device
{
    /// <summary>
    /// A class that contains information about an event raised by the Myo.
    /// </summary>
    public class MyoEventArgs : EventArgs
    {
        #region Fields
        private readonly IMyo _myo;
        private readonly DateTime _timestamp;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MyoEventArgs" /> class.
        /// </summary>
        /// <param name="myo">The Myo that raised the event. Cannot be <c>null</c>.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The exception that is thrown when <paramref name="myo"/> is <c>null</c>.
        /// </exception>
        public MyoEventArgs(IMyo myo, DateTime timestamp)
        {
            Contract.Requires<ArgumentNullException>(myo != null, "myo");

            _myo = myo;
            _timestamp = timestamp;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the Myo that raised the event.
        /// </summary>
        public IMyo Myo
        {
            get
            {
                Contract.Ensures(Contract.Result<IMyo>() != null);

                return _myo;
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
            Contract.Invariant(_myo != null);
        }
        #endregion
    }
}