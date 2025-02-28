using gs.chef.bakerymerge.Objects;
using NCG.template.models;
using Lofelt.NiceVibrations;
using NCG.template.enums;
using NCG.template.EventBus;
using NCG.template.Scripts.Interfaces;
using UnityEngine;

namespace NCG.template.Events
{
  

    public struct TapITableItemEvent : IEvent
    {
        public ITapableItem TapableItemItem;

        public TapITableItemEvent(ITapableItem tapableItemItem)
        {
            this.TapableItemItem = tapableItemItem;
        }
    }

    
    public class TapBoosterButtonEvent : IEvent
    {
        public BoosterType BoosterType;

        public TapBoosterButtonEvent(BoosterType boosterType)
        {
            BoosterType = boosterType;
        }
    }


    public class PlayOnButtonClickEvent : IEvent
    {
        public FailPopUpData FailPopUpData;

        public PlayOnButtonClickEvent(FailPopUpData failPopUpData)
        {
            FailPopUpData = failPopUpData;
        }
    }

    public class RewardPlayOnButtonClickEvent : IEvent
    {
        public FailPopUpData FailPopUpData;

        public RewardPlayOnButtonClickEvent(FailPopUpData failPopUpData)
        {
            FailPopUpData = failPopUpData;
        }
    }
    public class UseBoosterEvent : IEvent
    {
        public BoosterType BoosterType;

        public UseBoosterEvent(BoosterType boosterType)
        {
            BoosterType = boosterType;
        }
    }
}