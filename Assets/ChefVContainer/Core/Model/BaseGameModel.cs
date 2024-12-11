using UnityEngine;

namespace gs.chef.vcontainer.core.model
{
    public class BaseGameModel : NotifyPropertyChangedBase
    {
        #region IsMusicOn

        protected string prefKey_IsMusicOn = "IsMusicOn";

        private bool _isMusicOn;

        public bool IsMusicOn
        {
            get
            {
                _isMusicOn = (PlayerPrefs.GetInt(prefKey_IsMusicOn, 1) == 1);
                return _isMusicOn;
            }
            set
            {
                PlayerPrefs.SetInt(prefKey_IsMusicOn, (value) ? 1 : 0);
                SetField(ref _isMusicOn, value, nameof(IsMusicOn));
            }
        }

        #endregion

        #region IsSoundOn

        protected string prefKey_IsSoundOn = "IsSoundOn";

        private bool _isSoundOn;

        public bool IsSoundOn
        {
            get
            {
                _isSoundOn = (PlayerPrefs.GetInt(prefKey_IsSoundOn, 1) == 1);
                return _isSoundOn;
            }
            set
            {
                PlayerPrefs.SetInt(prefKey_IsSoundOn, (value) ? 1 : 0);
                SetField(ref _isSoundOn, value, nameof(IsSoundOn));
            }
        }

        #endregion

        #region IsHapticOn

        protected string prefKey_IsHapticOn = "IsHapticOn";

        private bool _isHapticOn;

        public bool IsHapticOn
        {
            get
            {
#if UNITY_IOS
                _isHapticOn = (PlayerPrefs.GetInt(prefKey_IsHapticOn, 1) == 1);
#elif UNITY_ANDROID
                _isHapticOn = (PlayerPrefs.GetInt(prefKey_IsHapticOn, 0) == 1);
#endif
                return _isHapticOn;
            }
            set
            {
                PlayerPrefs.SetInt(prefKey_IsHapticOn, (value) ? 1 : 0);
                SetField(ref _isHapticOn, value, nameof(IsHapticOn));
            }
        }

        #endregion

        #region IsJoystickOn

        protected string prefKey_IsJoystickOn = "IsJoystickOn";

        private bool _isJoystickOn;

        public bool IsJoystickOn
        {
            get
            {
                _isJoystickOn = (PlayerPrefs.GetInt(prefKey_IsJoystickOn, 1) == 1);
                return _isJoystickOn;
            }
            set
            {
                PlayerPrefs.SetInt(prefKey_IsJoystickOn, (value) ? 1 : 0);
                SetField(ref _isJoystickOn, value, nameof(IsJoystickOn));
            }
        }

        #endregion

        #region IsAdminOn

        protected string prefKey_IsAdminOn = "IsAdminOn";

        private bool _isAdminOn;

        public bool IsAdminOn
        {
            get
            {
                _isAdminOn = (PlayerPrefs.GetInt(prefKey_IsAdminOn, 1) == 1);
                return _isAdminOn;
            }
            set
            {
                PlayerPrefs.SetInt(prefKey_IsAdminOn, (value) ? 1 : 0);
                SetField(ref _isAdminOn, value, nameof(IsAdminOn));
            }
        }

        #endregion
    }
}