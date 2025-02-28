using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace gs.chef.bakerymerge.Objects
{
    public class RewardButton : MonoBehaviour
    {
        public event Action OnRewardButtonClicked;
        
        [SerializeField] Button _rewardButton;
        
        [SerializeField] GameObject _rewardActiveGO;
        [SerializeField] GameObject _rewardInactiveGO;
        
        
        [SerializeField] Image _rewardButtonImage;
        [SerializeField] Sprite _activeSprite;
        [SerializeField] Sprite _inactiveSprite;
        [SerializeField] Text _rewardTimerText;
         Tween _rewardTimerTween;
         
         public void SetStartSettings(bool isReady)
         {
             _rewardButton.onClick.AddListener(() =>
             {
                 OnRewardButtonClicked?.Invoke();
             });
             StatusChanged(isReady);
         }
        public void StatusChanged(bool isReady)
        {
          
            if (isReady)
            {
                _rewardButton.interactable = true;
                _rewardActiveGO.SetActive(true);
                _rewardInactiveGO.SetActive(false);
                _rewardTimerTween?.Kill();
                _rewardButtonImage.sprite = _activeSprite;
            }
            else
            {
                _rewardTimerTween?.Kill();
                _rewardButton.interactable = false;
                _rewardActiveGO.SetActive(false);
                _rewardInactiveGO.SetActive(true);
                _rewardButtonImage.sprite = _inactiveSprite;
                
                _rewardTimerTween= DOTween.To(() => 5, x => _rewardTimerText.text = x.ToString(), 0, 6).SetDelay(1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
            }    
        }
        public void SetCloseSettings()
        {
            _rewardButton.onClick.RemoveAllListeners();
            _rewardTimerTween?.Kill();
        }
    }
}