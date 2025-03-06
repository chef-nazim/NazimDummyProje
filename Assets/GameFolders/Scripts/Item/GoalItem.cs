using NCG.template._NCG.Pool;
using NCG.template.models;
using UnityEngine;

namespace NCG.template.Scripts.Item
{
    public class GoalItem : MonoBehaviour, ISpawnItem<GoalItemModel>
    {
        public GoalItemModel ItemModel { get; set; }
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void ReInitialize(GoalItemModel itemModel)
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