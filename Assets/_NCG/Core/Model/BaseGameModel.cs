using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace NCG.template._NCG.Core.Model
{
    public class BaseGameModel 
    {
      
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        #region Sound ////////////////////////////////////////////////////////

        private string prefKey_Sound = "Sound";

        public int Sound
        {
            get { return PlayerPrefs.GetInt(prefKey_Sound); }
            set
            {
                PlayerPrefs.SetInt(prefKey_Sound, value);
                OnPropertyChanged(nameof(Sound));
            }
        }

        #endregion

        #region Haptic ////////////////////////////////////////////////////////

        private string prefKey_Haptic = "Haptic";

        public int Haptic
        {
            get { return PlayerPrefs.GetInt(prefKey_Haptic); }
            set
            {
                PlayerPrefs.SetInt(prefKey_Haptic, value);
                OnPropertyChanged(nameof(Haptic));
            }
        }

        #endregion

        #region SoundTrac ////////////////////////////////////////////////////////

        private string prefKey_SoundTrac = "SoundTrac";

        public int SoundTrac
        {
            get { return PlayerPrefs.GetInt(prefKey_SoundTrac); }
            set
            {
                PlayerPrefs.SetInt(prefKey_SoundTrac, value);
                OnPropertyChanged(nameof(SoundTrac));
            }
        }

        #endregion

        protected BaseGameModel()
        {
          
            #region Sound ///////////////////////////////////////////////////

            if (!PlayerPrefs.HasKey(prefKey_Sound))
                Sound = 1;
            else
                Sound = PlayerPrefs.GetInt(prefKey_Sound);

            #endregion

            #region Haptic ///////////////////////////////////////////////////

            if (!PlayerPrefs.HasKey(prefKey_Haptic))
            {
#if !UNITY_ANDROID
                Haptic = 1;
#else
               Haptic = 0;
#endif
            }


            else
                Haptic = PlayerPrefs.GetInt(prefKey_Haptic);

            #endregion

            #region SoundTrac ///////////////////////////////////////////////////

            if (!PlayerPrefs.HasKey(prefKey_SoundTrac))
                SoundTrac = 1;
            else
                SoundTrac = PlayerPrefs.GetInt(prefKey_SoundTrac);

            #endregion
        }
    }
}