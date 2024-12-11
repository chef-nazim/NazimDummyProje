using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace gs.chef.vcontainer.processes
{
    public interface IGameProcess<TArgs> : IGameProcess where TArgs : IProcessArgs
    {
        TArgs Args { get; }
    }
    public interface IGameProcess : IDisposable
    {
        string Id { get; }
        
        bool IsCompleted { get; }

        void Initialize(IProcessArgs args);
        CancellationTokenSource cancellationTokenSource { get; }

        CancellationToken cancellationToken { get; }

        IGameProcess AppendProcess(IGameProcess process);

        IGameProcess OnComplete(Action<IGameProcess> callBack);

        void Execute(CancellationToken ctx);

        UniTask<IGameProcess> AsyncExecute(CancellationToken ctx);

        void Cancel();
    }
}