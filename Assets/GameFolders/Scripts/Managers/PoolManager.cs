using NCG.template._NCG.Core.BaseClass;
using NCG.template._NCG.Pool;
using NCG.template.models;
using NCG.template.Scripts.Item;
using UnityEngine;

namespace NCG.template.Managers
{
    public  class PoolManager : BaseManager
    {
        
        BasePool<Transform,CellItemModel ,CellItem > cellItemPool;
        CellItem cellItem;
        private Transform _transform;
        public override void Initialize()
        {
            var cellItemPool = new BasePool<Transform, CellItemModel, CellItem>( cellItem, 10, _transform);
            
        }

        public override void Dispose()
        {
            cellItemPool.Dispose();
        }
    }
}