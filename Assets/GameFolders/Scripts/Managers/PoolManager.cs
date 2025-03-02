using NCG.template._NCG.Core.AllEvents;
using NCG.template._NCG.Core.BaseClass;
using NCG.template._NCG.Pool;
using NCG.template.EventBus;
using NCG.template.models;
using NCG.template.Scripts.Item;
using UnityEngine;

namespace NCG.template.Managers
{
    public  class PoolManager : BaseManager
    {
        BasePool<Transform,CellItemModel ,CellItem > cellItemPool;
        
        public override void Initialize()
        {
            EventBus<CreatePoolsEvent>.Subscribe(CreatePools);
            
        }

        public void CreatePools(CreatePoolsEvent obj)
        {
            
        }

        public override void Dispose()
        {
            cellItemPool.Dispose();
        }
    }
}