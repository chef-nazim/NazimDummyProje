using System;

/// <summary>
/// Namespace for the timer functionality in the Chef game.
/// </summary>
namespace gs.chef.onnectnext.timer
{
    /// <summary>
    /// Interface for a timer in the Chef game.
    /// </summary>
    public interface ITimer : IDisposable
    {
        /// <summary>
        /// Gets the TimerModel representing the state of the timer.
        /// </summary>
        public TimerModel TimerModel { get; }

        /// <summary>
        /// Updates the timer on each tick.
        /// </summary>
        public void Tick();

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start();
        
        /// <summary>
        /// Restarts the timer.
        /// </summary>
        public void Restart();

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop();

        /// <summary>
        /// Pauses the timer.
        /// </summary>
        public void Pause();

        /// <summary>
        /// Resumes the timer.
        /// </summary>
        public void Resume();
        
        /// <summary>
        /// Adds extra duration to the timer.
        /// </summary>
        /// <param name="duration"></param>
        public void AddExtraDuration(float duration);
        
        public ITimer OnAddExtraDurationTimer(Action<string, float, float, float> onAddExtraDuration);

        /// <summary>
        /// Sets the OnComplete event and returns the timer.
        /// </summary>
        public ITimer OnCompleteTimer(Action<string> onComplete);

        /// <summary>
        /// Sets the OnUpdate event and returns the timer.
        /// </summary>
        public ITimer OnUpdateTimer(Action<string, float, float> onTick);

        /// <summary>
        /// Sets the OnCycleComplete event and returns the timer.
        /// </summary>
        public ITimer OnCycleCompleteTimer(Action<string, int> onCycleComplete);
    }
}