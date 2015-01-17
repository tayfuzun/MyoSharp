using System;

using Xunit;

using Moq;

using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.Exceptions;
using MyoSharp.Poses;
using MyoSharp.Math;
using MyoSharp.Internal;

namespace MyoSharp.Tests.Unit.Device
{
    public class MyoDeviceDriverTests
    {
        #region Methods
        [Fact]
        public void Create_DevicePointerNotSet_ThrowsArgumentException()
        {
            // Setup
            var bridge = new Mock<IMyoDeviceBridge>(MockBehavior.Strict);
            var myoErrorHandlerDriver = new Mock<IMyoErrorHandlerDriver>(MockBehavior.Strict);

            // Execute
            Assert.ThrowsDelegate method = () => MyoDeviceDriver.Create(
                IntPtr.Zero,
                bridge.Object,
                myoErrorHandlerDriver.Object);

            // Assert
            var exception = Assert.Throws<ArgumentException>(method);
            Assert.Equal("handle", exception.ParamName);
        }

        [Fact]
        public void Create_NullMyoDeviceBridge_ThrowsArgumentNullException()
        {
            // Setup
            var myoHandle = new IntPtr(123);
            var myoErrorHandlerDriver = new Mock<IMyoErrorHandlerDriver>(MockBehavior.Strict);

            // Execute
            Assert.ThrowsDelegate method = () => MyoDeviceDriver.Create(
                myoHandle,
                null,
                myoErrorHandlerDriver.Object);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(method);
            Assert.Equal("myoDeviceBridge", exception.ParamName);
        }

        public void Create_NullMyoErrorHandlerDriver_ThrowsArgumentNullException()
        {
            // Setup
            var myoHandle = new IntPtr(123);
            var bridge = new Mock<IMyoDeviceBridge>(MockBehavior.Strict);

            // Execute
            Assert.ThrowsDelegate method = () => MyoDeviceDriver.Create(
                myoHandle,
                bridge.Object,
                null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(method);
            Assert.Equal("myoErrorHandlerDriver", exception.ParamName);
        }

        [Fact]
        public void Create_ValidArguments_NewInstance()
        {
            // Setup
            var myoHandle = new IntPtr(123);
            var bridge = new Mock<IMyoDeviceBridge>(MockBehavior.Strict);
            var myoErrorHandlerDriver = new Mock<IMyoErrorHandlerDriver>(MockBehavior.Strict);

            // Execute
            var result = MyoDeviceDriver.Create(
                myoHandle,
                bridge.Object,
                myoErrorHandlerDriver.Object);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetHandle_ValidState_MatchesConstructorParameter()
        {
            // Setup
            var myoHandle = new IntPtr(123);
            var bridge = new Mock<IMyoDeviceBridge>(MockBehavior.Strict);
            var myoErrorHandlerDriver = new Mock<IMyoErrorHandlerDriver>(MockBehavior.Strict);
            var driver = MyoDeviceDriver.Create(
                myoHandle,
                bridge.Object,
                myoErrorHandlerDriver.Object);

            // Execute
            var result = driver.Handle;

            // Assert
            Assert.Equal(myoHandle, result);
        }

        [Fact]
        public void GetArm_ValidParameter_ExpectedBridgeCalls()
        {
            // Setup
            var myoHandle = new IntPtr(123);
            var eventHandle = new IntPtr(789);

            var bridge = new Mock<IMyoDeviceBridge>(MockBehavior.Strict);
            bridge
                .Setup(x => x.EventGetArm32(eventHandle))
                .Returns(Arm.Left);
            bridge
                .Setup(x => x.EventGetArm64(eventHandle))
                .Returns(Arm.Left);

            var myoErrorHandlerDriver = new Mock<IMyoErrorHandlerDriver>(MockBehavior.Strict);
            
            var driver = MyoDeviceDriver.Create(
                myoHandle,
                bridge.Object,
                myoErrorHandlerDriver.Object);

            // Execute
            var result = driver.GetArm(eventHandle);
            
            // Assert
            Assert.Equal(Arm.Left, result);

            bridge.Verify(x => x.EventGetArm32(eventHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.EventGetArm64(eventHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }

        [Fact]
        public void GetEventAccelerometer_ValidParameters_ExpectedBridgeCalls()
        {
            // Setup
            var myoHandle = new IntPtr(123);
            var eventHandle = new IntPtr(789);

            var expectedResult = new Vector3F(10, 20, 30);

            var bridge = new Mock<IMyoDeviceBridge>(MockBehavior.Strict);
            bridge
                .Setup(x => x.EventGetAccelerometer32(eventHandle, 0))
                .Returns(expectedResult.X);
            bridge
                .Setup(x => x.EventGetAccelerometer32(eventHandle, 1))
                .Returns(expectedResult.Y);
            bridge
                .Setup(x => x.EventGetAccelerometer32(eventHandle, 2))
                .Returns(expectedResult.Z);
            bridge
                .Setup(x => x.EventGetAccelerometer64(eventHandle, 0))
                .Returns(expectedResult.X);
            bridge
                .Setup(x => x.EventGetAccelerometer64(eventHandle, 1))
                .Returns(expectedResult.Y);
            bridge
                .Setup(x => x.EventGetAccelerometer64(eventHandle, 2))
                .Returns(expectedResult.Z);

            var myoErrorHandlerDriver = new Mock<IMyoErrorHandlerDriver>(MockBehavior.Strict);

            var driver = MyoDeviceDriver.Create(
                myoHandle,
                bridge.Object,
                myoErrorHandlerDriver.Object);

            // Execute
            var result = driver.GetEventAccelerometer(eventHandle);

            // Assert
            Assert.Equal(expectedResult.X, result.X);
            Assert.Equal(expectedResult.Y, result.Y);
            Assert.Equal(expectedResult.Z, result.Z);

            bridge.Verify(x => x.EventGetAccelerometer32(eventHandle, 0), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.EventGetAccelerometer32(eventHandle, 1), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.EventGetAccelerometer32(eventHandle, 2), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());

            bridge.Verify(x => x.EventGetAccelerometer64(eventHandle, 0), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
            bridge.Verify(x => x.EventGetAccelerometer64(eventHandle, 1), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
            bridge.Verify(x => x.EventGetAccelerometer64(eventHandle, 2), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }

        [Fact]
        public void GetEventDirectionX_ValidParameters_ExpectedBridgeCalls()
        {
            // Setup
            var myoHandle = new IntPtr(123);
            var eventHandle = new IntPtr(789);

            var bridge = new Mock<IMyoDeviceBridge>(MockBehavior.Strict);
            bridge
                .Setup(x => x.EventGetXDirection32(eventHandle))
                .Returns(XDirection.TowardElbow);
            bridge
                .Setup(x => x.EventGetXDirection64(eventHandle))
                .Returns(XDirection.TowardElbow);

            var myoErrorHandlerDriver = new Mock<IMyoErrorHandlerDriver>(MockBehavior.Strict);

            var driver = MyoDeviceDriver.Create(
                myoHandle,
                bridge.Object,
                myoErrorHandlerDriver.Object);

            // Execute
            var result = driver.GetEventDirectionX(eventHandle);

            // Assert
            Assert.Equal(XDirection.TowardElbow, result);

            bridge.Verify(x => x.EventGetXDirection32(eventHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.EventGetXDirection64(eventHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }

        [Fact]
        public void GetEventEmg_ValidParameters_ExpectedBridgeCalls()
        {
            // Setup
            sbyte expectedResult = 10;
            var sensorIndex = 0;
            var myoHandle = new IntPtr(123);
            var eventHandle = new IntPtr(789);

            var bridge = new Mock<IMyoDeviceBridge>(MockBehavior.Strict);
            bridge
                .Setup(x => x.EventGetEmg32(eventHandle, sensorIndex))
                .Returns(expectedResult);
            bridge
                .Setup(x => x.EventGetEmg64(eventHandle, sensorIndex))
                .Returns(expectedResult);

            var myoErrorHandlerDriver = new Mock<IMyoErrorHandlerDriver>(MockBehavior.Strict);

            var driver = MyoDeviceDriver.Create(
                myoHandle,
                bridge.Object,
                myoErrorHandlerDriver.Object);

            // Execute
            var result = driver.GetEventEmg(eventHandle, sensorIndex);

            // Assert
            Assert.Equal(expectedResult, result);

            bridge.Verify(x => x.EventGetEmg32(eventHandle, sensorIndex), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.EventGetEmg64(eventHandle, sensorIndex), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }

        [Fact]
        public void GetEventOrientation_ValidParameters_ExpectedBridgeCalls()
        {
            // Setup
            var myoHandle = new IntPtr(123);
            var eventHandle = new IntPtr(789);

            var expectedResult = new QuaternionF(10, 20, 30, 40);

            var bridge = new Mock<IMyoDeviceBridge>(MockBehavior.Strict);
            bridge
                .Setup(x => x.EventGetOrientation32(eventHandle, OrientationIndex.W))
                .Returns(expectedResult.W);
            bridge
                .Setup(x => x.EventGetOrientation32(eventHandle, OrientationIndex.X))
                .Returns(expectedResult.X);
            bridge
                .Setup(x => x.EventGetOrientation32(eventHandle, OrientationIndex.Y))
                .Returns(expectedResult.Y);
            bridge
                .Setup(x => x.EventGetOrientation32(eventHandle, OrientationIndex.Z))
                .Returns(expectedResult.Z);
            bridge
                .Setup(x => x.EventGetOrientation64(eventHandle, OrientationIndex.W))
                .Returns(expectedResult.W);
            bridge
                .Setup(x => x.EventGetOrientation64(eventHandle, OrientationIndex.X))
                .Returns(expectedResult.X);
            bridge
                .Setup(x => x.EventGetOrientation64(eventHandle, OrientationIndex.Y))
                .Returns(expectedResult.Y);
            bridge
                .Setup(x => x.EventGetOrientation64(eventHandle, OrientationIndex.Z))
                .Returns(expectedResult.Z);

            var myoErrorHandlerDriver = new Mock<IMyoErrorHandlerDriver>(MockBehavior.Strict);

            var driver = MyoDeviceDriver.Create(
                myoHandle,
                bridge.Object,
                myoErrorHandlerDriver.Object);

            // Execute
            var result = driver.GetEventOrientation(eventHandle);

            // Assert
            Assert.Equal(expectedResult.W, result.W);
            Assert.Equal(expectedResult.X, result.X);
            Assert.Equal(expectedResult.Y, result.Y);
            Assert.Equal(expectedResult.Z, result.Z);

            bridge.Verify(x => x.EventGetOrientation32(eventHandle, OrientationIndex.W), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.EventGetOrientation32(eventHandle, OrientationIndex.X), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.EventGetOrientation32(eventHandle, OrientationIndex.Y), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.EventGetOrientation32(eventHandle, OrientationIndex.Z), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());

            bridge.Verify(x => x.EventGetOrientation64(eventHandle, OrientationIndex.W), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
            bridge.Verify(x => x.EventGetOrientation64(eventHandle, OrientationIndex.X), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
            bridge.Verify(x => x.EventGetOrientation64(eventHandle, OrientationIndex.Y), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
            bridge.Verify(x => x.EventGetOrientation64(eventHandle, OrientationIndex.Z), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }

        [Fact]
        public void GetEventPose_ValidParameters_ExpectedBridgeCalls()
        {
            // Setup
            var expectedResult = Pose.Fist;
            var myoHandle = new IntPtr(123);
            var eventHandle = new IntPtr(789);

            var bridge = new Mock<IMyoDeviceBridge>(MockBehavior.Strict);
            bridge
                .Setup(x => x.EventGetPose32(eventHandle))
                .Returns(expectedResult);
            bridge
                .Setup(x => x.EventGetPose64(eventHandle))
                .Returns(expectedResult);

            var myoErrorHandlerDriver = new Mock<IMyoErrorHandlerDriver>(MockBehavior.Strict);

            var driver = MyoDeviceDriver.Create(
                myoHandle,
                bridge.Object,
                myoErrorHandlerDriver.Object);

            // Execute
            var result = driver.GetEventPose(eventHandle);

            // Assert
            Assert.Equal(expectedResult, result);

            bridge.Verify(x => x.EventGetPose32(eventHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.EventGetPose64(eventHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }

        [Fact]
        public void GetEventRssi_ValidParameters_ExpectedBridgeCalls()
        {
            // Setup
            sbyte expectedResult = 123;
            var myoHandle = new IntPtr(123);
            var eventHandle = new IntPtr(789);

            var bridge = new Mock<IMyoDeviceBridge>(MockBehavior.Strict);
            bridge
                .Setup(x => x.EventGetRssi32(eventHandle))
                .Returns(expectedResult);
            bridge
                .Setup(x => x.EventGetRssi64(eventHandle))
                .Returns(expectedResult);

            var myoErrorHandlerDriver = new Mock<IMyoErrorHandlerDriver>(MockBehavior.Strict);

            var driver = MyoDeviceDriver.Create(
                myoHandle,
                bridge.Object,
                myoErrorHandlerDriver.Object);

            // Execute
            var result = driver.GetEventRssi(eventHandle);

            // Assert
            Assert.Equal(expectedResult, result);

            bridge.Verify(x => x.EventGetRssi32(eventHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.EventGetRssi64(eventHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }

        [Fact]
        public void GetFirmwareVersion_ValidParameters_ExpectedBridgeCalls()
        {
            // Setup
            uint expectedResult = 123;
            var component = VersionComponent.Major;
            var myoHandle = new IntPtr(123);
            var eventHandle = new IntPtr(789);

            var bridge = new Mock<IMyoDeviceBridge>(MockBehavior.Strict);
            bridge
                .Setup(x => x.EventGetFirmwareVersion32(eventHandle, component))
                .Returns(expectedResult);
            bridge
                .Setup(x => x.EventGetFirmwareVersion64(eventHandle, component))
                .Returns(expectedResult);

            var myoErrorHandlerDriver = new Mock<IMyoErrorHandlerDriver>(MockBehavior.Strict);

            var driver = MyoDeviceDriver.Create(
                myoHandle,
                bridge.Object,
                myoErrorHandlerDriver.Object);

            // Execute
            var result = driver.GetFirmwareVersion(eventHandle, component);

            // Assert
            Assert.Equal(expectedResult, result);

            bridge.Verify(x => x.EventGetFirmwareVersion32(eventHandle, component), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.EventGetFirmwareVersion64(eventHandle, component), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }

        [Fact]
        public void GetGyroscope_ValidParameters_ExpectedBridgeCalls()
        {
            // Setup
            var myoHandle = new IntPtr(123);
            var eventHandle = new IntPtr(789);

            var expectedResult = new Vector3F(10, 20, 30);

            var bridge = new Mock<IMyoDeviceBridge>(MockBehavior.Strict);
            bridge
                .Setup(x => x.EventGetGyroscope32(eventHandle, 0))
                .Returns(expectedResult.X);
            bridge
                .Setup(x => x.EventGetGyroscope32(eventHandle, 1))
                .Returns(expectedResult.Y);
            bridge
                .Setup(x => x.EventGetGyroscope32(eventHandle, 2))
                .Returns(expectedResult.Z);
            bridge
                .Setup(x => x.EventGetGyroscope64(eventHandle, 0))
                .Returns(expectedResult.X);
            bridge
                .Setup(x => x.EventGetGyroscope64(eventHandle, 1))
                .Returns(expectedResult.Y);
            bridge
                .Setup(x => x.EventGetGyroscope64(eventHandle, 2))
                .Returns(expectedResult.Z);

            var myoErrorHandlerDriver = new Mock<IMyoErrorHandlerDriver>(MockBehavior.Strict);

            var driver = MyoDeviceDriver.Create(
                myoHandle,
                bridge.Object,
                myoErrorHandlerDriver.Object);

            // Execute
            var result = driver.GetGyroscope(eventHandle);
            
            // Assert
            Assert.Equal(expectedResult.X, result.X);
            Assert.Equal(expectedResult.Y, result.Y);
            Assert.Equal(expectedResult.Z, result.Z);

            bridge.Verify(x => x.EventGetGyroscope32(eventHandle, 0), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.EventGetGyroscope32(eventHandle, 1), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.EventGetGyroscope32(eventHandle, 2), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());

            bridge.Verify(x => x.EventGetGyroscope64(eventHandle, 0), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
            bridge.Verify(x => x.EventGetGyroscope64(eventHandle, 1), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
            bridge.Verify(x => x.EventGetGyroscope64(eventHandle, 2), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());
        }

        [Fact]
        public void Lock_ValidState_ExpectedBridgeCalls()
        {
            // Setup
            var myoHandle = new IntPtr(123);
            var errorHandle = IntPtr.Zero;

            var bridge = new Mock<IMyoDeviceBridge>(MockBehavior.Strict);
            bridge
                .Setup(x => x.Lock32(myoHandle, out errorHandle))
                .Returns(MyoResult.Success);
            bridge
                .Setup(x => x.Lock64(myoHandle, out errorHandle))
                .Returns(MyoResult.Success);

            var myoErrorHandlerDriver = new Mock<IMyoErrorHandlerDriver>(MockBehavior.Strict);
            myoErrorHandlerDriver
                .Setup(x => x.FreeMyoError(errorHandle));

            var driver = MyoDeviceDriver.Create(
                myoHandle,
                bridge.Object,
                myoErrorHandlerDriver.Object);

            // Execute
            driver.Lock();

            // Assert
            bridge.Verify(x => x.Lock32(myoHandle, out errorHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.Lock64(myoHandle, out errorHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());

            myoErrorHandlerDriver.Verify(x => x.FreeMyoError(errorHandle), Times.Once);
        }

        [Fact]
        public void Unlock_ValidParameter_ExpectedBridgeCalls()
        {
            // Setup
            var unlockType = UnlockType.Hold;
            var myoHandle = new IntPtr(123);
            var errorHandle = IntPtr.Zero;

            var bridge = new Mock<IMyoDeviceBridge>(MockBehavior.Strict);
            bridge
                .Setup(x => x.Unlock32(myoHandle, unlockType, out errorHandle))
                .Returns(MyoResult.Success);
            bridge
                .Setup(x => x.Unlock64(myoHandle, unlockType, out errorHandle))
                .Returns(MyoResult.Success);

            var myoErrorHandlerDriver = new Mock<IMyoErrorHandlerDriver>(MockBehavior.Strict);
            myoErrorHandlerDriver
                .Setup(x => x.FreeMyoError(errorHandle));

            var driver = MyoDeviceDriver.Create(
                myoHandle,
                bridge.Object,
                myoErrorHandlerDriver.Object);

            // Execute
            driver.Unlock(unlockType);

            // Assert
            bridge.Verify(x => x.Unlock32(myoHandle, unlockType, out errorHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.Unlock64(myoHandle, unlockType, out errorHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());

            myoErrorHandlerDriver.Verify(x => x.FreeMyoError(errorHandle), Times.Once);
        }

        [Fact]
        public void RequestRssi_ValidParameters_ExpectedBridgeCalls()
        {
            // Setup
            var myoHandle = new IntPtr(123);
            var errorHandle = IntPtr.Zero;
            
            var bridge = new Mock<IMyoDeviceBridge>(MockBehavior.Strict);
            bridge
                .Setup(x => x.RequestRssi32(myoHandle, out errorHandle))
                .Returns(MyoResult.Success);
            bridge
                .Setup(x => x.RequestRssi64(myoHandle, out errorHandle))
                .Returns(MyoResult.Success);

            var myoErrorHandlerDriver = new Mock<IMyoErrorHandlerDriver>(MockBehavior.Strict);
            myoErrorHandlerDriver
                .Setup(x => x.FreeMyoError(errorHandle));

            var driver = MyoDeviceDriver.Create(
                myoHandle,
                bridge.Object,
                myoErrorHandlerDriver.Object);

            // Execute
            driver.RequestRssi();

            // Assert
            bridge.Verify(x => x.RequestRssi32(myoHandle, out errorHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.RequestRssi64(myoHandle, out errorHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());

            myoErrorHandlerDriver.Verify(x => x.FreeMyoError(errorHandle), Times.Once);
        }

        [Fact]
        public void SetEmgStreaming_Enabled_ExpectedBridgeCalls()
        {
            // Setup
            var myoHandle = new IntPtr(123);
            var errorHandle = IntPtr.Zero;

            var bridge = new Mock<IMyoDeviceBridge>(MockBehavior.Strict);
            bridge
                .Setup(x => x.StreamEmg32(myoHandle, StreamEmgType.Enabled, out errorHandle))
                .Returns(MyoResult.Success);
            bridge
                .Setup(x => x.StreamEmg64(myoHandle, StreamEmgType.Enabled, out errorHandle))
                .Returns(MyoResult.Success);

            var myoErrorHandlerDriver = new Mock<IMyoErrorHandlerDriver>(MockBehavior.Strict);
            myoErrorHandlerDriver
                .Setup(x => x.FreeMyoError(errorHandle));

            var driver = MyoDeviceDriver.Create(
                myoHandle,
                bridge.Object,
                myoErrorHandlerDriver.Object);

            // Execute
            driver.SetEmgStreaming(true);

            // Assert
            bridge.Verify(x => x.StreamEmg32(myoHandle, StreamEmgType.Enabled, out errorHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.StreamEmg64(myoHandle, StreamEmgType.Enabled, out errorHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());

            myoErrorHandlerDriver.Verify(x => x.FreeMyoError(errorHandle), Times.Once);
        }

        [Fact]
        public void SetEmgStreaming_Disabled_ExpectedBridgeCalls()
        {
            // Setup
            var myoHandle = new IntPtr(123);
            var errorHandle = IntPtr.Zero;

            var bridge = new Mock<IMyoDeviceBridge>(MockBehavior.Strict);
            bridge
                .Setup(x => x.StreamEmg32(myoHandle, StreamEmgType.Disabled, out errorHandle))
                .Returns(MyoResult.Success);
            bridge
                .Setup(x => x.StreamEmg64(myoHandle, StreamEmgType.Disabled, out errorHandle))
                .Returns(MyoResult.Success);

            var myoErrorHandlerDriver = new Mock<IMyoErrorHandlerDriver>(MockBehavior.Strict);
            myoErrorHandlerDriver
                .Setup(x => x.FreeMyoError(errorHandle));

            var driver = MyoDeviceDriver.Create(
                myoHandle,
                bridge.Object,
                myoErrorHandlerDriver.Object);

            // Execute
            driver.SetEmgStreaming(false);
            
            // Assert
            bridge.Verify(x => x.StreamEmg32(myoHandle, StreamEmgType.Disabled, out errorHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.StreamEmg64(myoHandle, StreamEmgType.Disabled, out errorHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());

            myoErrorHandlerDriver.Verify(x => x.FreeMyoError(errorHandle), Times.Once);
        }

        [Fact]
        public void Vibrate_Medium_ExpectedBridgeCalls()
        {
            // Setup
            var vibration = VibrationType.Medium;
            var myoHandle = new IntPtr(123);
            var errorHandle = IntPtr.Zero;

            var bridge = new Mock<IMyoDeviceBridge>(MockBehavior.Strict);
            bridge
                .Setup(x => x.Vibrate32(myoHandle, vibration, out errorHandle))
                .Returns(MyoResult.Success);
            bridge
                .Setup(x => x.Vibrate64(myoHandle, vibration, out errorHandle))
                .Returns(MyoResult.Success);

            var myoErrorHandlerDriver = new Mock<IMyoErrorHandlerDriver>(MockBehavior.Strict);
            myoErrorHandlerDriver
                .Setup(x => x.FreeMyoError(errorHandle));

            var driver = MyoDeviceDriver.Create(
                myoHandle,
                bridge.Object,
                myoErrorHandlerDriver.Object);

            // Execute
            driver.Vibrate(VibrationType.Medium);

            // Assert
            bridge.Verify(x => x.Vibrate32(myoHandle, vibration, out errorHandle), PlatformInvocation.Running32Bit ? Times.Once() : Times.Never());
            bridge.Verify(x => x.Vibrate64(myoHandle, vibration, out errorHandle), PlatformInvocation.Running32Bit ? Times.Never() : Times.Once());

            myoErrorHandlerDriver.Verify(x => x.FreeMyoError(errorHandle), Times.Once);
        }
        #endregion
    }
}
