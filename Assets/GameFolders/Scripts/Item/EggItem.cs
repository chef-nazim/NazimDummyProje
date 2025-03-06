using NCG.template._NCG.Pool;
using NCG.template.models;
using NCG.template.Scripts.Interfaces;
using UnityEngine;

namespace NCG.template.Scripts.Item
{
    public class EggItem : MonoBehaviour, ISpawnItem<EggItemModel>
    {
        public EggItemModel ItemModel { get; set; }
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void ReInitialize(EggItemModel itemModel)
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
        }
    }
}