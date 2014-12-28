using System;
using MyoSharp.Device;

namespace MyoSharp.Communication
{
    public interface IChannelDriver
    {
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

        string GetErrorString(IntPtr errorHandle);

        void FreeMyoError(IntPtr errorHandle);
    }
}