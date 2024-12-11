using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace gs.chef.bakerymerge.Objects
{
    public class TutorialHand : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField] Image _handImage;
        [SerializeField] Sprite _handNormalSprite;
        [SerializeField] Sprite _handTapSprite;

        public void HandAnimTrigger(HandAnimEnum handAnimEnum)
        {
            _animator.SetTrigger(handAnimEnum.ToString());
        }
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public async UniTask HandMove(Vector3 targetPos)
        {
            await transform.DOMove(targetPos, 0.5f).SetEase(Ease.Linear).AsyncWaitForCompletion();
        }

        public Vector3 GetUIPos(Vector3 targetPos)
        {
            var pos = Camera.main.WorldToScreenPoint(targetPos);
            return pos;
        }
        public void SetHandPos(Vector3 targetPos)
        {
            transform.position = GetUIPos(targetPos);
        }

        public void SetFreeForm()
        {
            _handImage.sprite = _handNormalSprite;
        }

        public void SetTapForm()
        {
             _handImage.sprite = _handTapSprite;
        }

       
    }

    [Serializable]
    public enum HandAnimEnum
    {
        Tap,
        Open,
        Idle,
        Close
    }
}