using System;

using Xunit;

using Moq;

using MyoSharp.Device;

namespace MyoSharp.Tests.Unit.Device
{
    public class MyoEventArgsTests
    {
        #region Methods
        [Fact]
        public void Constructor_NullMyo_ThrowsArgumentNullException()
        {
            // Setup

            // Execute
            Assert.ThrowsDelegate method = () => new MyoEventArgs(
                null,
                DateTime.UtcNow);

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
            Assert.ThrowsDelegate method = () => new MyoEventArgs(
                myo.Object,
                DateTime.UtcNow);

            // Assert
            Assert.DoesNotThrow(method);
        }
        
        [Fact]
        public void GetMyo_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var myo = new Mock<IMyo>();
            
            var args = new MyoEventArgs(
                myo.Object,
                DateTime.UtcNow);

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
            
            var args = new MyoEventArgs(
                new Mock<IMyo>().Object,
                timestamp);

            // Execute
            var result = args.Timestamp;

            // Assert
            Assert.Equal(timestamp, result);
        }
        #endregion
    }
}
