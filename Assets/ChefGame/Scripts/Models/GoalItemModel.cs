using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using gs.chef.vcontainer.spawner;
using UnityEngine;

namespace gs.chef.game.models
{
    [Serializable]
    public class GoalItemModel : BaseItemModel
    {
        
        
    }




    [Serializable]
    public class BaseItemModel : ISpawnItemModel
    {
        
        public BaseItem Item { get; set; }
        
        public void SetParent(Transform transform)
        {
            Item.transform.SetParent(transform);
        }

        public void SetLocalPosition(Vector3 zero)
        {
            Item.transform.localPosition = zero;
        }

        public void SetLocalRotation(Quaternion identity)
        {
            Item.transform.localRotation = identity;
        }
        
        public void SetLocalScale(Vector3 one)
        {
            Item.transform.localScale = one;
        }
        
        
        public void SetPosition(Vector3 position)
        {
            Item.transform.position = position;
        }
        
        public void SetRotation(Quaternion rotation)
        {
            Item.transform.rotation = rotation;
        }
        
        public void SetScale(Vector3 scale)
        {
            Item.transform.localScale = scale;
        }
        
        
        public async UniTask MoveLocalPosition(Vector3 position, float duration , Ease ease)
        {
            await Item.transform.DOLocalMove(position, duration).SetEase(ease).AsyncWaitForCompletion();
        }
        
        public async UniTask MovePosition(Vector3 position, float duration , Ease ease)
        {
            await Item.transform.DOMove(position, duration).SetEase(ease).AsyncWaitForCompletion();
        }
        
        public async UniTask RotateLocal(Vector3 rotation, float duration , Ease ease)
        {
            await Item.transform.DOLocalRotate(rotation, duration).SetEase(ease).AsyncWaitForCompletion();
        }
        
        public async UniTask Rotate(Vector3 rotation, float duration , Ease ease)
        {
            await Item.transform.DORotate(rotation, duration).SetEase(ease).AsyncWaitForCompletion();
        }
        
        public async UniTask Scale(Vector3 scale, float duration , Ease ease)
        {
            await Item.transform.DOScale(scale, duration).SetEase(ease).AsyncWaitForCompletion();
        }
        
        public async UniTask ScaleLocal(Vector3 scale, float duration , Ease ease)
        {
            await Item.transform.DOScale(scale, duration).SetEase(ease).AsyncWaitForCompletion();
        }
        
        
        public virtual void DisposeModel()
        {
         
        }
    }

    public class BaseItem : MonoBehaviour , ISpawnItem<BaseItemModel>
    {
        public BaseItemModel ItemModel { get; set; }
        
        public void Dispose()
        {
            
        }

        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void ReInitialize(BaseItemModel itemModel)
        {
            ItemModel = itemModel;
            itemModel.Item = this;
        }
    }

}