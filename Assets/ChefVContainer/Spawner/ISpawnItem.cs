using System;

namespace gs.chef.vcontainer.spawner
{
    public interface ISpawnItem<TModel> : IDisposable  where TModel : ISpawnItemModel
    {
        TModel ItemModel { get; set; }
        
        void SetActive(bool isActive);
        
        void ReInitialize(TModel itemModel);
    }
}