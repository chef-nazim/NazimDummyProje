using System;
using Cysharp.Threading.Tasks;
using NCG.template._NCG.Core.Model;
using NCG.template.Scripts.Interfaces;
using NCG.template.Scripts.Item;
using UnityEngine;

namespace NCG.template.models
{
    [Serializable]
    public class MailBoxItemModel : BaseItemModel, IStockModel
    {
        public MailBoxItem Item;
        public void SetLocalPosition(int index)
        {
            Item.SetLocalPosition(Vector3.zero);
        }

        public void SetParent(Transform itemTransform)
        {
            Item.transform.parent = itemTransform;
        }
       

        public async UniTask FallMovePosition(Vector3 localPos)
        {
            
        }

        public void CreateSettings()
        {
            
        }

        public async void MailBoxDamage(Vector3 pos= default)
        {
            Item.MailBoxDamage(pos);
          
            /*await Item.transform.DOScale(1.2f, ItemHelper.FallMovePositionDuration/2f).SetEase(Ease.OutExpo);
            await Item.transform.DOScale(1f, ItemHelper.FallMovePositionDuration/2f).SetEase(Ease.OutExpo);*/
        }
    }
}