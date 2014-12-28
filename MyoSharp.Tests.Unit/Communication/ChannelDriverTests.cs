using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Xunit;

using Moq;

using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.Internal;

namespace MyoSharp.Tests.Unit.Communication
{
    public class ChannelDriverTests
    {
        #region Methods
        [Fact]
        public void Create_NullChannelBridge_ThrowsNullArgumentException()
        {
            // Setup

            // Execute
            Assert.ThrowsDelegate method = () => ChannelDriver.Create(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(method);
            Assert.Equal("channelBridge", exception.ParamName);
        }

        [Fact]
        public void Create_ValidParameters_NewInstance()
        {
            // Setup
            var bridge = new Mock<IChannelBridge>();            

            // Execute
            var result = ChannelDriver.Create(bridge.Object);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void FreeMyoError_ValidParameters_ExpectedBridgeCalls()
        {
            // Setup
            var errorHandle = new IntPtr(123);
            var bridge = new Mock<IChannelBridge>();
            var driver = ChannelDriver.Create(bridge.Object);

            // Execute
            driver.FreeMyoError(errorHandle);

            // Assert
            bridge.Verify(x => x.LibmyoFreeErrorDetails32(errorHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.LibmyoFreeErrorDetails64(errorHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }

        [Fact]
        public void GetErrorString_ValidParameters_ExpectedBridgeCalls()
        {
            // Setup
            var errorHandle = new IntPtr(123);
            var errorString = "The error";
            var bridge = new Mock<IChannelBridge>();
            bridge
                .Setup(x => x.LibmyoErrorCstring32(errorHandle))
                .Returns(errorString);
            bridge
                .Setup(x => x.LibmyoErrorCstring64(errorHandle))
                .Returns(errorString);

            var driver = ChannelDriver.Create(bridge.Object);

            // Execute
            var result = driver.GetErrorString(errorHandle);

            // Assert
            Assert.Equal(errorString, result);

            bridge.Verify(x => x.LibmyoErrorCstring32(errorHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.LibmyoErrorCstring64(errorHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }

        [Fact]
        public void GetMyoForEvent_ValidParameters_ExpectedBridgeCalls()
        {
            // Setup
            var eventHandle = new IntPtr(123);
            var myoHandle = new IntPtr(789);
            var bridge = new Mock<IChannelBridge>();
            bridge
                .Setup(x => x.EventGetMyo32(eventHandle))
                .Returns(myoHandle);
            bridge
                .Setup(x => x.EventGetMyo64(eventHandle))
                .Returns(myoHandle);

            var driver = ChannelDriver.Create(bridge.Object);

            // Execute
            var result = driver.GetMyoForEvent(eventHandle);

            // Assert
            Assert.Equal(myoHandle, result);

            bridge.Verify(x => x.EventGetMyo32(eventHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.EventGetMyo64(eventHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }

        [Fact]
        public void GetEventType_ValidParameters_ExpectedBridgeCalls()
        {
            // Setup
            var eventHandle = new IntPtr(123);
            var eventType = MyoEventType.Paired;
            var bridge = new Mock<IChannelBridge>();
            bridge
                .Setup(x => x.EventGetType32(eventHandle))
                .Returns(eventType);
            bridge
                .Setup(x => x.EventGetType64(eventHandle))
                .Returns(eventType);

            var driver = ChannelDriver.Create(bridge.Object);

            // Execute
            var result = driver.GetEventType(eventHandle);

            // Assert
            Assert.Equal(eventType, result);

            bridge.Verify(x => x.EventGetType32(eventHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.EventGetType64(eventHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }

        [Fact]
        public void Run_InvalidParameters_ThrowsNullArgumentException()
        {
            // Setup
            var myoHandle = new IntPtr(123);
            var userData = new IntPtr(789);
            var bridge = new Mock<IChannelBridge>();
            var driver = ChannelDriver.Create(bridge.Object);

            // Execute
            Assert.ThrowsDelegate method = () => driver.Run(myoHandle, null, userData);

            // Assert
            var result = Assert.Throws<ArgumentNullException>(method);
            Assert.Equal("handler", result.ParamName);

            IntPtr errorHandle;
            bridge.Verify(x => x.Run32(It.IsAny<IntPtr>(), It.IsAny<uint>(), It.IsAny<MyoRunHandler>(), It.IsAny<IntPtr>(), out errorHandle), Times.Never());
            bridge.Verify(x => x.Run64(It.IsAny<IntPtr>(), It.IsAny<uint>(), It.IsAny<MyoRunHandler>(), It.IsAny<IntPtr>(), out errorHandle), Times.Never());
        }

        [Fact]
        public void Run_ValidParameters_ExpectedBridgeCalls()
        {
            // Setup
            var myoHandle = new IntPtr(123);
            var userData = new IntPtr(789);
            var bridge = new Mock<IChannelBridge>();
            var driver = ChannelDriver.Create(bridge.Object);
            MyoRunHandler handler = (_, __) => MyoRunHandlerResult.Continue;

            // Execute
            driver.Run(myoHandle, handler, userData);

            // Assert
            IntPtr errorHandle;
            bridge.Verify(x => x.Run32(myoHandle, 50, handler, userData, out errorHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.Run64(myoHandle, 50, handler, userData, out errorHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }

        [Fact]
        public void GetEventTimestamp_ValidParameters_ExpectedBridgeCalls()
        {
            // Setup
            var eventHandle = new IntPtr(123);
            var bridge = new Mock<IChannelBridge>();
            bridge
                .Setup(x => x.EventGetTimestamp32(eventHandle))
                .Returns(10000);
            bridge
                .Setup(x => x.EventGetTimestamp64(eventHandle))
                .Returns(10000);

            var driver = ChannelDriver.Create(bridge.Object);
            
            // Execute
            var result = driver.GetEventTimestamp(eventHandle);

            // Assert
            Assert.Equal(new DateTime(1970, 1, 1).Add(TimeSpan.FromMilliseconds(10000 / 1000)), result);

            bridge.Verify(x => x.EventGetTimestamp32(eventHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.EventGetTimestamp64(eventHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }

        [Fact]
        public void InitializeMyoHub_ValidParameters_ExpectedBridgeCalls()
        {
            // Setup
            IntPtr errorHandle;
            IntPtr myoHandle = new IntPtr(123);
            var applicationIdentifier = "com.myosharp.tests";

            var bridge = new Mock<IChannelBridge>();
            bridge
                .Setup(x => x.InitHub32(out myoHandle, applicationIdentifier, out errorHandle))
                .Returns(MyoResult.Success);
            bridge
                .Setup(x => x.InitHub64(out myoHandle, applicationIdentifier, out errorHandle))
                .Returns(MyoResult.Success);

            var driver = ChannelDriver.Create(bridge.Object);
            
            // Execute
            var result = driver.InitializeMyoHub(applicationIdentifier);

            // Assert
            Assert.Equal(new IntPtr(123), result);

            bridge.Verify(x => x.InitHub32(out myoHandle, applicationIdentifier, out errorHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.InitHub64(out myoHandle, applicationIdentifier, out errorHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }

        [Fact]
        public void InitializeMyoHub_HubErrorInvalidArgument_ThrowsArgumentException()
        {
            // Setup
            IntPtr errorHandle;
            IntPtr myoHandle = new IntPtr(123);
            var applicationIdentifier = "com.myosharp.tests";

            var bridge = new Mock<IChannelBridge>();
            bridge
                .Setup(x => x.InitHub32(out myoHandle, applicationIdentifier, out errorHandle))
                .Returns(MyoResult.ErrorInvalidArgument);
            bridge
                .Setup(x => x.InitHub64(out myoHandle, applicationIdentifier, out errorHandle))
                .Returns(MyoResult.ErrorInvalidArgument);

            var driver = ChannelDriver.Create(bridge.Object);

            // Execute
            Assert.ThrowsDelegate method = () => driver.InitializeMyoHub(applicationIdentifier);

            // Assert
            Assert.Throws<ArgumentException>(method);

            bridge.Verify(x => x.InitHub32(out myoHandle, applicationIdentifier, out errorHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.InitHub64(out myoHandle, applicationIdentifier, out errorHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }

        [Fact]
        public void InitializeMyoHub_HubError_ThrowsInvalidOperationException()
        {
            // Setup
            IntPtr errorHandle;
            IntPtr myoHandle = new IntPtr(123);
            var applicationIdentifier = "com.myosharp.tests";

            var bridge = new Mock<IChannelBridge>();
            bridge
                .Setup(x => x.InitHub32(out myoHandle, applicationIdentifier, out errorHandle))
                .Returns(MyoResult.Error);
            bridge
                .Setup(x => x.InitHub64(out myoHandle, applicationIdentifier, out errorHandle))
                .Returns(MyoResult.Error);

            var driver = ChannelDriver.Create(bridge.Object);

            // Execute
            Assert.ThrowsDelegate method = () => driver.InitializeMyoHub(applicationIdentifier);

            // Assert
            Assert.Throws<InvalidOperationException>(method);

            bridge.Verify(x => x.InitHub32(out myoHandle, applicationIdentifier, out errorHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.InitHub64(out myoHandle, applicationIdentifier, out errorHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }
        

        [Fact]
        public void ShutdownMyoHub_ValidParameters_ExpectedBridgeCalls()
        {
            // Setup
            IntPtr errorHandle;
            IntPtr myoHandle = new IntPtr(123);

            var bridge = new Mock<IChannelBridge>();
            bridge
                .Setup(x => x.ShutdownHub32(myoHandle, out errorHandle))
                .Returns(MyoResult.Success);
            bridge
                .Setup(x => x.ShutdownHub64(myoHandle, out errorHandle))
                .Returns(MyoResult.Success);

            var driver = ChannelDriver.Create(bridge.Object);

            // Execute
            driver.ShutdownMyoHub(myoHandle);

            // Assert
            bridge.Verify(x => x.ShutdownHub32(myoHandle, out errorHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.ShutdownHub64(myoHandle, out errorHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }

        [Fact]
        public void ShutdownMyoHub_HubErrorInvalidArgument_ThrowsArgumentException()
        {
            // Setup
            IntPtr errorHandle;
            IntPtr myoHandle = new IntPtr(123);

            var bridge = new Mock<IChannelBridge>();
            bridge
                .Setup(x => x.ShutdownHub32(myoHandle, out errorHandle))
                .Returns(MyoResult.ErrorInvalidArgument);
            bridge
                .Setup(x => x.ShutdownHub64(myoHandle, out errorHandle))
                .Returns(MyoResult.ErrorInvalidArgument);

            var driver = ChannelDriver.Create(bridge.Object);


            // Execute
            Assert.ThrowsDelegate method = () => driver.ShutdownMyoHub(myoHandle);

            // Assert
            Assert.Throws<ArgumentException>(method);

            bridge.Verify(x => x.ShutdownHub32(myoHandle, out errorHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.ShutdownHub64(myoHandle, out errorHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }

        [Fact]
        public void ShutdownMyoHub_HubError_ThrowsInvalidOperationException()
        {
            // Setup
            IntPtr errorHandle;
            IntPtr myoHandle = new IntPtr(123);

            var bridge = new Mock<IChannelBridge>();
            bridge
                .Setup(x => x.ShutdownHub32(myoHandle, out errorHandle))
                .Returns(MyoResult.Error);
            bridge
                .Setup(x => x.ShutdownHub64(myoHandle, out errorHandle))
                .Returns(MyoResult.Error);

            var driver = ChannelDriver.Create(bridge.Object);

            // Execute
            Assert.ThrowsDelegate method = () => driver.ShutdownMyoHub(myoHandle);

            // Assert
            Assert.Throws<InvalidOperationException>(method);

            bridge.Verify(x => x.ShutdownHub32(myoHandle, out errorHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.ShutdownHub64(myoHandle, out errorHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }
        #endregion
    }
}
