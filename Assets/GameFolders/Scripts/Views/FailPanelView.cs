using System;
using gs.chef.bakerymerge.Objects;
using Lofelt.NiceVibrations;
using NCG.template._NCG.Core.AllEvents;
using NCG.template._NCG.Core.Model;
using NCG.template._NCG.Core.View;
using NCG.template.enums;
using NCG.template.EventBus;
using NCG.template.Events;
using UnityEngine;
using UnityEngine.UI;

namespace NCG.template.Views
{
     public class FailPanelView : BaseView
    {
        [SerializeField] Button _restartButton;
        [SerializeField] FailPopUp _failPopUp;
     

       
        

        public void SetFailType(FailType menuDataFailType,bool isHaveCoin,int price)
        {
            switch (menuDataFailType)
            {
                case FailType.TimeAndSlot:
                    _failPopUp.OpenPopUp(FailPopUpType.TimeAndSlotReward,isHaveCoin,price);
                    break;
                case FailType.SlotIsFull:
                    _failPopUp.OpenPopUp(FailPopUpType.SlotIsFullReward,isHaveCoin,price);
                    break;
                case FailType.TimeIsOut:
                     _failPopUp.OpenPopUp(FailPopUpType.TimeIsOutReward,isHaveCoin,price);
                    break;
                case FailType.NoPossibleMove:
                    _failPopUp.OpenPopUp(FailPopUpType.NoPossibleMove,isHaveCoin,price);
                    break;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _failPopUp.OnRestartButtonClick += RestartButtonClick;
            _failPopUp.OnPlayOnButtonClick += PlayOnButtonClick;
            _failPopUp.OnRewardButtonClicked += RewardButtonClicked;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _failPopUp.OnRestartButtonClick -= RestartButtonClick;
            _failPopUp.OnPlayOnButtonClick -= PlayOnButtonClick;
            _failPopUp.OnRewardButtonClicked -= RewardButtonClicked;
        }

        protected override void SubscribeEvents()
        {
            
        }

        protected override void UnSubscribeEvents()
        {
            
        }

        public override void Show(MenuData menuData)
        {
            var failPanelViewData =  (menuData as FailPanelViewData);
            EventBus<FeelingEvent>.Publish(new FeelingEvent
            {
                SoundType = PlaySound.Fail,
                HapticType = HapticPatterns.PresetType.Failure
            });
            
            bool isHaveCoin;
            FailType failType ;
            int playOnPrice;

            switch (failPanelViewData.FailType)
            {
                case FailType.NoPossibleMove:
                    failType = FailType.NoPossibleMove;
                    //isHaveCoin = _gameModel.Coin >= _gameHelper.TimeAndSlotFailPrice;
                    isHaveCoin = false;
                    //playOnPrice = _gameHelper.TimeAndSlotFailPrice;
                    playOnPrice = 100;
                    break;
                default:
                    return;
                    break;
            }
            SetFailType(failType,isHaveCoin,playOnPrice);
          
            View.SetActive(true);
        }

        public override void Hide()
        {
            
            View.SetActive(false);
        }
        
        
        private void RewardButtonClicked(FailPopUpData obj)
        {
            EventBus<RewardPlayOnButtonClickEvent>.Publish(new RewardPlayOnButtonClickEvent(obj));
            EventBus<FeelingEvent>.Publish(new FeelingEvent
            {
                SoundType = PlaySound.ButtonClick,
                HapticType = HapticPatterns.PresetType.SoftImpact
            });
        }

        private void PlayOnButtonClick(FailPopUpData obj)
        {
            EventBus<PlayOnButtonClickEvent>.Publish(new PlayOnButtonClickEvent(obj));
            EventBus<FeelingEvent>.Publish(new FeelingEvent
            {
                SoundType = PlaySound.ButtonClick,
                HapticType = HapticPatterns.PresetType.SoftImpact
            });
        }

        private void RestartButtonClick(FailPopUpData obj)
        {
            EventBus<FeelingEvent>.Publish(new FeelingEvent
            {
                SoundType = PlaySound.ButtonClick,
                HapticType = HapticPatterns.PresetType.SoftImpact
            });
            EventBus<CloseMenuEvent>.Publish(new CloseMenuEvent(MenuNames.FailMenu));
            switch (obj.FailPopUpType)
            {
                case FailPopUpType.NoPossibleMove:
                    EventBus<RestartButtonClickEvent>.Publish(new RestartButtonClickEvent( RestartFrom.NoPossibleMoveFailView));
                    break;
            }

        }
    }

    public class FailPanelViewData : MenuData
    {
        
        public FailType FailType;
        public FailPanelViewData(FailType failType)
        {
            FailType = failType;
        }
    }

    
}