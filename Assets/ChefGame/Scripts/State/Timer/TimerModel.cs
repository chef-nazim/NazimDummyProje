using UnityEngine;

namespace gs.chef.onnectnext.timer
{
    /// <summary>
    /// Class representing the state of a timer in the Chef game.
    /// </summary>
    public class TimerModel
    {
        /// <summary>
        /// Gets the unique identifier of the timer.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the duration of the timer in seconds.
        /// </summary>
        public float Duration { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the timer should loop.
        /// </summary>
        public bool IsLoop { get; private set; }

        /// <summary>
        /// Gets the number of loops for the timer.
        /// </summary>
        public int LoopCount { get; private set; }

        /// <summary>
        /// Gets or sets the elapsed time of the timer in seconds.
        /// </summary>
        public float ElapsedTime { get; set; }
        
        /// <summary>
        /// Gets or sets the current time of the timer in seconds.
        /// </summary>
        public float TempDuration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the timer is running.
        /// </summary>
        public bool IsRunning { get; set; } = false;

        /// <summary>
        /// Gets the remaining time of the timer in seconds.
        /// </summary>
        public float RemainingTime
        {
            get
            {
                var result = TempDuration - ElapsedTime;
                result = Mathf.Clamp(result, 0f, TempDuration);
                return result;
            }
        }

        /// <summary>
        /// Constructor for the TimerModel class.
        /// </summary>
        /// <param name="id">The unique identifier of the timer.</param>
        /// <param name="duration">The duration of the timer in seconds.</param>
        /// <param name="loopCount">The number of loops for the timer. -1 is unlimited looping</param>
        public TimerModel(string id, float duration, int loopCount = 0)
        {
            Id = id;
            Duration = duration;
            TempDuration = duration;
            IsLoop = loopCount <= 0;
            LoopCount = loopCount;
        }
    }
}