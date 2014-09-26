using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

using MyoSharp.Device;

namespace MyoSharp.Poses
{
    public class PoseSequenceEventArgs : PoseEventArgs
    {
        #region Fields
        private readonly ReadOnlyCollection<Pose> _poses;
        #endregion

        #region Constructors
        public PoseSequenceEventArgs(IMyo myo, DateTime timestamp, IList<Pose> poses)
            : base(myo, timestamp, poses[poses.Count - 1])
        {
            // copy this list so we don't have any unexpected reference sharing
            _poses = new List<Pose>(poses).AsReadOnly();
        }
        #endregion

        #region Properties
        public ReadOnlyCollection<Pose> Poses
        {
            get { return _poses; }
        }
        #endregion
    }
}
