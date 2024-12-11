using System;
using DG.Tweening;
using gs.chef.vcontainer.menu;
using UnityEngine;
using UnityEngine.UI;

namespace gs.chef.game.Views
{
    public class WinMenuView : BaseMenuView
    {
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Text _coinText;
        [SerializeField] private Image _successBG;
        [SerializeField] private GameObject _successText;
        [SerializeField] private GameObject _coinAmount;
        
        public event Action OnNextLevelButtonClick;

        private void Start()
        {
            _nextLevelButton.onClick.AddListener(() =>
            {
                OnNextLevelButtonClick?.Invoke();
            });
        }

        public void SetCoinText(int i)
        {
            _coinText.text = i.ToString();
           
        }
        
        public void PlayPanelOpeningAnimation()
        {
            _successText.transform.localScale = Vector3.zero;
            _coinAmount.transform.localScale = Vector3.zero;
            _successBG.transform.localScale = Vector3.zero;
            _nextLevelButton.gameObject.transform.localScale = Vector3.zero;
            _successBG.transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBack).SetDelay(0.3f);
            _successBG.transform.DOLocalRotate( new Vector3(0,0,360), 15f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetDelay(1.1f).SetLoops(-1, LoopType.Restart);
            _successText.transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBack).SetDelay(0.3f);
            _coinAmount.transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBack).SetDelay(0.3f);
            _coinText.transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBack).SetDelay(0.3f);
            _nextLevelButton.gameObject.transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBack).SetDelay(0.3f);
        }

        public void KillPanelOpeningAnimation()
        {
            DOTween.Kill(_successBG.transform);
            DOTween.Kill(_successText.transform);
            DOTween.Kill(_coinAmount.transform);
            DOTween.Kill(_coinText.transform);
            DOTween.Kill(_nextLevelButton.gameObject.transform);
        }
    }

    public class WinMenuViewData : MenuData
    {
    }
}