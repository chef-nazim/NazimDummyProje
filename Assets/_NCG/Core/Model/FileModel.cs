using System;
using System.IO;
using NCG.template.io;
using NCG.template.models;
using UnityEngine;

namespace NCG.template._NCG.Core.Model
{
    public static class FileModel
    {
        public static void SaveData<T>(string fileName, T gameLevels)
        {
            var path = Path.Combine(Application.streamingAssetsPath, $"LevelDatas/{fileName}.json");
            Debug.Log($"Save path = {path}");
            FileSystem.Save(path, gameLevels);
        }


        public static WrapperLevelData GetLevelData(int levelID)
        {
            //int factor = Mathf.FloorToInt(levelID == 1 ? 1 : (levelID) / (float)1);

            try
            {
                string fileName = $"LevelDatas/LevelData_{levelID.ToString("D4")}";

                //string fileName = $"LevelData/LevelData_{factor * jsonSize}_{(factor + 1) * jsonSize - 1}";


                Debug.Log($"file name = {fileName}");
                WrapperLevelData wrapperContainerData = FileSystem.Load<WrapperLevelData>(
                    System.IO.Path.Combine
                        (Application.streamingAssetsPath, fileName + ".json"));

                //return wrapperLevelData.LevelDataList.Find(item => item.LevelID == levelID) ?? GetRandomLevelData(levelID);
                return wrapperContainerData;
            }
            catch (Exception ex)
            {
                //Debug.LogError(ex.Message);
                return null;
            }
        }
    }
    [Serializable]
    public class WrapperLevelData
    {
        public int Seed;
       
    }
}