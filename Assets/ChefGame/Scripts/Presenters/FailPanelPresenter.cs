using gs.chef.bakerymerge.Objects;
using gs.chef.game.Controllers;
using gs.chef.game.enums;
using gs.chef.game.Events;
using gs.chef.game.models;
using gs.chef.game.Scripts.Others;
using gs.chef.game.Scripts.ScriptableObjects;
using gs.chef.game.Views;
using gs.chef.vcontainer.menu;
using Lofelt.NiceVibrations;
using MessagePipe;
using VContainer;

namespace gs.chef.game.Scripts.Presenters
{
    public class FailPanelPresenter : BaseMenuPresenter<MenuNames, FailPanelViewData, FailPanelView>
    {
        public override MenuMode MenuMode => MenuMode.Additive;
        public override MenuNames MenuName => MenuNames.FailMenu;
        [Inject] private readonly IPublisher<RestartButtonClickEvent> _restartButtonClickEventPublisher;
        [Inject] private readonly IPublisher<CloseMenuEvent> _closeMenuEventPublisher;
        [Inject] private readonly IPublisher<PlayOnButtonClickEvent> _playOnButtonClickEventPublisher;
        [Inject] private readonly IPublisher<RewardPlayOnButtonClickEvent> _rewardPlayOnButtonClickEventPublisher;
        [Inject] private readonly IPublisher<FeelingEvent> _feelingEventPublisher;
            
        [Inject] private readonly LevelModelController _levelModelController;
        [Inject] private readonly GameHelper _gameHelper;
        [Inject] private readonly GameModel _gameModel;
        [Inject] private readonly Containers _containers;
        
        protected override void OnShow(FailPanelViewData menuData)
        {
             
            
            _feelingEventPublisher.Publish(new FeelingEvent
            {
                SoundType = PlaySound.Fail,
                HapticType = HapticPatterns.PresetType.Failure
            });
            
            bool isHaveCoin;
            FailType failType ;
            int playOnPrice;

            switch (menuData.FailType)
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
            View.SetFailType(failType,isHaveCoin,playOnPrice);
            View.OnRestartButtonClick += RestartButtonClick;
            View.OnPlayOnButtonClick += PlayOnButtonClick;
            View.OnRewardButtonClicked += RewardButtonClicked;
            
        }
        protected override void OnHide()
        {
            View.OnRestartButtonClick -= RestartButtonClick;
            View.OnPlayOnButtonClick -= PlayOnButtonClick;
            View.OnRewardButtonClicked -= RewardButtonClicked;
        }

        private void RewardButtonClicked(FailPopUpData obj)
        {
            _rewardPlayOnButtonClickEventPublisher.Publish(new RewardPlayOnButtonClickEvent(obj));
            _feelingEventPublisher.Publish(new FeelingEvent
            {
                SoundType = PlaySound.ButtonClick,
                HapticType = HapticPatterns.PresetType.SoftImpact
            });
        }

        private void PlayOnButtonClick(FailPopUpData obj)
        {
            _playOnButtonClickEventPublisher.Publish(new PlayOnButtonClickEvent(obj));
            _feelingEventPublisher.Publish(new FeelingEvent
            {
                SoundType = PlaySound.ButtonClick,
                HapticType = HapticPatterns.PresetType.SoftImpact
            });
        }

        

        private void RestartButtonClick(FailPopUpData obj)
        {
            
            _feelingEventPublisher.Publish(new FeelingEvent
            {
                SoundType = PlaySound.ButtonClick,
                HapticType = HapticPatterns.PresetType.SoftImpact
            });
            _closeMenuEventPublisher.Publish(new CloseMenuEvent(MenuNames.FailMenu));
            switch (obj.FailPopUpType)
            {
                case FailPopUpType.NoPossibleMove:
                    _restartButtonClickEventPublisher.Publish(new RestartButtonClickEvent( RestartFrom.NoPossibleMoveFailView));
                    break;
            }
            
        }
    }
}