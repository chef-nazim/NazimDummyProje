using System;

using UnityEngine;

/// <summary>
/// Abstract base class for a timer in the Chef game.
/// </summary>
namespace gs.chef.onnectnext.timer
{
    public abstract class AbsTimer : ITimer
    {
        /// <summary>
        /// The model representing the timer's state.
        /// </summary>
        protected readonly TimerModel _timerModel;

        /// <summary>
        /// Event triggered when the timer completes.
        /// </summary>
        protected event Action<string> OnComplete;

        /// <summary>
        /// Event triggered on each timer update.
        /// </summary>
        protected event Action<string, float, float> OnUpdate;

        /// <summary>
        /// Event triggered when a timer cycle completes.
        /// </summary>
        protected event Action<string, int> OnCycleComplete;
        
        /// <summary>
        /// Event triggered when a timer adds extra duration.
        /// </summary>
        protected event Action<string, float, float, float> OnAddExtraDuration; 

        /// <summary>
        /// The current loop count of the timer.
        /// </summary>
        private int _loopCount;

        /// <summary>
        /// Constructor for the AbsChefTimer class.
        /// </summary>
        public AbsTimer(TimerModel timerModel)
        {
            _timerModel = timerModel;
            Debug.Log( $"[Timer] Duration: {_timerModel.Duration}");
            _timerModel.IsRunning = false;
            _loopCount = timerModel.LoopCount;
        }

        /// <summary>
        /// Disposes of the timer and logs the event.
        /// </summary>
        public virtual void Dispose()
        {
            OnComplete = null;
            OnUpdate = null;
            OnCycleComplete = null;
            OnAddExtraDuration = null;
            Debug.Log(  $"[Timer {_timerModel.Id}] Dispose");
        }

        /// <summary>
        /// Gets the TimerModel.
        /// </summary>
        public TimerModel TimerModel => _timerModel;

        /// <summary>
        /// Updates the timer on each tick.
        /// </summary>
        public virtual void Tick()
        {
            if (!_timerModel.IsRunning)
                return;

            _timerModel.ElapsedTime += Time.deltaTime;
            OnUpdate?.Invoke(_timerModel.Id, _timerModel.RemainingTime, _timerModel.ElapsedTime);

            if (_timerModel.RemainingTime > 0)
                return;

            if (_timerModel.IsLoop)
            {
                HandleLoop();
            }
            else
            {
                EndTimer();
            }
        }

        /// <summary>
        /// Handles the loop logic of the timer.
        /// </summary>
        private void HandleLoop()
        {
            if (_timerModel.LoopCount < 0)
            {
                _loopCount++;
                ResetElapsedTime();
                OnCycleComplete?.Invoke(_timerModel.Id, _loopCount);
            }
            else
            {
                if (_loopCount > 0)
                {
                    _loopCount--;
                }
                else if (_loopCount == 0)
                {
                    EndTimer();
                    return;
                }

                ResetElapsedTime();
                OnCycleComplete?.Invoke(_timerModel.Id, _timerModel.LoopCount - _loopCount);
            }
        }

        /// <summary>
        /// Ends the timer and triggers the OnComplete event.
        /// </summary>
        private void EndTimer()
        {
            _timerModel.IsRunning = false;
            OnComplete?.Invoke(_timerModel.Id);
        }

        /// <summary>
        /// Resets the elapsed time of the timer.
        /// </summary>
        private void ResetElapsedTime()
        {
            _timerModel.ElapsedTime = 0;
        }

        /// <summary>
        /// Starts the timer and logs the event.
        /// </summary>
        public virtual void Start()
        {
            Debug.Log(  $"[Timer {_timerModel.Id}] Start");
            _timerModel.TempDuration = _timerModel.Duration;
            _timerModel.IsRunning = true;
        }

        public void Restart()
        {
            Debug.Log( $"[Timer {_timerModel.Id}] Restart");
            _timerModel.IsRunning = false;
            _timerModel.ElapsedTime = 0;
            _loopCount = _timerModel.LoopCount;
            _timerModel.TempDuration = _timerModel.Duration;
            _timerModel.IsRunning = true;
        }


        /// <summary>
        /// Stops the timer, resets the elapsed time, and logs the event.
        /// </summary>
        public virtual void Stop()
        {
            Debug.Log(  $"[Timer {_timerModel.Id}] Stop");
            _timerModel.IsRunning = false;
            _timerModel.TempDuration = _timerModel.Duration;
            _timerModel.ElapsedTime = 0;
            _loopCount = _timerModel.LoopCount;
        }

        /// <summary>
        /// Pauses the timer and logs the event.
        /// </summary>
        public virtual void Pause()
        {
            Debug.Log(  $"[Timer {_timerModel.Id}] Pause");
            _timerModel.IsRunning = false;
        }

        /// <summary>
        /// Resumes the timer and logs the event.
        /// </summary>
        public virtual void Resume()
        {
            Debug.Log( $"[Timer {_timerModel.Id}] Resume");
            _timerModel.IsRunning = true;
        }

        /// <summary>
        /// Adds the duration to the timer.
        /// </summary>
        /// <param name="duration"></param>
        public virtual void AddExtraDuration(float duration)
        {
            _timerModel.TempDuration += duration;
            OnAddExtraDuration?.Invoke(_timerModel.Id, _timerModel.RemainingTime, _timerModel.ElapsedTime, duration);
        }
        
        
        /// <summary>
        /// Adds the duration to the timer.
        /// </summary>
        /// <param name="onAddExtraDuration"></param>
        /// <returns></returns>
        public virtual ITimer OnAddExtraDurationTimer(Action<string, float, float, float> onAddExtraDuration)
        {
            OnAddExtraDuration += onAddExtraDuration;
            return this;
        }

        /// <summary>
        /// Sets the OnComplete event and returns the timer.
        /// </summary>
        public virtual ITimer OnCompleteTimer(Action<string> onComplete)
        {
            OnComplete += onComplete;
            return this;
        }

        /// <summary>
        /// Sets the OnUpdate event and returns the timer.
        /// </summary>
        public virtual ITimer OnUpdateTimer(Action<string, float, float> onTick)
        {
            OnUpdate += onTick;
            return this;
        }

        /// <summary>
        /// Sets the OnCycleComplete event and returns the timer.
        /// </summary>
        public virtual ITimer OnCycleCompleteTimer(Action<string, int> onCycleComplete)
        {
            OnCycleComplete += onCycleComplete;
            return this;
        }

        /// <summary>
        /// Destructor for the AbsChefTimer class.
        /// </summary>
        ~AbsTimer()
        {
            Dispose();
        }
    }
}