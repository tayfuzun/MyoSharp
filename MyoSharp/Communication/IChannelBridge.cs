using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

using MyoSharp.Device;

namespace MyoSharp.Communication
{
    [ContractClass(typeof(IChannelBridgeContract))]
    public interface IChannelBridge
    {
        #region Methods
        ulong EventGetTimestamp32(IntPtr evt);

        ulong EventGetTimestamp64(IntPtr evt);

        MyoEventType EventGetType32(IntPtr evt);

        MyoEventType EventGetType64(IntPtr evt);

        IntPtr EventGetMyo32(IntPtr evt);

        IntPtr EventGetMyo64(IntPtr evt);

        MyoResult InitHub32(out IntPtr hub, string applicationIdentifier, out IntPtr error);

        MyoResult InitHub64(out IntPtr hub, string applicationIdentifier, out IntPtr error);

        MyoResult ShutdownHub32(IntPtr hub, out IntPtr error);

        MyoResult ShutdownHub64(IntPtr hub, out IntPtr error);

        MyoResult Run32(IntPtr hub, uint durationMs, MyoRunHandler handler, IntPtr userData, out IntPtr error);

        MyoResult Run64(IntPtr hub, uint durationMs, MyoRunHandler handler, IntPtr userData, out IntPtr error);
        #endregion
    }

    [ContractClassFor(typeof(IChannelBridge))]
    internal abstract class IChannelBridgeContract : IChannelBridge
    {
        #region Methods
        public ulong EventGetTimestamp32(IntPtr evt)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The event pointer must be set.");

            return default(ulong);
        }

        public ulong EventGetTimestamp64(IntPtr evt)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The event pointer must be set.");

            return default(ulong);
        }

        public MyoEventType EventGetType32(IntPtr evt)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The event pointer must be set.");

            return default(MyoEventType);
        }

        public MyoEventType EventGetType64(IntPtr evt)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The event pointer must be set.");

            return default(MyoEventType);
        }

        public IntPtr EventGetMyo32(IntPtr evt)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The event pointer must be set.");
            Contract.Ensures(Contract.Result<IntPtr>() != IntPtr.Zero);

            return default(IntPtr);
        }

        public IntPtr EventGetMyo64(IntPtr evt)
        {
            Contract.Requires<ArgumentException>(evt != IntPtr.Zero, "The event pointer must be set.");
            Contract.Ensures(Contract.Result<IntPtr>() != IntPtr.Zero);

            return default(IntPtr);
        }

        public MyoResult InitHub32(out IntPtr hub, string applicationIdentifier, out IntPtr error)
        {
            Contract.Requires<ArgumentNullException>(applicationIdentifier != null, "applicationIdentifier");

            hub = default(IntPtr);
            error = default(IntPtr);
            return default(MyoResult);
        }

        public MyoResult InitHub64(out IntPtr hub, string applicationIdentifier, out IntPtr error)
        {
            Contract.Requires<ArgumentNullException>(applicationIdentifier != null, "applicationIdentifier");

            hub = default(IntPtr);
            error = default(IntPtr);
            return default(MyoResult);
        }

        public MyoResult ShutdownHub32(IntPtr hub, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(hub != IntPtr.Zero, "The pointer to the hub must be set.");

            error = default(IntPtr);
            return default(MyoResult);
        }

        public MyoResult ShutdownHub64(IntPtr hub, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(hub != IntPtr.Zero, "The pointer to the hub must be set.");

            error = default(IntPtr);
            return default(MyoResult);
        }

        public MyoResult Run32(IntPtr hub, uint durationMs, MyoRunHandler handler, IntPtr userData, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(hub != IntPtr.Zero, "The pointer to the hub must be set.");
            Contract.Requires<ArgumentException>(userData != IntPtr.Zero, "The pointer to user data must be set.");
            Contract.Requires<NullReferenceException>(handler != null, "handler");

            error = default(IntPtr);
            return default(MyoResult);
        }

        public MyoResult Run64(IntPtr hub, uint durationMs, MyoRunHandler handler, IntPtr userData, out IntPtr error)
        {
            Contract.Requires<ArgumentException>(hub != IntPtr.Zero, "The pointer to the hub must be set.");
            Contract.Requires<ArgumentException>(userData != IntPtr.Zero, "The pointer to user data must be set.");
            Contract.Requires<NullReferenceException>(handler != null, "handler");

            error = default(IntPtr);
            return default(MyoResult);
        }
        #endregion
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate MyoRunHandlerResult MyoRunHandler(IntPtr userData, IntPtr evt);

    public enum MyoRunHandlerResult
    {
        Continue,
        Stop
    }
}