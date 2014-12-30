using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

using MyoSharp.Device;

namespace MyoSharp.Poses
{
    [ContractClass(typeof(IHeldPoseContract))]
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

    [ContractClassFor(typeof(IHeldPose))]
    internal abstract class IHeldPoseContract : IHeldPose
    {
        #region Events
        public abstract event EventHandler<PoseEventArgs> Triggered;
        #endregion

        #region Properties
        public TimeSpan Interval
        {
            get
            {
                Contract.Ensures(Contract.Result<TimeSpan>() > TimeSpan.Zero);

                return default(TimeSpan);
            }

            set
            {
                Contract.Requires<ArgumentOutOfRangeException>(value > TimeSpan.Zero, "The interval must be greater than zero seconds.");
            }
        }
        #endregion

        #region Methods
        public abstract void Start();

        public abstract void Stop();

        public abstract void Reset();

        public abstract void Dispose();
        #endregion
    }
}
