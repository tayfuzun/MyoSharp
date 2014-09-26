using System;
using System.Collections.Generic;
using System.Text;

using MyoSharp.Device;

namespace MyoSharp.Poses
{
    public interface IHeldPose : IDisposable
    {
        #region Events
        event EventHandler<PoseEventArgs> Triggered;
        #endregion

        #region Properties
        TimeSpan Interval { get; set; }
        #endregion

        #region Methods
        void Start();

        void Stop();

        void Reset();
        #endregion
    }
}
