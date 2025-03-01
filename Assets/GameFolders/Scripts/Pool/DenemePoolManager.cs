using FluffyUnderware.DevTools;
using NCG.template._NCG.Pool;
using NCG.template.models;
using NCG.template.Scripts.Item;
using UnityEngine;

namespace NCG.template.GameFolders.Scripts.Pool
{
    public class DenemePoolManager  : BasePool<Transform, BaseItemModel, BaseItem>
    {
        public DenemePoolManager(BaseItem prefab, int poolSize, Transform poolParent, bool willGrow = false) : base(prefab, poolSize, poolParent, willGrow)
        {
             
            
        }
    }
}