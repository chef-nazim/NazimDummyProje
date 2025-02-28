using System.Collections.Generic;
using DG.Tweening;
using NCG.template.models;
using NCG.template.Scripts.Objects;
using NCG.template.enums;
using NCG.template.Scripts.Others;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace NCG.template.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "GameHelper", fileName = "GameHelper", order = 0)]
    public class GameHelper : ScriptableObject
    {
        public List<SoundPackItem> Clips = new List<SoundPackItem>();

        public int LevelCompleteCoin = 20;


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
    }
}