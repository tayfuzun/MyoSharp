using System;

using Xunit;

using Moq;

using MyoSharp.Device;
using MyoSharp.Math;

namespace MyoSharp.Tests.Unit.Device
{
    public class AccelerometerDataEventArgsTests
    {
        #region Methods
        [Fact]
        public void Constructor_NullMyo_ThrowsArgumentNullException()
        {
            // Setup

            // Execute
            Assert.ThrowsDelegate method = () => new AccelerometerDataEventArgs(
                null,
                DateTime.UtcNow,
                new Vector3F());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(method);
            Assert.Equal("myo", exception.ParamName);
        }

        [Fact]
        public void Constructor_NullAccelerometerData_ThrowsArgumentNullException()
        {
            // Setup
            var myo = new Mock<IMyo>();

            // Execute
            Assert.ThrowsDelegate method = () => new AccelerometerDataEventArgs(
                myo.Object,
                DateTime.UtcNow,
                null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(method);
            Assert.Equal("accelerometerData", exception.ParamName);
        }

        [Fact]
        public void Constructor_ValidArguments_DoesNotThrow()
        {
            // Setup
            var myo = new Mock<IMyo>();

            // Execute
            Assert.ThrowsDelegate method = () => new AccelerometerDataEventArgs(
                myo.Object,
                DateTime.UtcNow,
                new Vector3F());

            // Assert
            Assert.DoesNotThrow(method);
        }
        
        [Fact]
        public void GetMyo_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var myo = new Mock<IMyo>();
            
            var args = new AccelerometerDataEventArgs(
                myo.Object,
                DateTime.UtcNow,
                new Vector3F());

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
            
            var args = new AccelerometerDataEventArgs(
                new Mock<IMyo>().Object,
                timestamp,
                new Vector3F());

            // Execute
            var result = args.Timestamp;

            // Assert
            Assert.Equal(timestamp, result);
        }

        [Fact]
        public void GetAccelerometer_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var accelerometerData = new Vector3F(1, 2, 3);
            
            var args = new AccelerometerDataEventArgs(
                new Mock<IMyo>().Object,
                DateTime.UtcNow,
                accelerometerData);

            // Execute
            var result = args.Accelerometer;

            // Assert
            Assert.Equal(accelerometerData, result);
        }
        #endregion
    }
}
