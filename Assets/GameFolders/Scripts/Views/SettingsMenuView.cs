using System;
using Lofelt.NiceVibrations;
using NCG.template._NCG.Core.AllEvents;
using NCG.template._NCG.Core.Model;
using NCG.template._NCG.Core.View;
using NCG.template.enums;
using NCG.template.EventBus;
using NCG.template.Events;
using NCG.template.models;
using NCG.template.Scripts.Objects;
using NCG.template.Scripts.Others;
using UnityEngine;
using UnityEngine.UI;

namespace NCG.template.Views
{
    public class SettingsMenuView : BaseView
    {
        #region Settings ////////////////////////////////////////////////////////

        public Text versionText;


        [Header("Button")] public Button SettingsCloseButton;

        public Button RestartButton;

        public SettingsButton SoundButton;


        public SettingsButton HapticButton;


        //public GameObject SoundTrack;

        #endregion

      

        GameModel _gameModel => GameModel.Instance;
        

        #region Support Vibration

        private static bool? _supportVibration;

        public static bool SupportVibration
        {
            get
            {
                if (!_supportVibration.HasValue)
                {
#if!UNITY_EDITOR
                _supportVibration = SystemInfo.supportsVibration;
#else
                    _supportVibration = true;
#endif
                }

                return _supportVibration.Value;
            }
        }

        #endregion

        public void ButtonControl()
        {
            bool isHapticOn = _gameModel.Haptic == 1 ? true : false;
            HapticButton.SetSprite(isHapticOn);

            bool isSoundOn = _gameModel.Sound == 1 ? true : false;
            SoundButton.SetSprite(isSoundOn);


            HapticButton.gameObject.SetActive(SupportVibration);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            versionText.text = $"v{Application.version}";
            SoundButton.button.onClick.AddListener(() => ButtonClicked(SoundButton));
            HapticButton.button.onClick.AddListener(() => ButtonClicked(HapticButton));
            SettingsCloseButton.onClick.AddListener(() => CloseButtonClicked());
            RestartButton.onClick.AddListener(() => RestartButtonClick());
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            SoundButton.button.onClick.RemoveAllListeners();
            HapticButton.button.onClick.RemoveAllListeners();
            SettingsCloseButton.onClick.RemoveAllListeners();
            RestartButton.onClick.RemoveAllListeners();
        }

        protected override void SubscribeEvents()
        {
            
        }

        protected override void UnSubscribeEvents()
        {
            
        }

        public override void Show(MenuData menuData)
        {
            
            View.SetActive(true);
        }

        public override void Hide()
        {
            
            View.SetActive(false);
        }
        
        
        private void RestartButtonClick()
        {
            AnalyticEventHelper.LevelFailDesignEvent(_gameModel.Level, FailType.RestartButton.ToString());
            EventBus<CloseMenuEvent>.Publish(new CloseMenuEvent(MenuNames.SettingsMenu));
            EventBus<FeelingEvent>.Publish(new FeelingEvent
            {
                SoundType = PlaySound.ButtonClick,
                HapticType = HapticPatterns.PresetType.SoftImpact
            });
            EventBus<RestartButtonClickEvent>.Publish(new RestartButtonClickEvent(RestartFrom.GamePlayView));
        }



        private void CloseButtonClicked()
        {
            EventBus<CloseMenuEvent>.Publish(new CloseMenuEvent(MenuNames.SettingsMenu));
            
            EventBus<FeelingEvent>.Publish(new FeelingEvent
            {
                SoundType = PlaySound.ButtonClick,
                HapticType = HapticPatterns.PresetType.SoftImpact
            });
        }


        private void ButtonClicked(SettingsButton button)
        {
            if (Equals(button, SoundButton))
            {
                _gameModel.Sound = (_gameModel.Sound == 1) ? 0 : 1;
            }
            else if (Equals(button, HapticButton))
            {
                _gameModel.Haptic = (_gameModel.Haptic == 1) ? 0 : 1;
            }

            ButtonControl();
        }

      
    }

    public class SettingsMenuViewData : MenuData
    {
      
        public  SettingsMenuViewData()
        {
          
        }
        
        
    }
}