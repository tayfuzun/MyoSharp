using System;
using System.Collections.Generic;
using System.Text;

using MyoSharp.Device;

namespace MyoSharp.Poses
{
    public class PoseSequence : IPoseSequence
    {
        #region Fields
        private readonly List<Pose> _sequence;
        private readonly List<Pose> _currentSequence;
        private IMyo _myo;
        private bool _disposed;
        #endregion

        #region Constructors
        protected PoseSequence(IMyo myo, IEnumerable<Pose> sequence)
        {
            if (myo == null)
            {
                throw new ArgumentNullException("myo", "The Myo cannot be null.");
            }

            if (sequence == null)
            {
                throw new ArgumentNullException("sequence", "The sequence cannot be null.");
            }

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

        protected PoseSequence(IMyo myo, params Pose[] sequence)
            : this(myo, (IEnumerable<Pose>)sequence)
        {
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
        public event EventHandler<PoseEventArgs> PoseSequenceCompleted;
        #endregion

        #region Methods
        public static IPoseSequence Create(IMyo myo, params Pose[] sequence)
        {
            return new PoseSequence(myo, sequence);
        }

        public static IPoseSequence Create(IMyo myo, IEnumerable<Pose> sequence)
        {
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
                    if (_myo != null)
                    {
                        _myo.PoseChanged -= Myo_PoseChanged;
                    }
                }
            }
            finally
            {
                _disposed = true;
            }
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
                PoseSequenceCompleted(this, e);
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
                    case Pose.ThumbToPinky:
                        _currentSequence.Add(e.Pose);
                        break;
                }
            }
        }
        #endregion
    }
}
