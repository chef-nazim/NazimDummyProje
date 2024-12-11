using gs.chef.bakerymerge.Objects;
using gs.chef.game.Scripts.Interfaces;
using gs.chef.game.enums;
using gs.chef.game.models;
using Lofelt.NiceVibrations;
using UnityEngine;

namespace gs.chef.game.Events
{
    #region fix

    public struct CreateGamePlaySceneEvent
    {
    }

    public struct LevelModelCreatEvent
    {
    }

    public struct LevelModelCreatedEvent
    {
    }

    public struct LevelCreatedEvent
    {
    }

    public struct LevelDestroyEvent
    {
    }

    public struct LevelCompleteEvent
    {
    }

    public class LevelFailEvent
    {
        public FailType FailType;

        public LevelFailEvent(FailType failType)
        {
            FailType = failType;
        }
    }

    public class RestartButtonClickEvent
    {
        public RestartFrom From;

        public RestartButtonClickEvent(RestartFrom from)
        {
            From = from;
        }
    }

    public class FeelingEvent
    {
        public PlaySound SoundType = PlaySound.None;
        public HapticPatterns.PresetType HapticType = HapticPatterns.PresetType.None;
    }

    #endregion


    public struct TapITableItemEvent
    {
        public ITapableItem TapableItemItem;

        public TapITableItemEvent(ITapableItem tapableItemItem)
        {
            this.TapableItemItem = tapableItemItem;
        }
    }

    
    public class TapBoosterButtonEvent
    {
        public BoosterType BoosterType;

        public TapBoosterButtonEvent(BoosterType boosterType)
        {
            BoosterType = boosterType;
        }
    }


    public class PlayOnButtonClickEvent
    {
        public FailPopUpData FailPopUpData;

        public PlayOnButtonClickEvent(FailPopUpData failPopUpData)
        {
            FailPopUpData = failPopUpData;
        }
    }

    public class RewardPlayOnButtonClickEvent
    {
        public FailPopUpData FailPopUpData;

        public RewardPlayOnButtonClickEvent(FailPopUpData failPopUpData)
        {
            FailPopUpData = failPopUpData;
        }
    }
    public class UseBoosterEvent
    {
        public BoosterType BoosterType;

        public UseBoosterEvent(BoosterType boosterType)
        {
            BoosterType = boosterType;
        }
    }
}