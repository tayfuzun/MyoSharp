using System;

using Xunit;

using Moq;

using MyoSharp.Device;
using MyoSharp.Communication;
using MyoSharp.Poses;

namespace MyoSharp.Tests.Unit.Communication
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
        #endregion
    }
}
