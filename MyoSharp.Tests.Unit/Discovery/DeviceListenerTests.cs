using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Xunit;

using Moq;

using MyoSharp.Communication;
using MyoSharp.Discovery;
using MyoSharp.Device;

namespace MyoSharp.Tests.Unit.Discovery
{
    public class DeviceListenerTests
    {
        #region Methods
        [Fact]
        public void Create_NullChannelListener_ThrowsNullArgumentException()
        {
            // Setup

            // Execute
            Assert.ThrowsDelegate method = () => DeviceListener.Create(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(method);
            Assert.Equal("channelListener", exception.ParamName);
        }

        [Fact]
        public void Create_ValidParameters_NewInstance()
        {
            // Setup
            var channel = new Mock<IChannelListener>();

            // Execute
            var result = DeviceListener.Create(channel.Object);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetChannelListener_ValidState_SameInstanceAsConstructor()
        {
            // Setup
            var channel = new Mock<IChannelListener>();
            var deviceListener = DeviceListener.Create(channel.Object);

            // Execute
            var result = deviceListener.ChannelListener;

            // Assert
            Assert.Equal(channel.Object, result);
        }

        [Fact]
        public void EventReceived_EmgData_DoesNotTriggerPairedEvent()
        {
            // Setup
            var channel = new Mock<IChannelListener>();

            var deviceListener = DeviceListener.Create(channel.Object);

            var gotPairedEvent = false;
            deviceListener.Paired += (_, __) => gotPairedEvent = true;

            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(123), 
                MyoEventType.Emg, 
                DateTime.UtcNow);

            // Execute
            channel.Raise(x => x.EventReceived += null, routeEventArgs); 

            // Assert
            Assert.False(
                gotPairedEvent,
                "Not expecting to get the paired event.");
        }

        [Fact]
        public void EventReceived_Paired_TriggersPairedEvent()
        {
            // Setup
            var channel = new Mock<IChannelListener>();

            var deviceListener = DeviceListener.Create(channel.Object);

            PairedEventArgs actualEventArgs = null;
            deviceListener.Paired += (_, args) => actualEventArgs = args;

            var myoHandle = new IntPtr(12345);
            var timestamp = DateTime.UtcNow;
            var routeEventArgs = new RouteMyoEventArgs(
                 myoHandle,
                 new IntPtr(123),
                 MyoEventType.Paired,
                 timestamp);

            // Execute
            channel.Raise(x => x.EventReceived += null, routeEventArgs); 

            // Assert
            Assert.NotNull(actualEventArgs);
            Assert.Equal(myoHandle, actualEventArgs.MyoHandle);
            Assert.Equal(timestamp, actualEventArgs.Timestamp);
        }
        #endregion
    }
}
