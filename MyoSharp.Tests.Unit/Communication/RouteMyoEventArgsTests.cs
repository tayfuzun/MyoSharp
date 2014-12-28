using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Xunit;

using MyoSharp.Communication;
using MyoSharp.Device;

namespace MyoSharp.Tests.Unit.Communication
{
    public class RouteMyoEventArgsTests
    {
        #region Methods
        [Fact]
        public void Constructor_InvalidMyoHandle_ThrowsArgumentException()
        {
            // Setup

            // Execute
            Assert.ThrowsDelegate method = () => new RouteMyoEventArgs(
                IntPtr.Zero, 
                new IntPtr(123), 
                MyoEventType.Paired, 
                DateTime.UtcNow);

            // Assert
            var exception = Assert.Throws<ArgumentException>(method);
            Assert.Equal("myoHandle", exception.ParamName);
        }

        [Fact]
        public void Constructor_InvalidEventHandle_ThrowsArgumentException()
        {
            // Setup

            // Execute
            Assert.ThrowsDelegate method = () => new RouteMyoEventArgs(
                new IntPtr(123),
                IntPtr.Zero,
                MyoEventType.Paired,
                DateTime.UtcNow);

            // Assert
            var exception = Assert.Throws<ArgumentException>(method);
            Assert.Equal("evt", exception.ParamName);
        }

        [Fact]
        public void Constructor_ValidArguments_DoesNotThrow()
        {
            // Setup

            // Execute
            Assert.ThrowsDelegate method = () => new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(123),
                MyoEventType.Paired,
                DateTime.UtcNow);

            // Assert
            Assert.DoesNotThrow(method);
        }

        [Fact]
        public void GetEvent_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var eventHandle = new IntPtr(123);
            var args = new RouteMyoEventArgs(
                new IntPtr(789),
                eventHandle,
                MyoEventType.Paired,
                DateTime.UtcNow);

            // Execute
            var result = args.Event;

            // Assert
            Assert.Equal(eventHandle, result);
        }

        [Fact]
        public void GetMyoHandle_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var myoHandle = new IntPtr(123);
            var args = new RouteMyoEventArgs(
                myoHandle,
                new IntPtr(789),
                MyoEventType.Paired,
                DateTime.UtcNow);

            // Execute
            var result = args.MyoHandle;

            // Assert
            Assert.Equal(myoHandle, result);
        }

        [Fact]
        public void GetEventType_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var eventType = MyoEventType.Emg;
            var args = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(123),
                eventType,
                DateTime.UtcNow);

            // Execute
            var result = args.EventType;

            // Assert
            Assert.Equal(eventType, result);
        }

        [Fact]
        public void GetTimestamp_ValidState_EqualsConstructorParameter()
        {
            // Setup
            var timestamp = DateTime.UtcNow;
            var args = new RouteMyoEventArgs(
                new IntPtr(123),
                new IntPtr(123),
                MyoEventType.Paired,
                timestamp);

            // Execute
            var result = args.Timestamp;

            // Assert
            Assert.Equal(timestamp, result);
        }
        #endregion
    }
}
