using System;
using gs.chef.game.models;
using gs.chef.game.Scripts.Item;
using gs.chef.vcontainer.spawner;
using UnityEngine;

namespace gs.chef.game.Scripts.Pools
{
    public class BaseItemPooling : BaseSpawnPool<Transform, BaseItemModel, BaseItem>
    {
        public BaseItemPooling(Func<Transform, BaseItemModel, BaseItem> spawnFunc, int poolSize, Transform poolTarget, bool willGrow = false) : base(spawnFunc, poolSize, poolTarget, willGrow)
        {
            
        }
    }
}