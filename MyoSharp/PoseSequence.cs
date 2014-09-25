using MyoSharp.Device;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyoSharp
{
    public class PoseSequence
    {
        #region Fields
        private readonly List<Pose> _sequence;
        private readonly List<Pose> _currentSequence;
        private IMyo _myo;
        #endregion

        #region Constructors
        public PoseSequence(IMyo myo, IEnumerable<Pose> sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            _sequence = new List<Pose>();
            _sequence.AddRange(sequence);

            if (_sequence.Count < 2)
            {
                throw new ArgumentException("Sequence length should be at least 2 poses long");
            }

            _currentSequence = new List<Pose>();

            _myo = myo;
            HookMyoEvents();
        }

        public PoseSequence(IMyo myo, params Pose[] sequence)
            : this(myo, (IEnumerable<Pose>)sequence)
        {
        }
        #endregion

        #region Events
        public event EventHandler<PoseEventArgs> PoseSequenceComplete;
        #endregion

        #region Properties
        #endregion

        #region Exposed Members
        #endregion

        #region Internal 
        private void HookMyoEvents() 
        {
            _myo.PoseChanged += Myo_PoseChanged;
        }

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
                PoseSequenceComplete(this, e);
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

        #region Event Handlers
        #endregion
    }
}
