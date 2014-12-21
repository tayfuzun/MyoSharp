using System;
using MyoSharp.Device;

namespace MyoSharp.Communication
{
    public interface IChannelDriver
    {
        void ShutdownMyoHub(IntPtr hubPointer);

        IntPtr InitializeMyoHub(string applicationIdentifier);

        DateTime GetEventTimestamp(IntPtr evt);

        void Run(IntPtr hubHandle, MyoRunHandler handler, IntPtr userData);

        MyoEventType GetEventType(IntPtr evt);

        IntPtr GetMyoForEvent(IntPtr evt);

        string GetErrorString(IntPtr errorHandle);

        void FreeMyoError(IntPtr errorHandle);
    }
}