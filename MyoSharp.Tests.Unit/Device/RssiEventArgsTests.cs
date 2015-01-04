using System;

using Xunit;

using Moq;

using MyoSharp.Device;

namespace MyoSharp.Tests.Unit.Device
{
    public class RssiEventArgsTests
    {
        #region Methods
        [Fact]
        public void Constructor_NullMyo_ThrowsArgumentNullException()
        {
            // Setup

            // Execute
            Assert.ThrowsDelegate method = () => new RssiEventArgs(
                null,
                DateTime.UtcNow,
                0);

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
            Assert.ThrowsDelegate method = () => new RssiEventArgs(
                myo.Object,
                DateTime.UtcNow,
                0);

            // Assert
            Assert.DoesNotThrow(method);
        }
        
        [Fact]
        public void GetMyo_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var myo = new Mock<IMyo>();

            var args = new RssiEventArgs(
                myo.Object,
                DateTime.UtcNow,
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

            var args = new RssiEventArgs(
                new Mock<IMyo>().Object,
                timestamp,
                0);

            // Execute
            var result = args.Timestamp;

            // Assert
            Assert.Equal(timestamp, result);
        }

        [Fact]
        public void GetRssi_ValidState_EqualsConstructorParameter()
        {
            // Setup
            sbyte rssi = 123;

            var args = new RssiEventArgs(
                new Mock<IMyo>().Object,
                DateTime.UtcNow,
                rssi);

            // Execute
            var result = args.Rssi;

            // Assert
            Assert.Equal(rssi, result);
        }
        #endregion
    }
}
