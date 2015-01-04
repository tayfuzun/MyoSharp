using System;

using Xunit;

using Moq;

using MyoSharp.Poses;
using MyoSharp.Device;

namespace MyoSharp.Tests.Unit.Poses
{
    public class HeldPoseTests
    {
        #region Methods
        [Fact]
        public void Create_NullMyo_ThrowsArgumentNullException()
        {
            // Setup
            var pose = Pose.Fist;

            // Execute
            Assert.ThrowsDelegate method = () => HeldPose.Create(
                null,
                pose);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(method);
            Assert.Equal("myo", exception.ParamName);
        }

        [Fact]
        public void Create_NoPose_ThrowsArgumentException()
        {
            // Setup
            var myo = new Mock<IMyoEventGenerator>(MockBehavior.Strict);

            // Execute
            Assert.ThrowsDelegate method = () => HeldPose.Create(myo.Object);

            // Assert
            var exception = Assert.Throws<ArgumentException>(method);
            Assert.Equal("targetPoses", exception.ParamName);
        }

        [Fact]
        public void Create_InvalidInterval_ThrowsArgumentOutOfRangeException()
        {
            // Setup
            var myo = new Mock<IMyoEventGenerator>(MockBehavior.Strict);
            var pose = Pose.Fist;

            // Execute
            Assert.ThrowsDelegate method = () => HeldPose.Create(
                myo.Object,
                TimeSpan.FromSeconds(-1),
                pose);

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(method);
            Assert.Equal("interval", exception.ParamName);
        }

        [Fact]
        public void Create_SinglePoseParams_NewInstance()
        {
            // Setup
            var myo = new Mock<IMyoEventGenerator>(MockBehavior.Strict);
            var pose = Pose.Fist;

            // Execute
            var result = HeldPose.Create(
                myo.Object,
                pose);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Create_SinglePoseEnumerable_NewInstance()
        {
            // Setup
            var myo = new Mock<IMyoEventGenerator>(MockBehavior.Strict);
            var poses = new Pose[] { Pose.Fist };

            // Execute
            var result = HeldPose.Create(
                myo.Object,
                poses);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Create_MultiplePoseEnumerable_NewInstance()
        {
            // Setup
            var myo = new Mock<IMyoEventGenerator>(MockBehavior.Strict);
            var poses = new Pose[] { Pose.Fist, Pose.FingersSpread, Pose.Rest };

            // Execute
            var result = HeldPose.Create(
                myo.Object,
                poses);

            // Assert
            Assert.NotNull(result);
        }
        #endregion
    }
}
