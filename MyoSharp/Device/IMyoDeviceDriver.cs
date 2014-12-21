using System;
using System.Collections.Generic;
using System.Text;

using MyoSharp.Math;
using MyoSharp.Poses;

namespace MyoSharp.Device
{
    public interface IMyoDeviceDriver
    {
        #region Properties
        IntPtr Handle { get; }
        #endregion

        #region Methods
        void Vibrate(VibrationType type);
        
        void RequestRssi();
        
        void Lock();
        
        void Unlock(UnlockType type);
        
        sbyte GetEventRssi(IntPtr evt);
        
        Vector3F GetEventAccelerometer(IntPtr evt);
        
        float GetEventAccelerometer(IntPtr evt, uint index);
        
        XDirection GetEventDirectionX(IntPtr evt);
        
        QuaternionF GetEventOrientation(IntPtr evt);
        
        float GetEventOrientation(IntPtr evt, OrientationIndex index);
        
        float GetFirmwareVersion(IntPtr evt, VersionComponent component);
        
        Arm GetArm(IntPtr evt);
        
        Vector3F GetGyroscope(IntPtr evt);
        
        float GetGyroscope(IntPtr evt, uint index);
        
        Pose GetEventPose(IntPtr evt);
        #endregion
    }
}
