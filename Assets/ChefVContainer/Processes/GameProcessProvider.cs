using System;
using System.Collections.Generic;
using gs.chef.vcontainer.core.model;
using gs.chef.vcontainer.utility;
using VContainer;

namespace gs.chef.vcontainer.processes
{
    public class GameProcessProvider : IDisposable
    {
        [Inject] private readonly IObjectResolver _resolver;

        public IObjectResolver Resolver => _resolver;

        private readonly List<IGameProcess> _gameProcesses = new List<IGameProcess>();

        public IGameProcess GetProcess<T>() where T : IGameProcess
        {
            return _gameProcesses.Find(process => process.GetType() == typeof(T));
        }

        public List<IGameProcess> GetProcesses<T>() where T : IGameProcess
        {
            return _gameProcesses.FindAll(process => process.GetType() == typeof(T));
        }
        
        public bool IsAnyProcess => _gameProcesses.Count > 0;

        public void RemoveProcess<T>() where T : IGameProcess
        {
            var process = _gameProcesses.Find(p => p.GetType() == typeof(T));
            process.Cancel();
            process.Dispose();
            _gameProcesses.Remove(process);
        }

        public void RemoveProcesses<T>() where T : IGameProcess
        {
            var processes = _gameProcesses.FindAll(p => p.GetType() == typeof(T));
            foreach (var process in processes)
            {
                process.Cancel();
                process.Dispose();
                _gameProcesses.Remove(process);
                CLogger.Log(LogState.Process, $"GameProcessProvider: Process removed! {process.Id}");
            }
        }

        public void RemoveAllProcesses()
        {
            foreach (var process in _gameProcesses)
            {
                process.Cancel();
                process.Dispose();
            }

            CLogger.Log(LogState.Process, "GameProcessProvider: All processes removed!");
            _gameProcesses.Clear();
        }

        public void Dispose()
        {
            RemoveAllProcesses();
        }

        public void AddProcess(IGameProcess gameProcess)
        {
            gameProcess.OnComplete(process =>
            {
                _gameProcesses.Remove(process);
                CLogger.Log(LogState.Process, $"GameProcessProvider: Process removed! {process.Id}");
                process.Dispose();
                process.cancellationTokenSource.Cancel();
            });
            _gameProcesses.Add(gameProcess);
        }

        public TProcess CreateProcess<TProcessArgs, TProcess>(TProcessArgs args) where TProcessArgs : IProcessArgs where TProcess : IGameProcess<TProcessArgs>, new()
        {
            // https://stackoverflow.com/a/51722293/16598809
            //var process = new TProcess();
            var process = (TProcess)Activator.CreateInstance(typeof(TProcess));
            _resolver.Inject(process);
            process.Initialize(args);
            AddProcess(process);
            return process;
        }
    }
}