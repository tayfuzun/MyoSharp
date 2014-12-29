using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

using MyoSharp.Device;

namespace MyoSharp.Poses
{
    /// <summary>
    /// A class that will monitor a Myo for a specified sequence of poses.
    /// </summary>
    public class PoseSequence : IPoseSequence
    {
        #region Fields
        private readonly List<Pose> _sequence;
        private readonly List<Pose> _currentSequence;
        private readonly IMyoEventGenerator _myo;

        private bool _disposed;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PoseSequence"/> class.
        /// </summary>
        /// <param name="myo">The Myo.</param>
        /// <param name="sequence">The sequence of poses to watch for. Cannot be <c>null</c>.</param>
        protected PoseSequence(IMyoEventGenerator myo, IEnumerable<Pose> sequence)
        {
            Contract.Requires<ArgumentNullException>(myo != null);
            Contract.Requires<ArgumentNullException>(sequence != null);

            _sequence = new List<Pose>();
            _sequence.AddRange(sequence);
            if (_sequence.Count < 2)
            {
                throw new ArgumentException("Sequence length should be at least 2 poses long.", "sequence");
            }
            
            _currentSequence = new List<Pose>();

            _myo = myo;
            _myo.PoseChanged += Myo_PoseChanged;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PoseSequence"/> class.
        /// </summary>
        /// <param name="myo">The Myo.</param>
        /// <param name="sequence">The sequence of poses to watch for. Cannot be <c>null</c>.</param>
        protected PoseSequence(IMyoEventGenerator myo, params Pose[] sequence)
            : this(myo, (IEnumerable<Pose>)sequence)
        {
            Contract.Requires<ArgumentNullException>(myo != null);
            Contract.Requires<ArgumentNullException>(sequence != null);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="PoseSequence"/> class.
        /// </summary>
        ~PoseSequence()
        {
            Dispose(false);
        }
        #endregion

        #region Events
        public event EventHandler<PoseSequenceEventArgs> PoseSequenceCompleted;
        #endregion

        #region Methods
        public static IPoseSequence Create(IMyoEventGenerator myo, params Pose[] sequence)
        {
            Contract.Requires<ArgumentNullException>(myo != null);
            Contract.Requires<ArgumentNullException>(sequence != null);
            Contract.Ensures(Contract.Result<IPoseSequence>() != null);

            return new PoseSequence(myo, sequence);
        }

        public static IPoseSequence Create(IMyoEventGenerator myo, IEnumerable<Pose> sequence)
        {
            Contract.Requires<ArgumentNullException>(myo != null);
            Contract.Requires<ArgumentNullException>(sequence != null);
            Contract.Ensures(Contract.Result<IPoseSequence>() != null);

            return new PoseSequence(myo, sequence);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) 
        {
            if (_disposed)
            {
                return;
            }

            try
            {
                if (disposing)
                {
                    _myo.PoseChanged -= Myo_PoseChanged;
                }
            }
            finally
            {
                _disposed = true;
            }
        }

        protected virtual void OnPoseSequenceCompleted(IMyo myo, DateTime timestamp, IList<Pose> poses)
        {
            Contract.Requires<ArgumentNullException>(myo != null, "myo");
            Contract.Requires<ArgumentNullException>(poses != null, "poses");
            Contract.Requires<ArgumentException>(poses.Count > 0, "The sequence must contain at least one pose.");

            var handler = PoseSequenceCompleted;
            if (handler != null)
            {
                var args = new PoseSequenceEventArgs(
                    myo,
                    timestamp,
                    poses);
                handler.Invoke(this, args);
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_sequence != null);
            Contract.Invariant(_sequence.Count >= 2);
            Contract.Invariant(_currentSequence != null);
            Contract.Invariant(_myo != null);
        }
        #endregion

        #region Event Handlers
        private void Myo_PoseChanged(object sender, PoseEventArgs e)
        {
            if (_currentSequence.Count == _sequence.Count)
            {
                for (int i = 0; i < _currentSequence.Count; i++)
                {
                    if (_currentSequence[i] != _sequence[i])
                    {
                        var lastItem = _currentSequence[_currentSequence.Count - 1];
                        _currentSequence.Clear();
                        _currentSequence.Add(lastItem);
                        return;
                    }
                }

                // got a match
                OnPoseSequenceCompleted(e.Myo, e.Timestamp, _currentSequence);
                _currentSequence.Clear();
            }
            else
            {
                switch (e.Pose)
                {
                    case Pose.WaveOut:
                    case Pose.WaveIn:
                    case Pose.FingersSpread:
                    case Pose.Fist:
                    case Pose.DoubleTap:
                        _currentSequence.Add(e.Pose);
                        break;
                }
            }
        }
        #endregion
    }
}
