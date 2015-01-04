using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

using MyoSharp.Internal;
using MyoSharp.Device;

namespace MyoSharp.Communication
{
    /// <summary>
    /// A class that implements a bridge between channel functionality in this library and another.
    /// </summary>
    public sealed class ChannelBridge : IChannelBridge
    {
        #region Constructors
        /// <summary>
        /// Prevents a default instance of the <see cref="ChannelBridge"/> class from being created.
        /// </summary>
        private ChannelBridge()
        {
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Creates a new <see cref="IChannelBridge"/> instance.
        /// </summary>
        /// <returns>Returns a new <see cref="IChannelBridge"/> instance.</returns>
        public static IChannelBridge Create()
        {
            Contract.Ensures(Contract.Result<IChannelBridge>() != null);

            return new ChannelBridge();
        }

        /// <inheritdoc />
        public ulong EventGetTimestamp32(IntPtr evt)
        {
            return event_get_timestamp_32(evt);
        }

        /// <inheritdoc />
        public ulong EventGetTimestamp64(IntPtr evt)
        {
            return event_get_timestamp_64(evt);
        }

        /// <inheritdoc />
        public MyoEventType EventGetType32(IntPtr evt)
        {
            return event_get_type_32(evt);
        }

        /// <inheritdoc />
        public MyoEventType EventGetType64(IntPtr evt)
        {
            return event_get_type_64(evt);
        }

        /// <inheritdoc />
        public IntPtr EventGetMyo32(IntPtr evt)
        {
            var result = event_get_myo_32(evt);

            if (result == IntPtr.Zero)
            {
                throw new InvalidOperationException("Could not get the Myo handle for the event handle '" + evt + "'.");
            }

            return result;
        }

        /// <inheritdoc />
        public IntPtr EventGetMyo64(IntPtr evt)
        {
            var result = event_get_myo_64(evt);

            if (result == IntPtr.Zero)
            {
                throw new InvalidOperationException("Could not get the Myo handle for the event handle '" + evt + "'.");
            }

            return result;
        }

        /// <inheritdoc />
        public MyoResult InitHub32(out IntPtr hub, string applicationIdentifier, out IntPtr error)
        {
            return init_hub_32(out hub, applicationIdentifier, out error);
        }

        /// <inheritdoc />
        public MyoResult InitHub64(out IntPtr hub, string applicationIdentifier, out IntPtr error)
        {
            return init_hub_64(out hub, applicationIdentifier, out error);
        }

        /// <inheritdoc />
        public MyoResult ShutdownHub32(IntPtr hub, out IntPtr error)
        {
            return shutdown_hub_32(hub, out error);
        }

        /// <inheritdoc />
        public MyoResult ShutdownHub64(IntPtr hub, out IntPtr error)
        {
            return shutdown_hub_64(hub, out error);
        }

        /// <inheritdoc />
        public MyoResult Run32(IntPtr hub, uint durationMs, MyoRunHandler handler, IntPtr userData, out IntPtr error)
        {
            return run_32(hub, durationMs, handler, userData, out error);
        }

        /// <inheritdoc />
        public MyoResult Run64(IntPtr hub, uint durationMs, MyoRunHandler handler, IntPtr userData, out IntPtr error)
        {
            return run_64(hub, durationMs, handler, userData, out error);
        }
        #endregion

        #region PInvokes
        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_event_get_timestamp", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong event_get_timestamp_32(IntPtr evt);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_event_get_timestamp", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong event_get_timestamp_64(IntPtr evt);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_event_get_type", CallingConvention = CallingConvention.Cdecl)]
        private static extern MyoEventType event_get_type_32(IntPtr evt);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_event_get_type", CallingConvention = CallingConvention.Cdecl)]
        private static extern MyoEventType event_get_type_64(IntPtr evt);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_event_get_myo", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr event_get_myo_32(IntPtr evt);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_event_get_myo", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr event_get_myo_64(IntPtr evt);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_init_hub", CallingConvention = CallingConvention.Cdecl)]
        private static extern MyoResult init_hub_32(out IntPtr hub, string applicationIdentifier, out IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_init_hub", CallingConvention = CallingConvention.Cdecl)]
        private static extern MyoResult init_hub_64(out IntPtr hub, string applicationIdentifier, out IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_shutdown_hub", CallingConvention = CallingConvention.Cdecl)]
        private static extern MyoResult shutdown_hub_32(IntPtr hub, out IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_shutdown_hub", CallingConvention = CallingConvention.Cdecl)]
        private static extern MyoResult shutdown_hub_64(IntPtr hub, out IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_run", CallingConvention = CallingConvention.Cdecl)]
        private static extern MyoResult run_32(IntPtr hub, uint durationMs, MyoRunHandler handler, IntPtr userData, out IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_run", CallingConvention = CallingConvention.Cdecl)]
        private static extern MyoResult run_64(IntPtr hub, uint durationMs, MyoRunHandler handler, IntPtr userData, out IntPtr error);
        #endregion
    }
}