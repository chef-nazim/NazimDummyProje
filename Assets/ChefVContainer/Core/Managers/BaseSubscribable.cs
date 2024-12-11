using System;
using MessagePipe;
using VContainer.Unity;

namespace gs.chef.vcontainer.core.managers
{
    public abstract class BaseSubscribable : IInitializable, IDisposable
    {
        protected IDisposable _subscription;
        
        protected DisposableBagBuilder _bagBuilder;
        
        public virtual void Initialize()
        {
            _bagBuilder = DisposableBag.CreateBuilder();
            Init();
            Subscriptions();
            _subscription = _bagBuilder.Build();
        }

        protected virtual void Init()
        {
            
        }

        protected virtual void Subscriptions(){}

        public virtual void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}