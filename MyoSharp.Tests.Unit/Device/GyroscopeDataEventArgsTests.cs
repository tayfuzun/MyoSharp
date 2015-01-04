using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Xunit;

using Moq;

using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.Math;

namespace MyoSharp.Tests.Unit.Device
{
    public class GyroscopeDataEventArgsTests
    {
        #region Methods
        [Fact]
        public void Constructor_NullMyo_ThrowsArgumentNullException()
        {
            // Setup

            // Execute
            Assert.ThrowsDelegate method = () => new GyroscopeDataEventArgs(
                null,
                DateTime.UtcNow,
                new Vector3F());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(method);
            Assert.Equal("myo", exception.ParamName);
        }

        [Fact]
        public void Constructor_NullGyroscopeData_ThrowsArgumentNullException()
        {
            // Setup
            var myo = new Mock<IMyo>();

            // Execute
            Assert.ThrowsDelegate method = () => new GyroscopeDataEventArgs(
                myo.Object,
                DateTime.UtcNow,
                null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(method);
            Assert.Equal("gyroscopeData", exception.ParamName);
        }

        [Fact]
        public void Constructor_ValidArguments_DoesNotThrow()
        {
            // Setup
            var myo = new Mock<IMyo>();

            // Execute
            Assert.ThrowsDelegate method = () => new GyroscopeDataEventArgs(
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
            
            var args = new GyroscopeDataEventArgs(
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
            
            var args = new GyroscopeDataEventArgs(
                new Mock<IMyo>().Object,
                timestamp,
                new Vector3F());

            // Execute
            var result = args.Timestamp;

            // Assert
            Assert.Equal(timestamp, result);
        }

        [Fact]
        public void GetGyroscope_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var gyroscopeData = new Vector3F(1, 2, 3);
            
            var args = new GyroscopeDataEventArgs(
                new Mock<IMyo>().Object,
                DateTime.UtcNow,
                gyroscopeData);

            // Execute
            var result = args.Gyroscope;

            // Assert
            Assert.Equal(gyroscopeData, result);
        }
        #endregion
    }
}
