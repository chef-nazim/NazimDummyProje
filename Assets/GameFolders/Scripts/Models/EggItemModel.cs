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
    public class EggItemModel : BaseItemModel, IStockModel
    {
        public EggItem Item;

        public void SetLocalPosition(int index)
        {
            Item.SetLocalPosition(new Vector3(0, -0.8f + index * ItemHelper.BoxItemPositionOffset, 0));
        }


        public void SetParent(Transform itemTransform)
        {
            Item.transform.parent = itemTransform;
        }

        public async UniTask FallMovePosition(Vector3 localPos)
        {
            DOTween.Kill(Item.transform);

            await Item.transform.DOLocalMove(localPos, ItemHelper.FallMovePositionDuration).SetEase(Ease.OutExpo);
        }

        public void CreateSettings()
        {
        }

        public async UniTask TakeDamage(Vector3 pos = default)
        {
            await Item.TakeDamage(pos);
        }
    }
}