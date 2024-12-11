using System.Threading;
using Cysharp.Threading.Tasks;

namespace gs.chef.vcontainer.processes
{
    public interface IGameTask<T> where T : IGameTask<T>
    {
        UniTask<T> Execute(CancellationToken ctx = default);
    }
}