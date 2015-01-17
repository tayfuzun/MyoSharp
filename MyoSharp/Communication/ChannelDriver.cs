using System;
using System.Diagnostics.Contracts;

using MyoSharp.Internal;
using MyoSharp.Device;
using MyoSharp.Commands;
using MyoSharp.Exceptions;

namespace MyoSharp.Communication
{
    /// <summary>
    /// A class that implements the low level functionality of a channel.
    /// </summary>
    public sealed class ChannelDriver : IChannelDriver
    {
        #region Constants
        private static readonly DateTime TIMESTAMP_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        #endregion

        #region Fields
        private readonly IChannelBridge _channelBridge;
        private readonly IMyoErrorHandlerDriver _myoErrorHandlerDriver;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelDriver"/> class.
        /// </summary>
        /// <param name="channelBridge">The channel bridge. Cannot be <c>null</c>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The exception that is thrown when <paramref name="channelBridge"/> is null.
        /// </exception>
        private ChannelDriver(IChannelBridge channelBridge, IMyoErrorHandlerDriver myoErrorHandlerDriver)
        {
            Contract.Requires<ArgumentNullException>(channelBridge != null, "channelBridge");
            Contract.Requires<ArgumentNullException>(myoErrorHandlerDriver != null, "myoErrorHandlerDriver");

            _channelBridge = channelBridge;
            _myoErrorHandlerDriver = myoErrorHandlerDriver;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new <see cref="IChannelDriver"/> instance.
        /// </summary>
        /// <param name="channelBridge">The channel bridge. Cannot be <c>null</c>.</param>
        /// <returns>Returns a new <see cref="IChannelDriver"/> instance.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The exception that is thrown when <paramref name="channelBridge"/> is null.
        /// </exception>
        [Obsolete("Please switch to the create method that takes in an IMyoErrorHandlerDriver parameter.")]
        public static IChannelDriver Create(IChannelBridge channelBridge)
        {
            Contract.Requires<ArgumentNullException>(channelBridge != null, "channelBridge");
            Contract.Ensures(Contract.Result<IChannelDriver>() != null);

            return Create(channelBridge, MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create()));
        }

        /// <summary>
        /// Creates a new <see cref="IChannelDriver"/> instance.
        /// </summary>
        /// <param name="channelBridge">The channel bridge. Cannot be <c>null</c>.</param>
        /// <param name="myoErrorHandlerDriver">The error handler driver. Cannot be <c>null</c>.</param>
        /// <returns>Returns a new <see cref="IChannelDriver"/> instance.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The exception that is thrown when <paramref name="channelBridge"/> or <paramref name="myoErrorHandlerDriver" is null.
        /// </exception>
        public static IChannelDriver Create(IChannelBridge channelBridge, IMyoErrorHandlerDriver myoErrorHandlerDriver)
        {
            Contract.Requires<ArgumentNullException>(channelBridge != null, "channelBridge");
            Contract.Requires<ArgumentNullException>(myoErrorHandlerDriver != null, "myoErrorHandlerDriver");
            Contract.Ensures(Contract.Result<IChannelDriver>() != null);

            return new ChannelDriver(channelBridge, myoErrorHandlerDriver);
        }

        /// <inheritdoc />
        public void ShutdownMyoHub(IntPtr hubPointer)
        {
            if (hubPointer == IntPtr.Zero)
            {
                return;
            }

            var command = MyoCommand.Create(
                _myoErrorHandlerDriver,
                () =>
                {
                    IntPtr errorHandle;
                    var result = PlatformInvocation.Running32Bit
                       ? _channelBridge.ShutdownHub32(hubPointer, out errorHandle)
                       : _channelBridge.ShutdownHub64(hubPointer, out errorHandle);

                    return MyoCommandResult.Create(result, errorHandle);
                });
            command.Execute();
        }

        /// <inheritdoc />
        public IntPtr InitializeMyoHub(string applicationIdentifier)
        {
            var hubPointer = IntPtr.Zero;

            var command = MyoCommand.Create(
                _myoErrorHandlerDriver,
                () =>
                {
                    IntPtr errorHandle;
                    var result = PlatformInvocation.Running32Bit
                        ? _channelBridge.InitHub32(out hubPointer, applicationIdentifier, out errorHandle)
                        : _channelBridge.InitHub64(out hubPointer, applicationIdentifier, out errorHandle);

                    return MyoCommandResult.Create(result, errorHandle);
                });
            command.Execute();

            Contract.Assume(hubPointer != IntPtr.Zero);
            return hubPointer;
        }

        /// <inheritdoc />
        public DateTime GetEventTimestamp(IntPtr evt)
        {
            var microseconds = PlatformInvocation.Running32Bit
                ? _channelBridge.EventGetTimestamp32(evt)
                : _channelBridge.EventGetTimestamp64(evt);
            return TIMESTAMP_EPOCH.AddMilliseconds(microseconds / 1000d);
        }

        /// <inheritdoc />
        public void Run(IntPtr hubHandle, MyoRunHandler handler, IntPtr userData)
        {
            var command = MyoCommand.Create(
                _myoErrorHandlerDriver,
                () =>
                {
                    IntPtr errorHandle;
                    var result = PlatformInvocation.Running32Bit
                           ? _channelBridge.Run32(
                               hubHandle,
                               1000 / 20,
                               handler,
                               userData,
                               out errorHandle)
                           : _channelBridge.Run64(
                                hubHandle,
                                1000 / 20,
                                handler,
                                userData,
                                out errorHandle);

                    return MyoCommandResult.Create(result, errorHandle);
                });
            command.Execute();
        }

        /// <inheritdoc />
        public MyoEventType GetEventType(IntPtr evt)
        {
            return PlatformInvocation.Running32Bit
                ? _channelBridge.EventGetType32(evt)
                : _channelBridge.EventGetType64(evt);
        }

        /// <inheritdoc />
        public IntPtr GetMyoForEvent(IntPtr evt)
        {
            return PlatformInvocation.Running32Bit
                ? _channelBridge.EventGetMyo32(evt)
                : _channelBridge.EventGetMyo64(evt);
        }
        
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_channelBridge != null);
            Contract.Invariant(_myoErrorHandlerDriver != null);
        }
        #endregion
    }
}