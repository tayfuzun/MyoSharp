using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Xunit;

using Moq;

using MyoSharp.Device;
using MyoSharp.Internal;
using MyoSharp.Exceptions;

namespace MyoSharp.Tests.Unit.Exceptions
{
    public class MyoErrorHandlerDriverTests
    {
        #region Methods
        [Fact]
        public void Create_NullMyoErrorHandlerBridge_ThrowsNullArgumentException()
        {
            // Setup

            // Execute
            Assert.ThrowsDelegate method = () => MyoErrorHandlerDriver.Create(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(method);
            Assert.Equal("myoErrorHandlerBridge", exception.ParamName);
        }

        [Fact]
        public void Create_ValidParameters_NewInstance()
        {
            // Setup
            var bridge = new Mock<IMyoErrorHandlerBridge>(MockBehavior.Strict);            

            // Execute
            var result = MyoErrorHandlerDriver.Create(bridge.Object);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void FreeMyoError_ValidParameters_ExpectedBridgeCalls()
        {
            // Setup
            var errorHandle = new IntPtr(123);
            
            var bridge = new Mock<IMyoErrorHandlerBridge>(MockBehavior.Strict);
            bridge
                .Setup(x => x.LibmyoFreeErrorDetails32(errorHandle));
            bridge
                .Setup(x => x.LibmyoFreeErrorDetails64(errorHandle));

            var driver = MyoErrorHandlerDriver.Create(bridge.Object);

            // Execute
            driver.FreeMyoError(errorHandle);

            // Assert
            bridge.Verify(x => x.LibmyoFreeErrorDetails32(errorHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.LibmyoFreeErrorDetails64(errorHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }

        [Fact]
        public void FreeMyoError_ErrorHandleNotSet_NoBridgeCalls()
        {
            // Setup
            var errorHandle = IntPtr.Zero;
            var bridge = new Mock<IMyoErrorHandlerBridge>(MockBehavior.Strict);
            var driver = MyoErrorHandlerDriver.Create(bridge.Object);

            // Execute
            driver.FreeMyoError(errorHandle);

            // Assert
            bridge.Verify(x => x.LibmyoFreeErrorDetails32(errorHandle), Times.Never());
            bridge.Verify(x => x.LibmyoFreeErrorDetails64(errorHandle), Times.Never());
        }
        #endregion
    }
}
