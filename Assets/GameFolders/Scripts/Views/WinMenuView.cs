using System;
using DG.Tweening;
using Lofelt.NiceVibrations;
using NCG.template._NCG.Core.AllEvents;
using NCG.template._NCG.Core.Model;
using NCG.template._NCG.Core.View;
using NCG.template.enums;
using NCG.template.EventBus;
using NCG.template.Managers;
using NCG.template.models;
using NCG.template.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace NCG.template.Views
{
    public class WinMenuView : BaseView
    {
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Text _coinText;
        [SerializeField] private Image _successBG;
        [SerializeField] private GameObject _successText;
        [SerializeField] private GameObject _coinAmount;


        GameModel _gameModel => GameModel.Instance;
        GameHelper _gameHelper => AppManager.instance.GameHelper;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _nextLevelButton.onClick.AddListener(NextLevelButtonClick);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _nextLevelButton.onClick.RemoveListener(NextLevelButtonClick);
        }

        protected override void SubscribeEvents()
        {
        }

        protected override void UnSubscribeEvents()
        {
        }

        public override void Show(MenuData menuData)
        {
            EventBus<FeelingEvent>.Publish(new FeelingEvent()
            {
                HapticType = HapticPatterns.PresetType.Success,
                SoundType = PlaySound.Win
            });


            SetCoinText(_gameHelper.LevelCompleteCoin);
            PlayPanelOpeningAnimation();
        }

        public override void Hide()
        {
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
            _successBG.transform.DOLocalRotate(new Vector3(0, 0, 360), 15f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear).SetDelay(1.1f).SetLoops(-1, LoopType.Restart);
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

        private void NextLevelButtonClick()
        {
            _gameModel.Level += 1;
            EventBus<FeelingEvent>.Publish(new FeelingEvent
            {
                SoundType = PlaySound.ButtonClick,
                HapticType = HapticPatterns.PresetType.SoftImpact
            });
            KillPanelOpeningAnimation();
            EventBus<LevelDestroyEvent>.Publish(new LevelDestroyEvent());
            _gameModel.AddCoin(_gameHelper.LevelCompleteCoin);
            EventBus<CreateGamePlaySceneEvent>.Publish(new CreateGamePlaySceneEvent());
        }
    }

    public class WinMenuViewData : MenuData
    {
    }
}