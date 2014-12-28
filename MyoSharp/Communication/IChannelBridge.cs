using System;
using System.Runtime.InteropServices;

using MyoSharp.Device;

namespace MyoSharp.Communication
{
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

        string LibmyoErrorCstring32(IntPtr errorHandle);

        string LibmyoErrorCstring64(IntPtr errorHandle);

        void LibmyoFreeErrorDetails32(IntPtr errorHandle);

        void LibmyoFreeErrorDetails64(IntPtr errorHandle);
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