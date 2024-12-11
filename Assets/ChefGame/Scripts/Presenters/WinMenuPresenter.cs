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
    public class WinMenuPresenter : BaseMenuPresenter<MenuNames, WinMenuViewData, WinMenuView>
    {
        [Inject] private readonly GameModel _gameModel;
        [Inject] private readonly GameHelper _gameHelper;
        [Inject] private readonly IPublisher<OpenMenuEvent> _openMenuEventPublisher;
        [Inject] private readonly IPublisher<CloseMenuEvent> _closeMenuEventPublisher;
        [Inject] private readonly IPublisher<LevelDestroyEvent> _levelDestroyEventPublisher;
        [Inject] private readonly IPublisher<FeelingEvent> _feelingEventPublisher;
        [Inject] private readonly IPublisher<CreateGamePlaySceneEvent> _crateGamePlaySceneEventPublisher;
        [Inject] private readonly ISubscriber<LevelModelCreatedEvent> _levelModelCreatedEventSubscriber;

        [Inject] private readonly Containers _containers;
        
        public override MenuMode MenuMode => MenuMode.Additive;
        public override MenuNames MenuName => MenuNames.WinMenu;
        
        protected override async void OnShow(WinMenuViewData menuData)
        {
            View.OnNextLevelButtonClick += NextLevelButtonClick;
            
          
            _feelingEventPublisher.Publish(new FeelingEvent()
            {
                HapticType = HapticPatterns.PresetType.Success,
                SoundType = PlaySound.Win
            });
            
           
          
            View.SetCoinText(_gameHelper.LevelCompleteCoin);
            View.PlayPanelOpeningAnimation();
        }

        private void NextLevelButtonClick()
        {
            _gameModel.Level += 1;
            _feelingEventPublisher.Publish(new FeelingEvent
            {
                SoundType = PlaySound.ButtonClick,
                HapticType = HapticPatterns.PresetType.SoftImpact
            });
            View.KillPanelOpeningAnimation();
            _levelDestroyEventPublisher.Publish(new LevelDestroyEvent());
            _gameModel.AddCoin(_gameHelper.LevelCompleteCoin);
            _crateGamePlaySceneEventPublisher.Publish(new CreateGamePlaySceneEvent());
        }


        protected override void OnHide()
        {
            View.OnNextLevelButtonClick -= NextLevelButtonClick;
        }

        protected override void Subscriptions()
        {
            
        }

        
    }
}