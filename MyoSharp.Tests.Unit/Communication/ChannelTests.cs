using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Xunit;

using Moq;

using MyoSharp.Communication;
using MyoSharp.Device;

namespace MyoSharp.Tests.Unit.Communication
{
    public class ChannelTests
    {
        #region Constants
        private const string APPLICATION_IDENTIFIER = "com.myosharp.tests";
        #endregion

        #region Methods
        [Fact]
        public void Create_NullChannelDriver_ThrowsNullArgumentException()
        {
            // Setup

            // Execute
            Assert.ThrowsDelegate method = () => Channel.Create(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(method);
            Assert.Equal("channelDriver", exception.ParamName);
        }

        [Fact]
        public void Create_NullApplicationIdentifier_ThrowsNullArgumentException()
        {
            // Setup
            var driver = new Mock<IChannelDriver>();

            // Execute
            Assert.ThrowsDelegate method = () => Channel.Create(driver.Object, null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(method);
            Assert.Equal("applicationIdentifier", exception.ParamName);

            driver.Verify(x => x.InitializeMyoHub(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Create_LongApplicationIdentifier_ThrowsArgumentException()
        {
            // Setup
            var driver = new Mock<IChannelDriver>();

            // Execute
            Assert.ThrowsDelegate method = () => Channel.Create(driver.Object, new string('x', 256));

            // Assert
            var exception = Assert.Throws<ArgumentException>(method);
            Assert.Equal("The application identifier cannot be longer than 255 characters.", exception.ParamName);

            driver.Verify(x => x.InitializeMyoHub(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Create_HubInitializationError_ThrowsInvalidOperationException()
        {
            // Setup
            var driver = new Mock<IChannelDriver>();
            driver
                .Setup(x => x.InitializeMyoHub(APPLICATION_IDENTIFIER))
                .Returns(IntPtr.Zero);

            // Execute
            Assert.ThrowsDelegate method = () => Channel.Create(driver.Object, APPLICATION_IDENTIFIER);

            // Assert
            Assert.Throws<InvalidOperationException>(method);

            driver.Verify(x => x.InitializeMyoHub(APPLICATION_IDENTIFIER), Times.Once);
        }

        [Fact]
        public void Create_ValidParameters_NewInstance()
        {
            // Setup
            var driver = new Mock<IChannelDriver>();
            var myoHandle = new IntPtr(12345);
            driver
                .Setup(x => x.InitializeMyoHub(APPLICATION_IDENTIFIER))
                .Returns(myoHandle);

            // Execute
            var result = Channel.Create(driver.Object, APPLICATION_IDENTIFIER);

            // Assert
            Assert.NotNull(result);

            driver.Verify(x => x.InitializeMyoHub(APPLICATION_IDENTIFIER), Times.Once);
        }

        [Fact]
        public void Dispose_CalledDirectly_ShutsDownMyoHub()
        {
            // Setup
            var driver = new Mock<IChannelDriver>();

            var myoHandle = new IntPtr(12345);
            driver
                .Setup(x => x.InitializeMyoHub(APPLICATION_IDENTIFIER))
                .Returns(myoHandle);

            var channel = Channel.Create(driver.Object, APPLICATION_IDENTIFIER);
            
            // Execute
            channel.Dispose();

            // Assert
            driver.Verify(x => x.InitializeMyoHub(APPLICATION_IDENTIFIER), Times.Once);
            driver.Verify(x => x.ShutdownMyoHub(myoHandle), Times.Once);
        }

        [Fact]
        public void ListenerThread_HandleMyoEvent_TriggersEventReceivedEvent()
        {
            // Setup
            var driver = new Mock<IChannelDriver>();

            var timestamp = DateTime.UtcNow;
            var eventHandle = new IntPtr(123);
            var myoHandle = new IntPtr(12345);
            driver
                .Setup(x => x.InitializeMyoHub(APPLICATION_IDENTIFIER))
                .Returns(myoHandle);
            driver
                .Setup(x => x.Run(myoHandle, It.IsAny<MyoRunHandler>(), It.IsAny<IntPtr>()))
                .Callback<IntPtr, MyoRunHandler, IntPtr>((handle, runHandler, userData) =>
                {
                    runHandler(userData, eventHandle);
                });
            driver
                .Setup(x => x.GetEventType(eventHandle))
                .Returns(MyoEventType.Emg);
            driver
                .Setup(x => x.GetMyoForEvent(eventHandle))
                .Returns(myoHandle);
            driver
                .Setup(x => x.GetEventTimestamp(eventHandle))
                .Returns(timestamp);

            var channel = Channel.Create(driver.Object, APPLICATION_IDENTIFIER);

            var trigger = new ManualResetEventSlim();
            object actualSender = null;
            RouteMyoEventArgs actualEventArgs = null;
            channel.EventReceived += (sender, args) =>
            {
                actualSender = sender;
                actualEventArgs = args;
                trigger.Set();
            };

            // Execute
            channel.StartListening();

            // Assert
            try
            {
                Assert.True(
                    trigger.Wait(5000),
                    "The asycnhronous operation did not finish within a reasonable amount of time.");

                Assert.Equal(channel, actualSender);
                Assert.NotNull(actualEventArgs);
                Assert.Equal(myoHandle, actualEventArgs.MyoHandle);
                Assert.Equal(eventHandle, actualEventArgs.Event);
                Assert.Equal(MyoEventType.Emg, actualEventArgs.EventType);
                Assert.Equal(timestamp, actualEventArgs.Timestamp);

                driver.Verify(x => x.InitializeMyoHub(APPLICATION_IDENTIFIER), Times.Once);
                driver.Verify(x => x.Run(myoHandle, It.IsAny<MyoRunHandler>(), It.IsAny<IntPtr>()), Times.AtLeastOnce());
                driver.Verify(x => x.GetEventType(eventHandle), Times.AtLeastOnce());
                driver.Verify(x => x.GetMyoForEvent(eventHandle), Times.AtLeastOnce());
                driver.Verify(x => x.GetEventTimestamp(eventHandle), Times.AtLeastOnce());
            }
            finally
            {
                channel.Dispose();
            }
        }
        #endregion
    }
}
