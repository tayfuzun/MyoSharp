using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Text;

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
            Contract.Requires<ArgumentNullException>(myo != null, "myo");
            Contract.Requires<ArgumentNullException>(poses != null, "poses");
            Contract.Requires<ArgumentException>(poses.Count > 0, "The sequence must contain at least one pose.");

            // copy this list so we don't have any unexpected reference sharing
            _poses = new List<Pose>(poses).AsReadOnly();
        }
        #endregion

        #region Properties
        public ReadOnlyCollection<Pose> Poses
        {
            get
            {
                Contract.Ensures(Contract.Result<ReadOnlyCollection<Pose>>() != null);

                return _poses;
            }
        }
        #endregion

        #region Methods
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_poses != null);
        }
        #endregion
    }
}
