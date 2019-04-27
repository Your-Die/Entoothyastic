using System;
using MEC;
using UnityEngine;
using UnityEngine.Events;

namespace Chinchillada.Timers
{
    /// <summary>
    /// A timer that runs for the <see cref="Duration"/> before invoking <see cref="Finished"/>.
    /// </summary>
    [Serializable]
    public class Timer : ITimer
    {
        [SerializeField] private float _duration;

        [SerializeField] private UnityEvent _finished = null;

        /// <summary>
        /// Handle to the routine.
        /// </summary>
        private CoroutineHandle _timerHandle;

        /// <summary>
        /// Time the current timer run was started.
        /// </summary>
        private float _startTime;

        /// <summary>
        /// The amount of time that this timer has spent running since the last <see cref="Stop"/>, <see cref="Finish"/> or <see cref="Restart"/>.
        /// </summary>
        private float _elapsedTime;

        /// <summary>
        /// Duration of the timer.
        /// </summary>
        public float Duration => _duration;

        public UnityEvent Finished => _finished;

        /// <summary>
        /// Whether the timer is currently running.
        /// </summary>
        public bool IsRunning { get; private set; }

        #region Constructors

        /// <summary>
        /// Construct a new timer.
        /// </summary>
        public Timer() { }

        /// <summary>
        /// Construct a new timer.
        /// </summary>
        /// <param name="duration">The amount of time the timer should run.</param>
        public Timer(float duration)
        {
            _duration = duration;
        }

        /// <summary>
        /// Construct a new timer.
        /// </summary>
        /// <param name="duration">The amount of time the timer should run.</param>
        /// <param name="onFinished">Action invoked when the timer is finished.</param>
        public Timer(float duration, UnityAction onFinished) : this(duration)
        {
            Finished.AddListener(onFinished);
        }

        /// <summary>
        /// Constructs a new timer and immediately starts it.
        /// </summary>
        /// <param name="duration">The amount of time the timer should run.</param>
        /// <param name="onFinished">Action invoked when the timer is finished.</param>
        /// <returns></returns>
        public static Timer StartTimer(float duration, UnityAction onFinished = null)
        {
            Timer timer = new Timer(duration);

            if (onFinished != null)
                timer.Finished.AddListener(onFinished);

            timer.Start();
            return timer;
        }

        #endregion

        /// <summary>
        /// Starts running the timer.
        /// </summary>
        public void Start()
        {
            if (IsRunning)
                return;

            _startTime = Time.time;
            IsRunning = true;

            float remaining = Duration - _elapsedTime;

            if (remaining > 0)
                _timerHandle = Timing.CallDelayed(remaining, Finish);
            else
                Finish();
        }

        /// <summary>
        /// Pause the timer.
        /// </summary>
        public void Pause()
        {
            if (!IsRunning)
                return;

            Timing.KillCoroutines(_timerHandle);
            IsRunning = false;

            float time = Time.time;
            float elapsed = time - _startTime;

            _elapsedTime += elapsed;
        }

        /// <summary>
        /// Restart the timer.
        /// </summary>
        public void Restart()
        {
            Stop();
            Start();
        }

        /// <summary>
        /// Manually finish the timer, making it stop and invoke <see cref="Finished"/>.
        /// </summary>
        public void Finish()
        {
            Stop();
            Finished?.Invoke();
        }

        /// <summary>
        /// Manually stop the timer without invoking <see cref="Finished"/>.
        /// </summary>
        public void Stop()
        {
            Timing.KillCoroutines(_timerHandle);
            IsRunning = false;
            _elapsedTime = 0;
        }
    }
}
