using System.Collections.Generic;
using DG.Tweening;
using NCG.template.models;
using NCG.template.Scripts.Objects;
using NCG.template.enums;
using NCG.template.Scripts.Others;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace NCG.template.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "GameHelper", fileName = "GameHelper", order = 0)]
    public class GameHelper : ScriptableObject
    {
        [SerializeField]private int maxLevel = 2;
        [SerializeField]private int loopLevel = 1;
        public List<SoundPackItem> Clips = new List<SoundPackItem>();

        public int LevelCompleteCoin = 20;
        
        [BoxGroup("GridItem")]
        public float GridItemWidth = 1.0f;
        [BoxGroup("GridItem")]
        public float GridItemHeight = 1f;

        

        public static int GetBoosterPrice(BoosterType boosterType)
        {
            switch (boosterType)
            {
                /*case BoosterType.Hammer:
                    return 100;
                case BoosterType.Bomb:
                    return 100;
                case BoosterType.Shuffle:
                    return 100;
                case BoosterType.Color:
                    return 100;
                case BoosterType.Magic:
                    return 100;*/
                default:
                    return 0;
            }
        }

        public int CurrentLevel(int level)
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
    }
}