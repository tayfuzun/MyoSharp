using System;

using Xunit;

using Moq;

using MyoSharp.Device;
using MyoSharp.Math;

namespace MyoSharp.Tests.Unit.Device
{
    public class OrientationDataEventArgsTests
    {
        #region Methods
        [Fact]
        public void Constructor_NullMyo_ThrowsArgumentNullException()
        {
            // Setup

            // Execute
            Assert.ThrowsDelegate method = () => new OrientationDataEventArgs(
                null,
                DateTime.UtcNow,
                new QuaternionF(),
                0,
                0,
                0);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(method);
            Assert.Equal("myo", exception.ParamName);
        }

        [Fact]
        public void Constructor_NullOrientationData_ThrowsArgumentNullException()
        {
            // Setup
            var myo = new Mock<IMyo>();

            // Execute
            Assert.ThrowsDelegate method = () => new OrientationDataEventArgs(
                myo.Object,
                DateTime.UtcNow,
                null,
                0,
                0,
                0);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(method);
            Assert.Equal("orientationData", exception.ParamName);
        }

        [Fact]
        public void Constructor_ValidArguments_DoesNotThrow()
        {
            // Setup
            var myo = new Mock<IMyo>();

            // Execute
            Assert.ThrowsDelegate method = () => new OrientationDataEventArgs(
                myo.Object,
                DateTime.UtcNow,
                new QuaternionF(),
                0,
                0,
                0);

            // Assert
            Assert.DoesNotThrow(method);
        }
        
        [Fact]
        public void GetMyo_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var myo = new Mock<IMyo>();
            
            var args = new OrientationDataEventArgs(
                myo.Object,
                DateTime.UtcNow,
                new QuaternionF(),
                0,
                0,
                0);

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
            
            var args = new OrientationDataEventArgs(
                new Mock<IMyo>().Object,
                timestamp,
                new QuaternionF(),
                0,
                0,
                0);

            // Execute
            var result = args.Timestamp;

            // Assert
            Assert.Equal(timestamp, result);
        }

        [Fact]
        public void GetOrientation_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var orientationData = new QuaternionF(1, 2, 3, 4);

            var args = new OrientationDataEventArgs(
                new Mock<IMyo>().Object,
                DateTime.UtcNow,
                orientationData,
                0,
                0,
                0);

            // Execute
            var result = args.Orientation;

            // Assert
            Assert.Equal(orientationData, result);
        }

        [Fact]
        public void GetRoll_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var roll = 1;

            var args = new OrientationDataEventArgs(
                new Mock<IMyo>().Object,
                DateTime.UtcNow,
                new QuaternionF(),
                roll,
                0,
                0);

            // Execute
            var result = args.Roll;

            // Assert
            Assert.Equal(roll, result);
        }

        [Fact]
        public void GetPitch_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var pitch = 1;

            var args = new OrientationDataEventArgs(
                new Mock<IMyo>().Object,
                DateTime.UtcNow,
                new QuaternionF(),
                0,
                pitch,
                0);

            // Execute
            var result = args.Pitch;

            // Assert
            Assert.Equal(pitch, result);
        }

        [Fact]
        public void GetYaw_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var yaw = 1;

            var args = new OrientationDataEventArgs(
                new Mock<IMyo>().Object,
                DateTime.UtcNow,
                new QuaternionF(),
                0,
                0,
                yaw);

            // Execute
            var result = args.Yaw;

            // Assert
            Assert.Equal(yaw, result);
        }
        #endregion
    }
}
