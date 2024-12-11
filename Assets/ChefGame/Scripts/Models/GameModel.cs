using System;
using System.Collections.Generic;
using System.IO;
using gs.chef.game.enums;
using gs.chef.vcontainer.core.model;
using gs.chef.vcontainer.io;
using UnityEngine;
using UnityEngine.Serialization;

namespace gs.chef.game.models
{
    public class GameModel : BaseGameModel
    {
        #region Privates

        private int jsonSize = 1;
        private int maxLevel = 8;
        private int loopLevel = 2;
        private string jsonFileName;
        private WrapperLevelData _levelData;

        #endregion

        #region Tutorial ////////////////////////////////////////////////////////

        private string prefKey_Tutorial = "Tutorial";

        public int Tutorial
        {
            get { return PlayerPrefs.GetInt(prefKey_Tutorial); }
            set
            {
                PlayerPrefs.SetInt(prefKey_Tutorial, value);
                OnPropertyChanged(nameof(Tutorial));
            }
        }

        #endregion

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


        #region CoinCount ////////////////////////////////////////////////////////

        private string prefKey_CoinCount = "CoinCount";

        public int CoinCount
        {
            get { return PlayerPrefs.GetInt(prefKey_CoinCount); }
            set
            {
                PlayerPrefs.SetInt(prefKey_CoinCount, value);
                OnPropertyChanged(nameof(CoinCount));
            }
        }

        #endregion

        #region Reload ////////////////////////////////////////////////////////

        private string prefKey_Reload = "Reload";

        public int Reload
        {
            get { return PlayerPrefs.GetInt(prefKey_Reload); }
            set
            {
                PlayerPrefs.SetInt(prefKey_Reload, value);
                OnPropertyChanged(nameof(Reload));
            }
        }

        #endregion


        #region Level ////////////////////////////////////////////////////////

        private string prefKey_Level = "Level";

        public int Level
        {
            get { return PlayerPrefs.GetInt(prefKey_Level); }
            set
            {
                PlayerPrefs.SetInt(prefKey_Level, value);
                OnPropertyChanged(nameof(Level));
            }
        }


        public static float ShuffleScaleDuration = 0.5f;

        #endregion


        public GameModel() : base()
        {
            #region Tutorial ///////////////////////////////////////////////////

            if (!PlayerPrefs.HasKey(prefKey_Tutorial))
                Tutorial = -1;
            else
                Tutorial = PlayerPrefs.GetInt(prefKey_Tutorial);

            #endregion

            #region CoinCount ///////////////////////////////////////////////////

            if (!PlayerPrefs.HasKey(prefKey_CoinCount))
                CoinCount = 200;
            else
                CoinCount = PlayerPrefs.GetInt(prefKey_CoinCount);

            #endregion

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

            #region Reload ///////////////////////////////////////////////////

            if (!PlayerPrefs.HasKey(prefKey_Reload))
                Reload = 1;
            else
                Reload = PlayerPrefs.GetInt(prefKey_Reload);

            #endregion

            #region Level ///////////////////////////////////////////////////

            if (!PlayerPrefs.HasKey(prefKey_Level))
                Level = 1;
            else
                Level = PlayerPrefs.GetInt(prefKey_Level);

            #endregion
        }


        public void SaveData<T>(string fileName, T gameLevels)
        {
            var path = Path.Combine(Application.streamingAssetsPath, $"LevelDatas/{fileName}.json");
            Debug.Log($"Save path = {path}");
            FileSystem.Save(path, gameLevels);
        }

        public int GetJsonLevel(int level)
        {
            int levelID = level;
            if (levelID > maxLevel)
            {
                levelID = (level - maxLevel);
                levelID = (levelID % (maxLevel - loopLevel)) + (loopLevel - 1);
                if (levelID == (loopLevel - 1))
                {
                    levelID = maxLevel;
                }
            }


            return levelID;
        }

        public WrapperLevelData GetLevelData(int levelID)
        {
            levelID = GetJsonLevel(levelID);

            int factor = Mathf.FloorToInt(levelID == 1 ? 1 : (levelID) / (float)jsonSize);

            try
            {
                string fileName = $"LevelDatas/LevelData_{factor.ToString("D4")}";


                //string fileName = $"LevelData/LevelData_{factor * jsonSize}_{(factor + 1) * jsonSize - 1}";


                Debug.Log($"file name = {fileName}");
                WrapperLevelData wrapperContainerData = FileSystem.Load<WrapperLevelData>(
                    System.IO.Path.Combine
                        (Application.streamingAssetsPath, fileName + ".json"));

                //return wrapperLevelData.LevelDataList.Find(item => item.LevelID == levelID) ?? GetRandomLevelData(levelID);
                _levelData = wrapperContainerData;
                return wrapperContainerData;
            }
            catch (Exception ex)
            {
                //Debug.LogError(ex.Message);
                return null;
            }
        }


        public void AddCoin(int amount)
        {
            CoinCount += amount;
        }
        
        public int GetBoosterCount(BoosterType objBoosterType)
        {
            switch (objBoosterType)
            {
                

                default:
                    return 0;
            }
        }
        public void UseBooster(BoosterType objBoosterType)
        {
            switch (objBoosterType)
            {
                
            }
        }

        public void AddBooster(BoosterType boosterType)
        {
            switch (boosterType)
            {
              
            }
        }
    }

    [Serializable]
    public class WrapperLevelData
    {
        public int Seed;
       
    }


}