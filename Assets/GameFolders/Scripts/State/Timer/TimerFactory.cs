using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer.Unity;

/// <summary>
/// Namespace for the timer functionality in the Chef game.
/// </summary>
namespace gs.chef.onnectnext.timer
{
    /// <summary>
    /// Factory class for creating and managing timers in the Chef game.
    /// </summary>
    public class TimerFactory : IDisposable, ITickable
    {
        /// <summary>
        /// List of timers managed by the factory.
        /// </summary>
        private List<ITimer> _timers;

        /// <summary>
        /// Constructor for the ChefTimerFactory class.
        /// </summary>
        public TimerFactory()
        {
        }

        /// <summary>
        /// Adds a timer of type T to the factory.
        /// </summary>
        public T AddTimer<T>(TimerModel timerModel) where T : AbsTimer
        {
            T checkTimer = FindTimer<T>(timerModel);
            if (checkTimer != null)
            {
                return checkTimer;
            }

            ITimer timer = Activator.CreateInstance(typeof(T), timerModel) as ITimer;
            _timers ??= new List<ITimer>();
            _timers.Add(timer);
            return timer as T;
        }

        /// <summary>
        /// Finds a timer of type T with the given id.
        /// </summary>
        public T FindTimer<T>(string id) where T : AbsTimer
        {
            return GetTimer<T>(id);
        }

        /// <summary>
        /// Finds a timer of type T with the given TimerModel.
        /// </summary>
        public T FindTimer<T>(TimerModel timerModel) where T : AbsTimer
        {
            return GetTimer<T>(timerModel.Id);
        }

        /// <summary>
        /// Finds or creates a timer of type T with the given TimerModel.
        /// </summary>
        public T FindOrCreateTimer<T>(TimerModel timerModel) where T : AbsTimer
        {
            ITimer timer = FindTimer<T>(timerModel);
            if (timer == null)
            {
                timer = AddTimer<T>(timerModel);
            }

            return timer as T;
        }

        /// <summary>
        /// Gets a timer of type T with the given id.
        /// </summary>
        private T GetTimer<T>(string id) where T : AbsTimer
        {
            _timers ??= new List<ITimer>();
            return (T)_timers.FirstOrDefault(s => s.TimerModel.Id == id);
        }

        /// <summary>
        /// Removes a timer of type T with the given id.
        /// </summary>
        public void RemoveTimer<T>(string id) where T : AbsTimer
        {
            ITimer timer = FindTimer<T>(id);
            if (timer != null)
            {
                timer.Dispose();
                _timers.Remove(timer);
            }
        }

        /// <summary>
        /// Removes a timer of type T with the given TimerModel.
        /// </summary>
        public void RemoveTimer<T>(TimerModel timerModel) where T : AbsTimer
        {
            RemoveTimer<T>(timerModel.Id);
        }

        /// <summary>
        /// Removes all timers managed by the factory.
        /// </summary>
        public void RemoveAllTimers()
        {
            if (_timers == null) return;

            foreach (ITimer timer in _timers)
            {
                timer.Dispose();
            }

            _timers.Clear();
        }

        /// <summary>
        /// Disposes of the factory and all its timers.
        /// </summary>
        public void Dispose()
        {
            RemoveAllTimers();
            Debug.Log( $"[Timer {nameof(TimerFactory)}] Dispose");
        }

        /// <summary>
        /// Updates all timers managed by the factory on each tick.
        /// </summary>
        public void Tick()
        {
            if (_timers == null) return;

            foreach (ITimer timer in _timers)
            {
                timer.Tick();
            }
        }
    }
}