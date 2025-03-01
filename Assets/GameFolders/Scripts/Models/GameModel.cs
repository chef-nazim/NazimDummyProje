using System;
using System.IO;
using NCG.template._NCG.Core.Model;
using NCG.template.enums;
using NCG.template.io;
using UnityEngine;

namespace NCG.template.models
{
    public class GameModel : BaseGameModel 
    {

        public static GameModel Instance;
        
        
        
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
            Instance = this;
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

    


}