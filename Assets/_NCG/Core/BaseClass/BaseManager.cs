using Cysharp.Threading.Tasks;
using NCG.template.EventBus;

namespace NCG.template._NCG.Core.BaseClass
{
    public abstract class BaseManager
    {
        public abstract void Initialize();

        public abstract void Dispose();

    }
}