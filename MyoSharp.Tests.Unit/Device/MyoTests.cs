using System;

using Xunit;

using Moq;

using MyoSharp.Device;
using MyoSharp.Communication;
using MyoSharp.Poses;
using MyoSharp.Math;

namespace MyoSharp.Tests.Unit.Device
{
    public class MyoTests
    {
        #region Methods
        [Fact]
        public void Create_NullChannelListener_ThrowsArgumentNullException()
        {
            // Setup
            var myoDeviceDriver = new Mock<IMyoDeviceDriver>();

            // Execute
            Assert.ThrowsDelegate method = () => Myo.Create(
                null,
                myoDeviceDriver.Object);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(method);
            Assert.Equal("channelListener", exception.ParamName);
        }

        [Fact]
        public void Create_NullMyoDeviceDriver_ThrowsArgumentNullException()
        {
            // Setup
            var channelListener = new Mock<IChannelListener>();

            // Execute
            Assert.ThrowsDelegate method = () => Myo.Create(
                channelListener.Object,
                null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(method);
            Assert.Equal("myoDeviceDriver", exception.ParamName);
        }

        [Fact]
        public void Create_ValidArguments_NewInstance()
        {
            // Setup
            var channelListener = new Mock<IChannelListener>();
            var myoDeviceDriver = new Mock<IMyoDeviceDriver>();

            // Execute
            var result = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Vibrate_Long_CallsDeviceDriver()
        {
            // Setup
            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);
            
            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .Setup(x => x.Vibrate(VibrationType.Long));

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            // Execute
            myo.Vibrate(VibrationType.Long);

            // Assert
            myoDeviceDriver.Verify(x => x.Vibrate(VibrationType.Long), Times.Once);
        }

        [Fact]
        public void RequestRssi_ValidState_CallsDeviceDriver()
        {
            // Setup
            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);
            
            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .Setup(x => x.RequestRssi());

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            // Execute
            myo.RequestRssi();

            // Assert
            myoDeviceDriver.Verify(x => x.RequestRssi(), Times.Once);
        }

        [Fact]
        public void Unlock_Hold_CallsDeviceDriver()
        {
            // Setup
            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .Setup(x => x.Unlock(UnlockType.Hold));

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            // Execute
            myo.Unlock(UnlockType.Hold);

            // Assert
            myoDeviceDriver.Verify(x => x.Unlock(UnlockType.Hold), Times.Once);
        }

        [Fact]
        public void Lock_ValidState_CallsDeviceDriver()
        {
            // Setup
            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .Setup(x => x.Lock());

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            // Execute
            myo.Lock();

            // Assert
            myoDeviceDriver.Verify(x => x.Lock(), Times.Once);
        }

        [Fact]
        public void SetEmgStreaming_Enabled_CallsDeviceDriver()
        {
            // Setup
            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .Setup(x => x.SetEmgStreaming(true));

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            // Execute
            myo.SetEmgStreaming(true);

            // Assert
            myoDeviceDriver.Verify(x => x.SetEmgStreaming(true), Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_DifferentMyoHandle_DoesNotTriggerEvent()
        {
            // Setup
            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(789));

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);
            
            var gotPoseChangedEvent = false;
            myo.PoseChanged += (_, __) => gotPoseChangedEvent = true;

            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(123), 
                MyoEventType.Pose, 
                DateTime.UtcNow);

            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.False(
                gotPoseChangedEvent,
                "Not expecting to get the pose changed event.");

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_Disposed_DoesNotTriggerEvent()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(123),
                MyoEventType.Pose,
                DateTime.UtcNow);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(routeEventArgs.MyoHandle);
            myoDeviceDriver
                .Setup(x => x.GetEventPose(routeEventArgs.Event))
                .Returns(Pose.Fist);

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            var gotPoseChangedEvent = false;
            myo.PoseChanged += (_, __) => gotPoseChangedEvent = true;

            myo.Dispose();

            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.False(
                gotPoseChangedEvent,
                "Not expecting to get the pose changed event.");

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Never);
        }

        [Fact]
        public void ChannelEventReceived_PoseChanged_TriggersPoseChangedEvent()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.Pose,
                DateTime.UtcNow);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));
            myoDeviceDriver
                .Setup(x => x.GetEventPose(routeEventArgs.Event))
                .Returns(Pose.Fist);

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            PoseEventArgs actualEventArgs = null;
            object actualSender = null;
            myo.PoseChanged += (sender, args) =>
            {
                actualSender = sender;
                actualEventArgs = args;
            };

            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.Equal(myo, actualSender);
            Assert.Equal(myo, actualEventArgs.Myo);
            Assert.Equal(routeEventArgs.Timestamp, actualEventArgs.Timestamp);
            Assert.Equal(Pose.Fist, actualEventArgs.Pose);

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_Locked_TriggersLockedEvent()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.Locked,
                DateTime.UtcNow);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            MyoEventArgs actualEventArgs = null;
            object actualSender = null;
            myo.Locked += (sender, args) =>
            {
                actualSender = sender;
                actualEventArgs = args;
            };

            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.Equal(myo, actualSender);
            Assert.Equal(myo, actualEventArgs.Myo);
            Assert.Equal(routeEventArgs.Timestamp, actualEventArgs.Timestamp);

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_Unlocked_TriggersUnlockedEvent()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.Unlocked,
                DateTime.UtcNow);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            MyoEventArgs actualEventArgs = null;
            object actualSender = null;
            myo.Unlocked += (sender, args) =>
            {
                actualSender = sender;
                actualEventArgs = args;
            };

            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.Equal(myo, actualSender);
            Assert.Equal(myo, actualEventArgs.Myo);
            Assert.Equal(routeEventArgs.Timestamp, actualEventArgs.Timestamp);

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_Connected_TriggersConnectedEvent()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.Connected,
                DateTime.UtcNow);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            MyoEventArgs actualEventArgs = null;
            object actualSender = null;
            myo.Connected += (sender, args) =>
            {
                actualSender = sender;
                actualEventArgs = args;
            };

            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.Equal(myo, actualSender);
            Assert.Equal(myo, actualEventArgs.Myo);
            Assert.Equal(routeEventArgs.Timestamp, actualEventArgs.Timestamp);

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_Connected_IsConnectedTrue()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.Connected,
                DateTime.UtcNow);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);
            
            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.True(
                myo.IsConnected,
                "Expecting the Myo to be connected.");

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_Disconnected_TriggersDisconnectedEvent()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.Disconnected,
                DateTime.UtcNow);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            MyoEventArgs actualEventArgs = null;
            object actualSender = null;
            myo.Disconnected += (sender, args) =>
            {
                actualSender = sender;
                actualEventArgs = args;
            };

            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.Equal(myo, actualSender);
            Assert.Equal(myo, actualEventArgs.Myo);
            Assert.Equal(routeEventArgs.Timestamp, actualEventArgs.Timestamp);

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_Disconnected_IsConnectedFalse()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.Disconnected,
                DateTime.UtcNow);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.False(
                myo.IsConnected,
                "Expecting the Myo to be disconnected.");

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_ArmRecognized_TriggersArmRecognizedEvent()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.ArmRecognized,
                DateTime.UtcNow);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));
            myoDeviceDriver
                .Setup(x => x.GetArm(routeEventArgs.Event))
                .Returns(Arm.Left);
            myoDeviceDriver
                .Setup(x => x.GetEventDirectionX(routeEventArgs.Event))
                .Returns(XDirection.TowardElbow);

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            ArmRecognizedEventArgs actualEventArgs = null;
            object actualSender = null;
            myo.ArmRecognized += (sender, args) =>
            {
                actualSender = sender;
                actualEventArgs = args;
            };

            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.Equal(myo, actualSender);
            Assert.Equal(myo, actualEventArgs.Myo);
            Assert.Equal(routeEventArgs.Timestamp, actualEventArgs.Timestamp);
            Assert.Equal(Arm.Left, actualEventArgs.Arm);
            Assert.Equal(XDirection.TowardElbow, actualEventArgs.XDirection);

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
            myoDeviceDriver.Verify(x => x.GetArm(routeEventArgs.Event), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventDirectionX(routeEventArgs.Event), Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_ArmRecognized_ArmIsLeft()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.ArmRecognized,
                DateTime.UtcNow);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));
            myoDeviceDriver
                .Setup(x => x.GetArm(routeEventArgs.Event))
                .Returns(Arm.Left);
            myoDeviceDriver
                .Setup(x => x.GetEventDirectionX(routeEventArgs.Event))
                .Returns(XDirection.TowardElbow);

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.Equal(Arm.Left, myo.Arm);

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
            myoDeviceDriver.Verify(x => x.GetArm(routeEventArgs.Event), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventDirectionX(routeEventArgs.Event), Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_ArmRecognized_XDirectionOnArmIsTowardElbow()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.ArmRecognized,
                DateTime.UtcNow);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));
            myoDeviceDriver
                .Setup(x => x.GetArm(routeEventArgs.Event))
                .Returns(Arm.Left);
            myoDeviceDriver
                .Setup(x => x.GetEventDirectionX(routeEventArgs.Event))
                .Returns(XDirection.TowardElbow);

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.Equal(XDirection.TowardElbow, myo.XDirectionOnArm);

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
            myoDeviceDriver.Verify(x => x.GetArm(routeEventArgs.Event), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventDirectionX(routeEventArgs.Event), Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_ArmLost_TriggersArmLostEvent()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.ArmLost,
                DateTime.UtcNow);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            MyoEventArgs actualEventArgs = null;
            object actualSender = null;
            myo.ArmLost += (sender, args) =>
            {
                actualSender = sender;
                actualEventArgs = args;
            };

            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.Equal(myo, actualSender);
            Assert.Equal(myo, actualEventArgs.Myo);
            Assert.Equal(routeEventArgs.Timestamp, actualEventArgs.Timestamp);

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_ArmLost_ArmIsUnknown()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.ArmLost,
                DateTime.UtcNow);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.Equal(Arm.Unknown, myo.Arm);

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_ArmLost_XDirectionOnArmIsUnknown()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.ArmLost,
                DateTime.UtcNow);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.Equal(XDirection.Unknown, myo.XDirectionOnArm);

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_Orientation_TriggersOrientationDataAcquiredEvent()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.Orientation,
                DateTime.UtcNow);

            var orientation = new QuaternionF(10, 20, 30, 40);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));
            myoDeviceDriver
                .Setup(x => x.GetEventOrientation(routeEventArgs.Event))
                .Returns(orientation);
            myoDeviceDriver
                .Setup(x => x.GetEventAccelerometer(routeEventArgs.Event))
                .Returns(new Vector3F());
            myoDeviceDriver
                .Setup(x => x.GetGyroscope(routeEventArgs.Event))
                .Returns(new Vector3F());

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            OrientationDataEventArgs actualEventArgs = null;
            object actualSender = null;
            myo.OrientationDataAcquired += (sender, args) =>
            {
                actualSender = sender;
                actualEventArgs = args;
            };

            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.Equal(myo, actualSender);
            Assert.Equal(myo, actualEventArgs.Myo);
            Assert.Equal(routeEventArgs.Timestamp, actualEventArgs.Timestamp);
            Assert.Equal(orientation, actualEventArgs.Orientation);
            Assert.InRange(actualEventArgs.Roll, 2.03404385580104d, 2.03404385580106d);
            Assert.Equal(double.NaN, actualEventArgs.Pitch);
            Assert.InRange(actualEventArgs.Yaw, 2.31898255934001d, 2.31898255934003d);

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventOrientation(routeEventArgs.Event), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventAccelerometer(routeEventArgs.Event), Times.Once);
            myoDeviceDriver.Verify(x => x.GetGyroscope(routeEventArgs.Event), Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_Orientation_TriggersAccelerationDataAcquiredEvent()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.Orientation,
                DateTime.UtcNow);

            var acceleration = new Vector3F(10, 20, 30);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));
            myoDeviceDriver
                .Setup(x => x.GetEventOrientation(routeEventArgs.Event))
                .Returns(new QuaternionF());
            myoDeviceDriver
                .Setup(x => x.GetEventAccelerometer(routeEventArgs.Event))
                .Returns(acceleration);
            myoDeviceDriver
                .Setup(x => x.GetGyroscope(routeEventArgs.Event))
                .Returns(new Vector3F());

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            AccelerometerDataEventArgs actualEventArgs = null;
            object actualSender = null;
            myo.AccelerometerDataAcquired += (sender, args) =>
            {
                actualSender = sender;
                actualEventArgs = args;
            };

            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.Equal(myo, actualSender);
            Assert.Equal(myo, actualEventArgs.Myo);
            Assert.Equal(routeEventArgs.Timestamp, actualEventArgs.Timestamp);
            Assert.Equal(acceleration, actualEventArgs.Accelerometer);;

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventOrientation(routeEventArgs.Event), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventAccelerometer(routeEventArgs.Event), Times.Once);
            myoDeviceDriver.Verify(x => x.GetGyroscope(routeEventArgs.Event), Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_Orientation_TriggersGyroscopeDataAcquiredEvent()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.Orientation,
                DateTime.UtcNow);

            var gyroscope = new Vector3F(10, 20, 30);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));
            myoDeviceDriver
                .Setup(x => x.GetEventOrientation(routeEventArgs.Event))
                .Returns(new QuaternionF());
            myoDeviceDriver
                .Setup(x => x.GetEventAccelerometer(routeEventArgs.Event))
                .Returns(new Vector3F());
            myoDeviceDriver
                .Setup(x => x.GetGyroscope(routeEventArgs.Event))
                .Returns(gyroscope);

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            GyroscopeDataEventArgs actualEventArgs = null;
            object actualSender = null;
            myo.GyroscopeDataAcquired += (sender, args) =>
            {
                actualSender = sender;
                actualEventArgs = args;
            };

            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.Equal(myo, actualSender);
            Assert.Equal(myo, actualEventArgs.Myo);
            Assert.Equal(routeEventArgs.Timestamp, actualEventArgs.Timestamp);
            Assert.Equal(gyroscope, actualEventArgs.Gyroscope); ;

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventOrientation(routeEventArgs.Event), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventAccelerometer(routeEventArgs.Event), Times.Once);
            myoDeviceDriver.Verify(x => x.GetGyroscope(routeEventArgs.Event), Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_Orientation_OrientationIsSet()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.Orientation,
                DateTime.UtcNow);

            var orientation = new QuaternionF(10, 20, 30, 40);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));
            myoDeviceDriver
                .Setup(x => x.GetEventOrientation(routeEventArgs.Event))
                .Returns(orientation);
            myoDeviceDriver
                .Setup(x => x.GetEventAccelerometer(routeEventArgs.Event))
                .Returns(new Vector3F());
            myoDeviceDriver
                .Setup(x => x.GetGyroscope(routeEventArgs.Event))
                .Returns(new Vector3F());

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.Equal(orientation, myo.Orientation);

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventOrientation(routeEventArgs.Event), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventAccelerometer(routeEventArgs.Event), Times.Once);
            myoDeviceDriver.Verify(x => x.GetGyroscope(routeEventArgs.Event), Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_Orientation_AccelerometerIsSet()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.Orientation,
                DateTime.UtcNow);

            var acceleration = new Vector3F(10, 20, 30);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));
            myoDeviceDriver
                .Setup(x => x.GetEventOrientation(routeEventArgs.Event))
                .Returns(new QuaternionF());
            myoDeviceDriver
                .Setup(x => x.GetEventAccelerometer(routeEventArgs.Event))
                .Returns(acceleration);
            myoDeviceDriver
                .Setup(x => x.GetGyroscope(routeEventArgs.Event))
                .Returns(new Vector3F());

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);
            
            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.Equal(acceleration, myo.Accelerometer);

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventOrientation(routeEventArgs.Event), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventAccelerometer(routeEventArgs.Event), Times.Once);
            myoDeviceDriver.Verify(x => x.GetGyroscope(routeEventArgs.Event), Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_Orientation_GyroscopeIsSet()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.Orientation,
                DateTime.UtcNow);

            var gyroscope = new Vector3F(10, 20, 30);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));
            myoDeviceDriver
                .Setup(x => x.GetEventOrientation(routeEventArgs.Event))
                .Returns(new QuaternionF());
            myoDeviceDriver
                .Setup(x => x.GetEventAccelerometer(routeEventArgs.Event))
                .Returns(new Vector3F());
            myoDeviceDriver
                .Setup(x => x.GetGyroscope(routeEventArgs.Event))
                .Returns(gyroscope);

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.Equal(gyroscope, myo.Gyroscope);

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventOrientation(routeEventArgs.Event), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventAccelerometer(routeEventArgs.Event), Times.Once);
            myoDeviceDriver.Verify(x => x.GetGyroscope(routeEventArgs.Event), Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_Rssi_TriggersRssiEvent()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.Rssi,
                DateTime.UtcNow);
            
            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));
            myoDeviceDriver
                .Setup(x => x.GetEventRssi(routeEventArgs.Event))
                .Returns(123);

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            RssiEventArgs actualEventArgs = null;
            object actualSender = null;
            myo.Rssi += (sender, args) =>
            {
                actualSender = sender;
                actualEventArgs = args;
            };

            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.Equal(myo, actualSender);
            Assert.Equal(myo, actualEventArgs.Myo);
            Assert.Equal(routeEventArgs.Timestamp, actualEventArgs.Timestamp);
            Assert.Equal(123, actualEventArgs.Rssi);

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventRssi(routeEventArgs.Event), Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_Emg_TriggersEmgDataAcquiredEvent()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.Emg,
                DateTime.UtcNow);

            var gyroscope = new Vector3F(10, 20, 30);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));

            for (int i = 0; i < 8; i++)
            {
                myoDeviceDriver
                    .Setup(x => x.GetEventEmg(routeEventArgs.Event, i))
                    .Returns((sbyte)(i * 10));
            }

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);

            EmgDataEventArgs actualEventArgs = null;
            object actualSender = null;
            myo.EmgDataAcquired += (sender, args) =>
            {
                actualSender = sender;
                actualEventArgs = args;
            };

            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.Equal(myo, actualSender);
            Assert.Equal(myo, actualEventArgs.Myo);
            Assert.Equal(routeEventArgs.Timestamp, actualEventArgs.Timestamp);
            Assert.NotNull(actualEventArgs.EmgData);
            Assert.Equal(0, actualEventArgs.EmgData.GetDataForSensor(0));
            Assert.Equal(10, actualEventArgs.EmgData.GetDataForSensor(1));
            Assert.Equal(20, actualEventArgs.EmgData.GetDataForSensor(2));
            Assert.Equal(30, actualEventArgs.EmgData.GetDataForSensor(3));
            Assert.Equal(40, actualEventArgs.EmgData.GetDataForSensor(4));
            Assert.Equal(50, actualEventArgs.EmgData.GetDataForSensor(5));
            Assert.Equal(60, actualEventArgs.EmgData.GetDataForSensor(6));
            Assert.Equal(70, actualEventArgs.EmgData.GetDataForSensor(7));

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventEmg(routeEventArgs.Event, 0), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventEmg(routeEventArgs.Event, 1), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventEmg(routeEventArgs.Event, 2), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventEmg(routeEventArgs.Event, 3), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventEmg(routeEventArgs.Event, 4), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventEmg(routeEventArgs.Event, 5), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventEmg(routeEventArgs.Event, 6), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventEmg(routeEventArgs.Event, 7), Times.Once);
        }

        [Fact]
        public void ChannelEventReceived_Emg_EmgDataIsSet()
        {
            // Setup
            var routeEventArgs = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(789),
                MyoEventType.Emg,
                DateTime.UtcNow);

            var gyroscope = new Vector3F(10, 20, 30);

            var channelListener = new Mock<IChannelListener>(MockBehavior.Strict);

            var myoDeviceDriver = new Mock<IMyoDeviceDriver>(MockBehavior.Strict);
            myoDeviceDriver
                .SetupGet(x => x.Handle)
                .Returns(new IntPtr(123));

            for (int i = 0; i < 8; i++)
            {
                myoDeviceDriver
                    .Setup(x => x.GetEventEmg(routeEventArgs.Event, i))
                    .Returns((sbyte)(i * 10));
            }

            var myo = Myo.Create(
                channelListener.Object,
                myoDeviceDriver.Object);
            
            // Execute
            channelListener.Raise(
                x => x.EventReceived += null,
                routeEventArgs);

            // Assert
            Assert.Equal(0, myo.EmgData.GetDataForSensor(0));
            Assert.Equal(10, myo.EmgData.GetDataForSensor(1));
            Assert.Equal(20, myo.EmgData.GetDataForSensor(2));
            Assert.Equal(30, myo.EmgData.GetDataForSensor(3));
            Assert.Equal(40, myo.EmgData.GetDataForSensor(4));
            Assert.Equal(50, myo.EmgData.GetDataForSensor(5));
            Assert.Equal(60, myo.EmgData.GetDataForSensor(6));
            Assert.Equal(70, myo.EmgData.GetDataForSensor(7));

            myoDeviceDriver.VerifyGet(x => x.Handle, Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventEmg(routeEventArgs.Event, 0), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventEmg(routeEventArgs.Event, 1), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventEmg(routeEventArgs.Event, 2), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventEmg(routeEventArgs.Event, 3), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventEmg(routeEventArgs.Event, 4), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventEmg(routeEventArgs.Event, 5), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventEmg(routeEventArgs.Event, 6), Times.Once);
            myoDeviceDriver.Verify(x => x.GetEventEmg(routeEventArgs.Event, 7), Times.Once);
        }
        #endregion
    }
}
