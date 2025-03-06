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
    public class CellItemModel : BaseItemModel, IStockModel
    
    {
        public CellItem Item;
        public int ColorID;
        public bool IsRuning;

        public void SetPosition(Vector3 position)
        {
            Item.SetPosition(position);
        }

        public void SetParent(Transform itemTransform)
        {
            Item.SetParent(itemTransform);
        }


        public void Dispose()
        {
            Item.Clear();
            Item.SetActive(false);
        }


        public void ItemLookAt()
        {
            Vector3 parentSpaceDirection = Item.transform.parent.transform.position;
            parentSpaceDirection.y = Item.transform.position.y;
            Item.transform.LookAt(parentSpaceDirection);
        }

        public async UniTask JumpPositon(Vector3 movePosition, float duration, float delay)
        {
            Item.transform
                .DOLocalRotate(
                    new Vector3(360, Item.transform.rotation.eulerAngles.y, Item.transform.rotation.eulerAngles.z),
                    ItemHelper.CollectItemMoveRotationDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear)
                .SetDelay(delay);
            float height1 = Item.transform.position.y;
            float height2 = movePosition.y + 0.4f;
            float height3 = height1 > height2 ? height1 : height2;

            await Item.transform.DOLocalJump(new Vector3(movePosition.x, height3, movePosition.z), 3, 1, duration)
                .SetEase(Ease.Linear).SetDelay(delay);
            await Item.transform.DOLocalMove(movePosition, ItemHelper.CollectItemMovePositionDuration / 2f)
                .SetEase(Ease.Linear);
        }

        public void PlayAnimation()
        {
            Item.PlayAnimation();
        }

        public async UniTask FallMovePosition(Vector3 localPos)
        {
            DOTween.Kill(Item.transform);

            await Item.transform.DOLocalMove(localPos, ItemHelper.FallMovePositionDuration).SetEase(Ease.OutExpo);
        }

        public void CreateSettings()
        {
        }

        public async UniTask FeedAnimation(Vector3 localPos, float delay)
        {
            Item.SetLocalPosition(localPos + new Vector3(0, ItemHelper.FeedYOffset, ItemHelper.FeedZOffset));
            Item.transform.localScale = Vector3.zero;
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            Item.transform.localScale = Vector3.one;
            Item.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            await Item.transform.DOLocalMove(localPos, ItemHelper.CollectItemMovePositionDuration)
                .SetEase(Ease.OutBack);
        }

        public void SetLocalPosition(int index)
        {
            Item.SetLocalPosition(new Vector3(0, index * ItemHelper.CellItemPositionOffset, 0));
        }

        public void SetLocalPosition(Vector3 pos)
        {
            Item.SetLocalPosition(pos);
        }


        public async UniTask MoveGoalPos(Vector3 vector3)
        {
            await Item.transform.DOScale(Vector3.one * 1.2f, ItemHelper.GoalItemMoveDuration).SetEase(Ease.OutBounce);
            Item.transform.DOScale(Vector3.one, ItemHelper.GoalItemMoveDuration).SetEase(Ease.InQuad);
            await Item.transform.DOMove(vector3, ItemHelper.GoalItemMoveDuration).SetEase(Ease.InQuad);
        }

        public Vector3 GetPosition()
        {
            return Item.transform.position;
        }
    }
}