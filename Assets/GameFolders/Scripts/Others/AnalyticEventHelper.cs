using NCG.template.enums;
using UnityEngine;

namespace NCG.template.Scripts.Others
{
    
        public  static class AnalyticEventHelper
        {
        #region public
        
        public static void LevelStart(int level)
        {
            /*string levelIDToString = GetLevelIDToString(level);
            LionAnalytics.LevelStart(level);

            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, levelIDToString);

            GameAnalytics.NewDesignEvent("LevelStart:" + levelIDToString);

            if (!PlayerPrefs.HasKey("UniqueLevelStart" + level.ToString("D5")))
            {
                PlayerPrefs.SetFloat("UniqueLevelStart" + level.ToString("D5"), 0);
                GameAnalytics.NewDesignEvent("UniqueLevelStart:" + levelIDToString);
            }


            int levelAttemptCount = GetLevelAttemptCount();
            levelAttemptCount += 1;
            SetLevelAttemptCount(levelAttemptCount);*/
        }


        public static void LevelComplete(int level, int timeSpent ,int remainingTime, int coin)
        {
            /*string levelIDToString = GetLevelIDToString(level);
            LevelCompleteBase(level);

            GameAnalytics.NewDesignEvent("LevelComplete:" + "TimeSpent:" + levelIDToString, timeSpent);
            GameAnalytics.NewDesignEvent("LevelComplete:" + "RemainingTime:" + levelIDToString, remainingTime);
            GameAnalytics.NewDesignEvent("LevelComplete:" + "Coin:" + levelIDToString, coin);*/
        }
        public static void LevelComplete(int level)
        {
            /*string levelIDToString = GetLevelIDToString(level);
            LevelCompleteBase(level);
        }
        public static void RVShow( FailType type,int level)
        {
            /*GameAnalytics.NewDesignEvent("RW:" + type.ToString()+":"+ level.ToString("D5"));
            BoosterUsed(level, type.ToString());*/
        }

        public static void GameStart()
        {
          //  LionAnalytics.GameStart();
        }
        public static void GameEnd()
        {
           //LionAnalytics.GameEnded();
        }

        public static void LevelFailDesignEvent(int level, string from)
        {
            /*string levelIDToString = GetLevelIDToString(level);
            GameAnalytics.NewDesignEvent($"LevelFail:{from}:" + levelIDToString);*/
        }
        
        public static void LevelStepEvent(int level,int step)
        {
            /*string collection1 = $"{level},{step}";
           LionAnalytics.LevelStep(level,0,collection1,"");*/
        }


        public static void LevelRestart(int level, string from)
        {
            LevelRestartBase(level, from);
        }

        public static void BoosterUsed(int level, string boosterType)
        {
            /*string levelIDToString = GetLevelIDToString(level);
            string levelID = level.ToString("D5");

            LionAnalytics.PowerUpUsed(levelID, "", 0, boosterType);

            GameAnalytics.NewDesignEvent($"Click:Booster:{boosterType}:" + levelIDToString);*/
        }

        #endregion

        #region private

        private static void LevelCompleteBase(int level)
        {
            /*string levelIDToString = GetLevelIDToString(level);
            int levelAttemptCount = GetLevelAttemptCount();
            SetLevelAttemptCount(0);

            LionAnalytics.LevelComplete(level, levelAttemptCount);

            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, levelIDToString);

            GameAnalytics.NewDesignEvent("LevelComplete:Level:" + levelIDToString);*/
        }

        private static void LevelRestartBase(int level, string from)
        {
            /*int levelAttemptCount = GetLevelAttemptCount();
            LionAnalytics.LevelAbandoned(level, levelAttemptCount);
            GameAnalytics.NewDesignEvent("Click:Restart:" + from + ":" + GetLevelIDToString(level));
            LevelFailBase(level);*/
        }

        private static void LevelFailBase(int level)
        {
            /*string levelIDToString = GetLevelIDToString(level);
            LionAnalytics.LevelFail(level);

            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, levelIDToString);*/
        }

        private static string GetLevelIDToString(int level)
        {
            return "Level_" + level.ToString("D5");
        }

        private static int GetLevelAttemptCount()
        {
            int levelAttemptCount = PlayerPrefs.GetInt("LevelAttemptCount", 0);
            return levelAttemptCount;
        }

        private static void SetLevelAttemptCount(int levelAttemptCount)
        {
            PlayerPrefs.SetInt("LevelAttemptCount", levelAttemptCount);
        }

        #endregion
        }    
    

    
}