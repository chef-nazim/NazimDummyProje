using System;
using gs.chef.bakerymerge.Objects;
using gs.chef.game.enums;
using gs.chef.vcontainer.menu;
using UnityEngine;
using UnityEngine.UI;

namespace gs.chef.game.Views
{
     public class FailPanelView : BaseMenuView
    {
        [SerializeField] Button _restartButton;
        [SerializeField] FailPopUp _failPopUp;
        
        public event Action<FailPopUpData> OnRestartButtonClick;
        public event Action<FailPopUpData> OnPlayOnButtonClick;
        
        public event Action<FailPopUpData> OnRewardButtonClicked; 

        private void Start()
        {
            _failPopUp.OnRestartButtonClick += RestartButtonClick;
            _failPopUp.OnPlayOnButtonClick += PlayOnButtonClick;
            _failPopUp.OnRewardButtonClicked += RewardButtonClicked;
        }

        private void RewardButtonClicked(FailPopUpData obj)
        {
            OnRewardButtonClicked?.Invoke(obj);
        }

        private void PlayOnButtonClick(FailPopUpData obj)
        {
            OnPlayOnButtonClick?.Invoke(obj);
        }

        private void RestartButtonClick(FailPopUpData obj)
        {
            OnRestartButtonClick?.Invoke(obj);
        }
        

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