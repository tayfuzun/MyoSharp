using System;
using System.Timers;

using MyoSharp.Device;

namespace MyoSharp.Poses
{
    public class HeldPose : IHeldPose
    {
        #region Constants
        private static readonly TimeSpan DEFAULT_INTERVAL = TimeSpan.FromSeconds(1);
        #endregion

        #region Fields
        private readonly Pose _targetPose;
        private readonly Timer _timer;

        private bool _timerAlive;
        private bool _timerPaused;
        private IMyoEventGenerator _myo;
        private IMyo _triggeringMyo;
        private bool _disposed;
        private Pose _lastPose;
        #endregion

        #region Constructors
        protected HeldPose(IMyoEventGenerator myo, Pose targetPose, TimeSpan interval)
        {
            if (myo == null)
            {
                throw new ArgumentNullException("myo", "The Myo cannot be null.");
            }

            if (targetPose == Pose.Unknown)
            {
                throw new ArgumentException("The target pose must be specified.", "targetPose");
            }

            if (interval.TotalMilliseconds <= 0)
            {
                throw new ArgumentException("The interval must be a positive time value.", "interval");
            }

            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Interval = interval.TotalMilliseconds;
            _timer.Elapsed += Timer_Elapsed;
            
            _targetPose = targetPose;
            _lastPose = Pose.Unknown;

            _myo = myo;
            _myo.PoseChanged += Myo_PoseChanged;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="HeldPose"/> class.
        /// </summary>
        ~HeldPose()
        {
            Dispose(false);
        }
        #endregion

        #region Events
        public event EventHandler<PoseEventArgs> Triggered;
        #endregion

        #region Properties
        public TimeSpan Interval
        {
            get
            {
                return TimeSpan.FromMilliseconds(_timer.Interval);
            }
            
            set
            {
                if (value.TotalMilliseconds <= 0)
                {
                    throw new ArgumentException("The interval must be a positive time value.", "interval");
                }

                _timer.Interval = value.TotalMilliseconds;
            }
        }
        #endregion

        #region Methods
        public static IHeldPose Create(IMyoEventGenerator myo, Pose targetPose)
        {
            return Create(myo, targetPose, DEFAULT_INTERVAL);
        }

        public static IHeldPose Create(IMyoEventGenerator myo, Pose targetPose, TimeSpan interval)
        {
            return new HeldPose(myo, targetPose, interval);
        }

        public void Start()
        {
            lock (_timer)
            {
                _timer.Start();
                _timerAlive = true;
                _timerPaused = false;
            }
        }

        public void Stop()
        {
            lock (_timer)
            {
                _timer.Stop();
                _timerAlive = false;
                _timerPaused = false;
            }
        }

        public void Reset()
        {
            lock (_timer)
            {
                Stop();
                Start();
            }
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
                    if (_timer != null)
                    {
                        Stop();
                        _timer.Dispose();
                    }

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

        protected virtual void OnTriggered(IMyo myo, DateTime timestamp, Pose pose)
        {
            var handler = Triggered;
            if (handler != null)
            {
                var args = new PoseEventArgs(
                    myo,
                    timestamp,
                    pose);
                handler.Invoke(this, args);
            }
        }
        #endregion

        #region Event Handlers
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_lastPose != _targetPose)
            {
                return;
            }

            OnTriggered(
                _triggeringMyo, 
                e.SignalTime,
                _targetPose);
        }

        private void Myo_PoseChanged(object sender, PoseEventArgs e)
        {
            _lastPose = e.Pose;
            _triggeringMyo = e.Myo;

            lock (_timer)
            {
                if (_timerAlive)
                {
                    return;
                }

                if (e.Pose == _targetPose)
                {
                    if (_timerPaused)
                    {
                        _timerPaused = false;
                        _timer.Start();
                    }
                }
                else
                {
                    _timerPaused = true;
                    _timer.Stop();
                }
            }
        }
        #endregion
    }
}
