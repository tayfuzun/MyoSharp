using System;
using System.Collections.Generic;
using System.Text;

using MyoSharp.Device;

namespace MyoSharp.Poses
{
    public interface IPoseSequence : IDisposable
    {
        #region Events
        event EventHandler<PoseEventArgs> PoseSequenceCompleted;
        #endregion
    }
}
