using System;
using NCG.template._NCG.Pool;

namespace NCG.template._NCG.Core.Model
{
    [Serializable]
    public class BaseItemModel : ISpawnItemModel
    {
    }
    /*public class BaseItem : MonoBehaviour/*, ISpawnItem<BaseItemModel>#1#
   {
       /*public BaseItemModel ItemModel { get; set; }
       public void SetActive(bool isActive)
       {
           gameObject.SetActive(isActive);
       }

       public void ReInitialize(BaseItemModel itemModel)
       {
           ItemModel = itemModel;
           itemModel.Item = this;
       }

       public void SetParent(Transform parent)
       {
           transform.SetParent(parent);
       }

       public void DisposeItem()
       {
           Destroy(gameObject);
       }#1#
   }*/
    /*public class BaseItem2 : MonoBehaviour, ISpawnItem
    {
        public BaseItemModel ItemModel { get; set; }
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void ReInitialize()
        {

        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void DisposeItem()
        {
            Destroy(gameObject);
        }
    }*/
}