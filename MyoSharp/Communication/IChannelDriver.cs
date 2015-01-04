using System;
using System.Diagnostics.Contracts;

using MyoSharp.Device;

namespace MyoSharp.Communication
{
    [ContractClass(typeof(IChannelDriverContract))]
    public interface IChannelDriver
    {
        #region Methods
        void ShutdownMyoHub(IntPtr hubPointer);

        IntPtr InitializeMyoHub(string applicationIdentifier);

        DateTime GetEventTimestamp(IntPtr evt);

        /// <summary>
        /// Runs the specified handler with event data from the Myo.
        /// </summary>
        /// <param name="hubHandle">The hub handle.</param>
        /// <param name="handler">The handler to be used as a callback.</param>
        /// <param name="userData">The pointer to the user data.</param>
        /// <exception cref="ArgumentNullException">
        /// The exception that is thrown when <paramref name="handler"/> is <c>null</c>.
        /// </exception>
        void Run(IntPtr hubHandle, MyoRunHandler handler, IntPtr userData);

        MyoEventType GetEventType(IntPtr evt);

        IntPtr GetMyoForEvent(IntPtr evt);
        #endregion
    }

    [ContractClassFor(typeof(IChannelDriver))]
    internal abstract class IChannelDriverContract : IChannelDriver
    {
        #region Methods
        public void ShutdownMyoHub(IntPtr hubPointer)
        {
            Contract.Requires<ArgumentException>(hubPointer != IntPtr.Zero, "The pointer to the hub must be set.");
        }

        public IntPtr InitializeMyoHub(string applicationIdentifier)
        {
            Contract.Requires<ArgumentNullException>(applicationIdentifier != null, "applicationIdentifier");
            Contract.Ensures(Contract.Result<IntPtr>() != IntPtr.Zero);

            return default(IntPtr);
        }

        public DateTime GetEventTimestamp(IntPtr evt)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The pointer to the event must be set.");
            
            return default(DateTime);
        }

        public void Run(IntPtr hubHandle, MyoRunHandler handler, IntPtr userData)
        {
            Contract.Requires<ArgumentException>(hubHandle != IntPtr.Zero, "The pointer to the hub must be set.");
            Contract.Requires<ArgumentNullException>(handler != null, "handler");
            Contract.Requires<ArgumentException>(userData != IntPtr.Zero, "The pointer to the user data must be set.");
        }

        public MyoEventType GetEventType(IntPtr evt)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The pointer to the event must be set.");

            return default(MyoEventType);
        }

        public IntPtr GetMyoForEvent(IntPtr evt)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The pointer to the event must be set.");
            Contract.Ensures(Contract.Result<IntPtr>() != IntPtr.Zero);

            return default(IntPtr);
        }
        #endregion
    }
}