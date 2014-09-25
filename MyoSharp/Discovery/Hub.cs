using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using MyoSharp.Internal;
using MyoSharp.Device;

namespace MyoSharp.Discovery
{
    public class Hub : IHub
    {
        #region Constants
        private static readonly DateTime TIMESTAMP_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        #endregion

        #region Fields
        private readonly Dictionary<IntPtr, Myo> _myos = new Dictionary<IntPtr, Myo>();

        private bool _disposed;
        private readonly IntPtr _handle;

        private volatile Thread _eventThread;
        #endregion

        #region Constructors
        private Hub(IntPtr hubHandle)
        {
            _handle = hubHandle;
        }

        ~Hub()
        {
            Dispose(false);
        }
        #endregion

        #region Events
        public event EventHandler<MyoEventArgs> Paired;
        #endregion

        #region Methods
        public static IHub Create(string applicationIdentifier)
        {
            IntPtr hubHandle;
            var result = InitializeHub(
                out hubHandle, 
                applicationIdentifier);

            if (result != MyoResult.Success)
            {
                throw new InvalidOperationException(string.Format("Unable to initialize Hub. Result code {0}.", result));
            }

            return new Hub(hubHandle);
        }

        public void StartListening()
        {
            _eventThread = new Thread(new ThreadStart(EventListenerThread));
            _eventThread.IsBackground = true;
            _eventThread.Name = "Myo Hub Listener Thread";
            _eventThread.Start();
        }

        public void StopListening()
        {
            if (_eventThread != null)
            {
                _eventThread.Join();
                _eventThread = null;
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                try
                {
                    StopListening();

                    // free managed objects
                    ////if (disposing)
                    ////{
                    //// TODO: implement this if necessary
                    ////}

                    // free unmanaged objects
                    ShutdownHub(_handle);
                }
                finally
                {
                    _disposed = true;
                }
            }
        }

        protected virtual void OnPaired(Myo myo)
        {
            var handler = Paired;
            if (handler != null)
            {
                var args = new MyoEventArgs(myo, DateTime.Now);
                handler.Invoke(this, args);
            }
        }

        private static void ShutdownHub(IntPtr hubPointer)
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

        private static MyoResult InitializeHub(out IntPtr hubPointer, string applicationIdentifier)
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

            switch (type)
            {
                case MyoEventType.Paired:
                    var myo = new Myo(this, myoHandle);
                    _myos.Add(myoHandle, myo);
                    OnPaired(myo);
                    break;
                case MyoEventType.Disconnected:
                    var timeOfDisconnect= GetEventTimestamp(evt);
                    _myos[myoHandle].HandleEvent(type, timeOfDisconnect, evt);
                    break;
                default:
                    var timestamp = GetEventTimestamp(evt);
                    _myos[myoHandle].HandleEvent(type, timestamp, evt);
                    break;
            }

            return MyoRunHandlerResult.Continue;
        }

        private void EventListenerThread()
        {
            var currentThread = Thread.CurrentThread;
            while (currentThread == _eventThread)
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