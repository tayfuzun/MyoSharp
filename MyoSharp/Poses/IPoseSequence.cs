using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp.Poses
{
    public interface IPoseSequence : IDisposable
    {
        #region Events
        event EventHandler<PoseSequenceEventArgs> PoseSequenceCompleted;
        #endregion
    }
}
