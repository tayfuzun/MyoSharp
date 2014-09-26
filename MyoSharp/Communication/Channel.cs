using System;
using System.Runtime.InteropServices;
using System.Threading;

using MyoSharp.Internal;
using MyoSharp.Device;

namespace MyoSharp.Communication
{
    public class Channel : IChannel
    {
        #region Constants
        private static readonly DateTime TIMESTAMP_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        #endregion

        #region Fields
        private readonly IntPtr _handle;
        private Thread _eventThread;
        private volatile bool _killEventThread;
        private bool _disposed;
        #endregion

        #region Constructors
        protected Channel(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentException("handle", "The handle must be set.");
            }

            _handle = handle;
        }

        ~Channel()
        {
            Dispose(false);
        }
        #endregion

        #region Events
        public event EventHandler<RouteMyoEventArgs> EventReceived;
        #endregion

        #region Methods
        public static IChannel Create(string applicationIdentifier)
        {
            IntPtr handle;
            var result = InitializeMyoHub(
                out handle, 
                applicationIdentifier);

            if (result == MyoResult.ErrorInvalidArgument)
            {
                throw new ArgumentException("applicationIdentifier", "The application identifier was invalid.");
            }

            if (result != MyoResult.Success)
            {
                throw new InvalidOperationException(string.Format("Unable to initialize Hub. Result code {0}.", result));
            }

            return new Channel(handle);
        }

        public void StartListening()
        {
            _killEventThread = false;
            _eventThread = new Thread(new ThreadStart(EventListenerThread));
            _eventThread.IsBackground = true;
            _eventThread.Name = "Myo Channel Listener Thread";
            _eventThread.Start();
        }

        public void StopListening()
        {
            if (_eventThread != null)
            {
                _killEventThread = true;
                _eventThread.Join();
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            try
            {
                StopListening();

                // free managed objects
                ////if (disposing)
                ////{
                //// TODO: implement this if necessary
                ////}

                // free unmanaged objects
                ShutdownMyoHub(_handle);
            }
            finally
            {
                _disposed = true;
            }
        }

        protected virtual void OnEventReceived(IntPtr myoHandle, IntPtr evt, MyoEventType eventType, DateTime timestamp)
        {
            var handler = EventReceived;
            if (handler != null)
            {
                var args = new RouteMyoEventArgs(
                    myoHandle, 
                    evt,
                    eventType,
                    timestamp);
                handler.Invoke(this, args);
            }
        }

        private static void ShutdownMyoHub(IntPtr hubPointer)
        {
            if (hubPointer == IntPtr.Zero)
            {
                return;
            }

            if (PlatformInvocation.Running32Bit)
            {
                shutdown_hub_32(hubPointer, IntPtr.Zero);
            }
            else
            {
                shutdown_hub_64(hubPointer, IntPtr.Zero);
            }
        }

        private static MyoResult InitializeMyoHub(out IntPtr hubPointer, string applicationIdentifier)
        {
            return PlatformInvocation.Running32Bit
                ? init_hub_32(out hubPointer, applicationIdentifier, IntPtr.Zero)
                : init_hub_64(out hubPointer, applicationIdentifier, IntPtr.Zero);
        }

        private static DateTime GetEventTimestamp(IntPtr evt)
        {
            var timestampSeconds = PlatformInvocation.Running32Bit
                ? event_get_timestamp_32(evt)
                : event_get_timestamp_64(evt);
            return TIMESTAMP_EPOCH.AddMilliseconds(timestampSeconds / 1000);
        }

        private static void Run(IntPtr hubHandle, MyoRunHandler handler, IntPtr userData)
        {
            if (PlatformInvocation.Running32Bit)
            {
                run_32(
                    hubHandle,
                    1000 / 20,
                    handler,
                    userData,
                    IntPtr.Zero);
            }
            else
            {
                run_64(
                     hubHandle,
                     1000 / 20,
                     handler,
                     userData,
                     IntPtr.Zero);
            }
        }

        private static MyoEventType GetEventType(IntPtr evt)
        {
            return PlatformInvocation.Running32Bit
                ? event_get_type_32(evt)
                : event_get_type_64(evt);
        }

        private static IntPtr GetMyoForEvent(IntPtr evt)
        {
            return PlatformInvocation.Running32Bit
                ? event_get_myo_32(evt)
                : event_get_myo_64(evt);
        }

        private MyoRunHandlerResult HandleEvent(IntPtr userData, IntPtr evt)
        {
            // check if the event is for us
            if (((GCHandle)userData).Target != this)
            {
                return MyoRunHandlerResult.Continue;
            }

            var type = GetEventType(evt);
            var myoHandle = GetMyoForEvent(evt);
            var timestamp = GetEventTimestamp(evt);
            OnEventReceived(
                myoHandle,
                evt,
                type,
                timestamp);

            return MyoRunHandlerResult.Continue;
        }

        private void EventListenerThread()
        {
            while (!_killEventThread)
            {
                Run(
                    _handle, 
                    (MyoRunHandler)HandleEvent, 
                    (IntPtr)GCHandle.Alloc(this));
            }
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
        private static extern MyoResult init_hub_32(out IntPtr hub, string applicationIdentifier, IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_init_hub", CallingConvention = CallingConvention.Cdecl)]
        private static extern MyoResult init_hub_64(out IntPtr hub, string applicationIdentifier, IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_shutdown_hub", CallingConvention = CallingConvention.Cdecl)]
        private static extern MyoResult shutdown_hub_32(IntPtr hub, IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_shutdown_hub", CallingConvention = CallingConvention.Cdecl)]
        private static extern MyoResult shutdown_hub_64(IntPtr hub, IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_run", CallingConvention = CallingConvention.Cdecl)]
        private static extern MyoResult run_32(IntPtr hub, uint durationMs, MyoRunHandler handler, IntPtr userData, IntPtr error);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_run", CallingConvention = CallingConvention.Cdecl)]
        private static extern MyoResult run_64(IntPtr hub, uint durationMs, MyoRunHandler handler, IntPtr userData, IntPtr error);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate MyoRunHandlerResult MyoRunHandler(IntPtr userData, IntPtr evt);

        private enum MyoRunHandlerResult
        {
            Continue,
            Stop
        }
        #endregion
    }
}