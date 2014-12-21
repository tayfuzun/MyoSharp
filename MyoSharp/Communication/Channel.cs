using System;
using System.Runtime.InteropServices;
using System.Threading;

using MyoSharp.Device;

namespace MyoSharp.Communication
{
    /// <summary>
    /// A class that can listen to Myo Bluetooth data.
    /// </summary>
    public sealed class Channel : IChannel
    {
        #region Fields
        private readonly IntPtr _handle;
        private readonly IChannelDriver _channelDriver;

        private Thread _eventThread;
        private volatile bool _killEventThread;
        private bool _disposed;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Channel" /> class.
        /// </summary>
        /// <param name="channelDriver">The channel driver. Cannot be <c>null</c>.</param>
        /// <param name="applicationIdentifier">The application identifier must follow a reverse domain name format (ex. com.domainname.appname). Application
        /// identifiers can be formed from the set of alphanumeric ASCII characters (a-z, A-Z, 0-9). The hyphen (-) and
        /// underscore (_) characters are permitted if they are not adjacent to a period (.) character  (i.e. not at the
        /// start or end of each segment), but are not permitted in the top-level domain. Application identifiers must have
        /// three or more segments. For example, if a company's domain is example.com and the application is named
        /// hello-world, one could use "com.example.hello-world" as a valid application identifier. The application identifier
        /// can be an empty string. The application identifier cannot be longer than 255 characters.</param>
        /// <param name="autostart">If set to <c>true</c>, the channel will be automatically started.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The exception that is thrown when <paramref name="channelDriver"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The exception that is thrown when the hub fails to initialize.
        /// </exception>
        private Channel(IChannelDriver channelDriver, string applicationIdentifier, bool autostart)
        {
            if (channelDriver == null)
            {
                throw new ArgumentNullException("channelDriver", "The channel driver cannot be null.");
            }

            _channelDriver = channelDriver;

            _handle = channelDriver.InitializeMyoHub(applicationIdentifier);
            if (_handle == IntPtr.Zero)
            {
                throw new InvalidOperationException("After an attempt to initialize the Myo hub, no pointer was provided.");
            }

            if (autostart)
            {
                StartListening();
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Channel"/> class.
        /// </summary>
        ~Channel()
        {
            Dispose(false);
        }
        #endregion

        #region Events
        /// <summary>
        /// The event that is triggered when an event is received on a communication channel.
        /// </summary>
        public event EventHandler<RouteMyoEventArgs> EventReceived;
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new <see cref="IChannel" /> instance.
        /// </summary>
        /// <param name="channelDriver">The channel driver.</param>
        /// <returns>
        /// Returns a new <see cref="IChannel" /> instance.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// The exception that is thrown when <paramref name="channelDriver"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">Thrown when there is a failure to connect to the Bluetooth hub.</exception>
        public static IChannel Create(IChannelDriver channelDriver)
        {
            if (channelDriver == null)
            {
                throw new ArgumentNullException("channelDriver", "The channel driver cannot be null.");
            }

            return Create(channelDriver, string.Empty);
        }

        /// <summary>
        /// Creates a new <see cref="IChannel" /> instance.
        /// </summary>
        /// <param name="channelDriver">The channel driver. Cannot be <c>null</c>.</param>
        /// <param name="applicationIdentifier">The application identifier must follow a reverse domain name format (ex. com.domainname.appname). Application
        /// identifiers can be formed from the set of alphanumeric ASCII characters (a-z, A-Z, 0-9). The hyphen (-) and
        /// underscore (_) characters are permitted if they are not adjacent to a period (.) character  (i.e. not at the
        /// start or end of each segment), but are not permitted in the top-level domain. Application identifiers must have
        /// three or more segments. For example, if a company's domain is example.com and the application is named
        /// hello-world, one could use "com.example.hello-world" as a valid application identifier. The application identifier
        /// can be an empty string. The application identifier cannot be longer than 255 characters.</param>
        /// <returns>
        /// A new <see cref="IChannel" /> instance.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// The exception that is thrown when the <paramref name="applicationIdentifier"/> is invalid.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// The exception that is thrown when <paramref name="channelDriver"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">Thrown when there is a failure to connect to the Bluetooth hub.</exception>
        public static IChannel Create(IChannelDriver channelDriver, string applicationIdentifier)
        {
            if (channelDriver == null)
            {
                throw new ArgumentNullException("channelDriver", "The channel driver cannot be null.");
            }

            return Create(channelDriver, applicationIdentifier, false);
        }

        /// <summary>
        /// Creates a new <see cref="IChannel" /> instance.
        /// </summary>
        /// <param name="channelDriver">The channel driver. Cannot be <c>null</c>.</param>
        /// <param name="applicationIdentifier">The application identifier must follow a reverse domain name format (ex. com.domainname.appname). Application
        /// identifiers can be formed from the set of alphanumeric ASCII characters (a-z, A-Z, 0-9). The hyphen (-) and
        /// underscore (_) characters are permitted if they are not adjacent to a period (.) character  (i.e. not at the
        /// start or end of each segment), but are not permitted in the top-level domain. Application identifiers must have
        /// three or more segments. For example, if a company's domain is example.com and the application is named
        /// hello-world, one could use "com.example.hello-world" as a valid application identifier. The application identifier
        /// can be an empty string. The application identifier cannot be longer than 255 characters.</param>
        /// <param name="autostart">If set to <c>true</c>, the channel will be automatically started.</param>
        /// <returns>
        /// A new <see cref="IChannel" /> instance.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// The exception that is thrown when the <paramref name="applicationIdentifier"/> is invalid.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// The exception that is thrown when <paramref name="channelDriver"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">Thrown when there is a failure to connect to the Bluetooth hub.</exception>
        public static IChannel Create(IChannelDriver channelDriver, string applicationIdentifier, bool autostart)
        {
            if (channelDriver == null)
            {
                throw new ArgumentNullException("channelDriver", "The channel driver cannot be null.");
            }

            return new Channel(channelDriver, applicationIdentifier, autostart);
        }

        /// <summary>
        /// Starts listening on the communication channel.
        /// </summary>
        public void StartListening()
        {
            if (_eventThread != null)
            {
                return;
            }

            _killEventThread = false;
            _eventThread = new Thread(EventListenerThread)
            {
                IsBackground = true,
                Name = "Myo Channel Listener Thread",
            };

            _eventThread.Start();
        }

        /// <summary>
        /// Stops listening on the communication channel.
        /// </summary>
        public void StopListening()
        {
            if (_eventThread != null)
            {
                _killEventThread = true;
                _eventThread.Join();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; 
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        private void Dispose(bool disposing)
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
                if (_channelDriver != null)
                {
                    _channelDriver.ShutdownMyoHub(_handle);
                }
            }
            finally
            {
                _disposed = true;
            }
        }

        /// <summary>
        /// Called when an event has been received on the communication channel.
        /// </summary>
        /// <param name="myoHandle">The Myo handle.</param>
        /// <param name="evt">The event handle.</param>
        /// <param name="eventType">The type of the event.</param>
        /// <param name="timestamp">The timestamp of the event.</param>
        private void OnEventReceived(
            IntPtr myoHandle, 
            IntPtr evt, 
            MyoEventType eventType, 
            DateTime timestamp)
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
        
        private MyoRunHandlerResult HandleEvent(IntPtr userData, IntPtr evt)
        {
            // check if the event is for us
            if (((GCHandle)userData).Target != this)
            {
                return  MyoRunHandlerResult.Continue;
            }

            var type = _channelDriver.GetEventType(evt);
            var myoHandle = _channelDriver.GetMyoForEvent(evt);
            var timestamp = _channelDriver.GetEventTimestamp(evt);
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
                _channelDriver.Run(
                    _handle, 
                    HandleEvent, 
                    (IntPtr)GCHandle.Alloc(this));
            }
        }
        #endregion
    }
}