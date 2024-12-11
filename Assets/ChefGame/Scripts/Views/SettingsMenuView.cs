using System;
using gs.chef.game.Scripts.Objects;
using gs.chef.vcontainer.menu;
using UnityEngine;
using UnityEngine.UI;

namespace gs.chef.game.Views
{
    public class SettingsMenuView : BaseMenuView
    {
        #region Settings ////////////////////////////////////////////////////////

        public Text versionText;


        [Header("Button")] public Button SettingsCloseButton;

        public Button RestartButton;

        public SettingsButton SoundButton;


        public SettingsButton HapticButton;


        //public GameObject SoundTrack;

        #endregion

        #region Action

        public event Action<SettingsButton> OnSettingsButtonClicked;
        public event Action OnSettingsCloseButtonClicked;
        public event Action OnRestartButtonClicked;

        #endregion


        private void Start()
        {
            versionText.text = $"v{Application.version}";
            SoundButton.button.onClick.AddListener(() => OnSettingsButtonClicked?.Invoke(SoundButton));
            HapticButton.button.onClick.AddListener(() => OnSettingsButtonClicked?.Invoke(HapticButton));
            SettingsCloseButton.onClick.AddListener(() => OnSettingsCloseButtonClicked?.Invoke());
            RestartButton.onClick.AddListener(() => OnRestartButtonClicked?.Invoke());
        }

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

        public void ButtonControl(SettingsMenuViewData data)
        {
            bool isHapticOn = data.Haptic == 1 ? true : false;
            HapticButton.SetSprite(isHapticOn);

            bool isSoundOn = data.Sound == 1 ? true : false;
            SoundButton.SetSprite(isSoundOn);


            HapticButton.gameObject.SetActive(SupportVibration);
        }
    }

    public class SettingsMenuViewData : MenuData
    {
        public int Sound;
        public int Haptic;
        
        
        
        public  SettingsMenuViewData(int sound, int haptic)
        {
            Sound = sound;
            Haptic = haptic;
        }
        
        
    }
}