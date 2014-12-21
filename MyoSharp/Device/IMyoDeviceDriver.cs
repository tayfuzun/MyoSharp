using System;
using System.Collections.Generic;
using System.Text;

using MyoSharp.Math;
using MyoSharp.Poses;

namespace MyoSharp.Device
{
    /// <summary>
    /// An interface that defines functionality for interacting with a Myo device.
    /// </summary>
    public interface IMyoDeviceDriver
    {
        #region Properties
        /// <summary>
        /// Gets the handle to the Myo device.
        /// </summary>
        IntPtr Handle { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Causes the Myo to vibrate.
        /// </summary>
        /// <param name="type">The type of vibration to use.</param>
        void Vibrate(VibrationType type);

        /// <summary>
        /// TODO: RSSI?
        /// </summary>
        void RequestRssi();

        /// <summary>
        /// Locks the Myo.
        /// </summary>
        void Lock();

        /// <summary>
        /// Unlocks the Myo.
        /// </summary>
        /// <param name="type">The type of unlock.</param>
        void Unlock(UnlockType type);

        /// <summary>
        /// TODO: RSSI?
        /// </summary>
        /// <param name="evt">The event handle.</param>
        /// <returns>TODO: RSSI?</returns>
        sbyte GetEventRssi(IntPtr evt);

        /// <summary>
        /// Gets the accelerometer event data.
        /// </summary>
        /// <param name="evt">The event handle.</param>
        /// <returns>Returns the accelerometer event data.</returns>
        Vector3F GetEventAccelerometer(IntPtr evt);

        /// <summary>
        /// Gets the accelerometer event data for the specified index.
        /// </summary>
        /// <param name="evt">The event handle.</param>
        /// <param name="index">The index.</param>
        /// <returns>Returns the accelerometer data for the specified index.</returns>
        float GetEventAccelerometer(IntPtr evt, uint index);

        /// <summary>
        /// Gets the x-axis direction data for the specified event.
        /// </summary>
        /// <param name="evt">The event handle.</param>
        /// <returns>Returns the x-axis direction data for the specified event.</returns>
        XDirection GetEventDirectionX(IntPtr evt);

        /// <summary>
        /// Gets the orientation data for the specified event.
        /// </summary>
        /// <param name="evt">The event handle.</param>
        /// <returns>Returns the orientation data for the specified event.</returns>
        QuaternionF GetEventOrientation(IntPtr evt);

        /// <summary>
        /// Gets the orientation data for the specified event and index.
        /// </summary>
        /// <param name="evt">The event handle.</param>
        /// <param name="index">The orientation index.</param>
        /// <returns>Returns the orientation data for the specified event and index.</returns>
        float GetEventOrientation(IntPtr evt, OrientationIndex index);

        /// <summary>
        /// Gets the firmware version for the specified version component.
        /// </summary>
        /// <param name="evt">The event handle.</param>
        /// <param name="component">The version component to get information for.</param>
        /// <returns>Returns the firmware version for the specified version component.</returns>
        float GetFirmwareVersion(IntPtr evt, VersionComponent component);

        /// <summary>
        /// Gets the arm that the Myo is on.
        /// </summary>
        /// <param name="evt">The event handle.</param>
        /// <returns>Returns the arm that the Myo is on.</returns>
        Arm GetArm(IntPtr evt);

        /// <summary>
        /// Gets the gyroscope data for the specified event.
        /// </summary>
        /// <param name="evt">The event handle.</param>
        /// <returns>Returns the gyroscope data for the specified event.</returns>
        Vector3F GetGyroscope(IntPtr evt);

        /// <summary>
        /// Gets the gyroscope data for the specified event and index.
        /// </summary>
        /// <param name="evt">The event handle.</param>
        /// <param name="index">The index of the gyroscope data.</param>
        /// <returns>Returns the gyroscope data for the specified event and index.</returns>
        float GetGyroscope(IntPtr evt, uint index);

        /// <summary>
        /// Gets the pose for the specified event.
        /// </summary>
        /// <param name="evt">The event handle.</param>
        /// <returns>Returns the pose for the specified event.</returns>
        Pose GetEventPose(IntPtr evt);

        /// <summary>
        /// Sets whether or not EMG data streaming should be enabled for the Myo.
        /// </summary>
        /// <param name="enabled">If set to <c>true</c>, EMG data streaming will be enabled on the Myo; Otherwise, it will be disabled.</param>
        void SetEmgStreaming(bool enabled);

        /// <summary>
        /// Gets the EMG data for a sensor for the specified event.
        /// </summary>
        /// <param name="evt">The event handle.</param>
        /// <param name="sensor">The sensor to get EMG data on.</param>
        /// <returns>Returns the EMG data for the specified sensor and event.</returns>
        sbyte GetEventEmg(IntPtr evt, int sensor);
        #endregion
    }
}
