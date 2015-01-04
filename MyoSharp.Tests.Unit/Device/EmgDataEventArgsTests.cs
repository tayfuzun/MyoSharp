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
    public class EmgDataEventArgsTests
    {
        #region Methods
        [Fact]
        public void Constructor_NullMyo_ThrowsArgumentNullException()
        {
            // Setup
            var emgData = new Mock<IEmgData>();

            // Execute
            Assert.ThrowsDelegate method = () => new EmgDataEventArgs(
                null,
                DateTime.UtcNow,
                emgData.Object);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(method);
            Assert.Equal("myo", exception.ParamName);
        }

        [Fact]
        public void Constructor_NullEmgData_ThrowsArgumentNullException()
        {
            // Setup
            var myo = new Mock<IMyo>();

            // Execute
            Assert.ThrowsDelegate method = () => new EmgDataEventArgs(
                myo.Object,
                DateTime.UtcNow,
                null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(method);
            Assert.Equal("emgData", exception.ParamName);
        }

        [Fact]
        public void Constructor_ValidArguments_DoesNotThrow()
        {
            // Setup
            var myo = new Mock<IMyo>();
            var emgData = new Mock<IEmgData>();

            // Execute
            Assert.ThrowsDelegate method = () => new EmgDataEventArgs(
                myo.Object,
                DateTime.UtcNow,
                emgData.Object);

            // Assert
            Assert.DoesNotThrow(method);
        }
        
        [Fact]
        public void GetMyo_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var myo = new Mock<IMyo>();
            
            var args = new EmgDataEventArgs(
                myo.Object,
                DateTime.UtcNow,
                new Mock<IEmgData>().Object);

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
            
            var args = new EmgDataEventArgs(
                new Mock<IMyo>().Object,
                timestamp,
                new Mock<IEmgData>().Object);

            // Execute
            var result = args.Timestamp;

            // Assert
            Assert.Equal(timestamp, result);
        }

        [Fact]
        public void GetEmgData_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var emgData = new Mock<IEmgData>();
            
            var args = new EmgDataEventArgs(
                new Mock<IMyo>().Object,
                DateTime.UtcNow,
                emgData.Object);

            // Execute
            var result = args.EmgData;

            // Assert
            Assert.Equal(emgData.Object, result);
        }
        #endregion
    }
}
