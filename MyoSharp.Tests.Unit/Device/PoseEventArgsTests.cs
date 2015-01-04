using System;

using Xunit;

using Moq;

using MyoSharp.Device;
using MyoSharp.Poses;

namespace MyoSharp.Tests.Unit.Device
{
    public class PoseEventArgsTests
    {
        #region Methods
        [Fact]
        public void Constructor_NullMyo_ThrowsArgumentNullException()
        {
            // Setup

            // Execute
            Assert.ThrowsDelegate method = () => new PoseEventArgs(
                null,
                DateTime.UtcNow,
                Pose.Unknown);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(method);
            Assert.Equal("myo", exception.ParamName);
        }

        [Fact]
        public void Constructor_ValidArguments_DoesNotThrow()
        {
            // Setup
            var myo = new Mock<IMyo>();

            // Execute
            Assert.ThrowsDelegate method = () => new PoseEventArgs(
                myo.Object,
                DateTime.UtcNow,
                Pose.Unknown);

            // Assert
            Assert.DoesNotThrow(method);
        }
        
        [Fact]
        public void GetMyo_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var myo = new Mock<IMyo>();

            var args = new PoseEventArgs(
                myo.Object,
                DateTime.UtcNow,
                Pose.Unknown);

            // Execute
            var result = args.Myo;

            // Assert
            Assert.Equal(myo.Object, result);
        }

        [Fact]
        public void GetTimestamp_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var timestamp = DateTime.UtcNow;

            var args = new PoseEventArgs(
                new Mock<IMyo>().Object,
                timestamp,
                Pose.Unknown);

            // Execute
            var result = args.Timestamp;

            // Assert
            Assert.Equal(timestamp, result);
        }

        [Fact]
        public void GetPose_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var pose = Pose.Fist;

            var args = new PoseEventArgs(
                new Mock<IMyo>().Object,
                DateTime.UtcNow,
                pose);

            // Execute
            var result = args.Pose;

            // Assert
            Assert.Equal(pose, result);
        }
        #endregion
    }
}
