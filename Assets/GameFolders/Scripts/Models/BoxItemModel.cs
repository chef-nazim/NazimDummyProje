using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NCG.template._NCG.Core.Model;
using NCG.template.Scripts.Interfaces;
using NCG.template.Scripts.Item;
using UnityEngine;

namespace NCG.template.models
{
    [Serializable]
    public class BoxItemModel : BaseItemModel, IStockModel
    
    {
        public BoxItem Item;
        public void SetLocalPosition(int  index)
        {
            Item.SetLocalPosition(new Vector3(0, -0.8f + index * ItemHelper.BoxItemPositionOffset, 0));
        }

        public void SetParent(Transform itemTransform)
        {
            Item.transform.parent = itemTransform;
        }

        public async UniTask FallMovePosition(Vector3 localPos)
        {
            /*DOTween.Kill(Item.transform);

            await Item.transform.DOLocalMove(localPos, ItemHelper.FallMovePositionDuration).SetEase(Ease.OutExpo);*/
        }

        public void CreateSettings()
        {
            
        }


        public async void Damage()
        {
            await Item.transform.DOScale(1.5f, ItemHelper.FallMovePositionDuration/2f).SetEase(Ease.OutExpo);
            await Item.transform.DOScale(1f, ItemHelper.FallMovePositionDuration/2f).SetEase(Ease.OutExpo);
            Dispose();
        }
        public void Dispose()
        {
            Item.CreateEffect();
            Item.Clear();
            Item.SetActive(false);
        }
    }
}