using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using gs.chef.vcontainer.core.model;
using gs.chef.vcontainer.utility;
using VContainer;

namespace gs.chef.vcontainer.processes
{
    public abstract class BaseProcess<TProcessArgs> : IGameProcess<TProcessArgs> where TProcessArgs : IProcessArgs
    {
        [Inject] protected readonly IObjectResolver _resolver;
        
        private IGameProcess _continueProcess;
        
        protected readonly List<IGameProcess> _continueProcesses;

        private CancellationTokenSource _cancellationTokenSource;

        public event Action<IGameProcess> onComplete;

        public bool IsCompleted { get; private set; } = false;

        //public IObjectResolver Resolver => _resolver;
        public string Id => GetType().Name;

        public virtual TProcessArgs Args { get; set; }

        public CancellationTokenSource cancellationTokenSource =>
            _cancellationTokenSource ??= new CancellationTokenSource();

        public CancellationToken cancellationToken => cancellationTokenSource.Token;

        protected BaseProcess()
        {
            _continueProcesses = new List<IGameProcess>();
#if UNITY_EDITOR
            onComplete += process => { CLogger.Log(LogState.Process, $"PROCESS COMPLETED : {Id}"); };
#endif
        }

        public void Initialize(IProcessArgs args)
        {
            Args = args is TProcessArgs ? (TProcessArgs)args : default;
            IsCompleted = false;
            Initialize();
        }

        protected virtual void Initialize(){}

        public IGameProcess OnComplete(Action<IGameProcess> callBack)
        {
            onComplete += callBack;
            return this;
        }

        public IGameProcess AppendProcess(IGameProcess process)
        {
            _continueProcesses.Add(process);
            return this;
        }

        public void Execute(CancellationToken ctx = default)
        {
            IsCompleted = false;
            ExecuteAsync(ctx).AttachExternalCancellation(cancellationToken);
        }

        private async UniTask ExecuteAsync(CancellationToken ctx = default)
        {
#if UNITY_EDITOR
            CLogger.Log(LogState.Process, $"PROCESS STARTED : {Id}");
#endif
            IsCompleted = false;
            await AsyncExecute(ctx).ContinueWith(s => ContinuationFunction(ctx))
                .AttachExternalCancellation(cancellationToken);
        }


        private async UniTask ContinuationFunction(CancellationToken ctx = default)
        {
            var checkList = _continueProcesses.ToList();
            if (checkList.Any())
            {
                foreach (var process in checkList)
                {
                    process.Execute(ctx);
                    if (process.IsCompleted)
                    {
                        //CLogger.Log(LogState.Process, $"----> PROCESS COMPLETED : {process.Id} {process.IsCompleted}");
                        continue;
                    }
                    await UniTask.WaitUntilValueChanged(process, x => x.IsCompleted, cancellationToken: ctx);
                    //CLogger.Log(LogState.Process, $"----> PROCESS COMPLETED : {process.Id} {process.IsCompleted}");
                }
            }

            IsCompleted = true;
            onComplete?.Invoke(this);
        }

        public abstract UniTask<IGameProcess> AsyncExecute(CancellationToken ctx = default);
        /*{
            return UniTask.FromResult<IGameProcess>(this);
        }*/


        public void Cancel()
        {
            cancellationTokenSource?.Cancel();
        }

        public void Dispose()
        {
#if UNITY_EDITOR
            CLogger.Log(LogState.Process, $"PROCESS DISPOSED : {Id}");
#endif
            if (_continueProcess != null)
                _continueProcess.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}