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
    public class ArmRecognizedEventArgsTests
    {
        #region Methods
        [Fact]
        public void Constructor_NullMyo_ThrowsArgumentNullException()
        {
            // Setup

            // Execute
            Assert.ThrowsDelegate method = () => new ArmRecognizedEventArgs(
                null,
                DateTime.UtcNow,
                Arm.Unknown,
                XDirection.Unknown);

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
            Assert.ThrowsDelegate method = () => new ArmRecognizedEventArgs(
                myo.Object,
                DateTime.UtcNow,
                Arm.Unknown,
                XDirection.Unknown);

            // Assert
            Assert.DoesNotThrow(method);
        }
        
        [Fact]
        public void GetMyo_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var myo = new Mock<IMyo>();
            
            var args = new ArmRecognizedEventArgs(
                myo.Object,
                DateTime.UtcNow,
                Arm.Unknown,
                XDirection.Unknown);

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
            
            var args = new ArmRecognizedEventArgs(
                new Mock<IMyo>().Object,
                timestamp,
                Arm.Unknown,
                XDirection.Unknown);

            // Execute
            var result = args.Timestamp;

            // Assert
            Assert.Equal(timestamp, result);
        }

        [Fact]
        public void GetArm_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var arm = Arm.Left;
            
            var args = new ArmRecognizedEventArgs(
                new Mock<IMyo>().Object,
                DateTime.UtcNow,
                arm,
                XDirection.Unknown);

            // Execute
            var result = args.Arm;

            // Assert
            Assert.Equal(arm, result);
        }

        [Fact]
        public void GetXDirection_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var directionX = XDirection.TowardElbow;

            var args = new ArmRecognizedEventArgs(
                new Mock<IMyo>().Object,
                DateTime.UtcNow,
                Arm.Unknown,
                directionX);

            // Execute
            var result = args.XDirection;

            // Assert
            Assert.Equal(directionX, result);
        }
        #endregion
    }
}
