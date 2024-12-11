using gs.chef.game.Controllers;
using gs.chef.game.enums;
using gs.chef.game.Events;
using gs.chef.game.models;
using gs.chef.game.Scripts.Objects;
using gs.chef.game.Scripts.Others;
using gs.chef.game.Scripts.ScriptableObjects;
using gs.chef.game.Views;
using gs.chef.vcontainer.menu;
using Lofelt.NiceVibrations;
using MessagePipe;
using VContainer;

namespace gs.chef.game.Scripts.Presenters
{
    public class SettingsMenuPresenter : BaseMenuPresenter<MenuNames, SettingsMenuViewData, SettingsMenuView>
    {
        public override MenuMode MenuMode => MenuMode.Additive;
        public override MenuNames MenuName => MenuNames.SettingsMenu;

        #region Subscriber

        #endregion

        #region Publisher

        [Inject] private readonly IPublisher<CloseMenuEvent> _closeMenuPublisher;
        [Inject] private readonly IPublisher<RestartButtonClickEvent> _restartButtonClickEventPublisher;
        [Inject] private readonly IPublisher<FeelingEvent> _feelingEventPublisher;

        #endregion


        [Inject] private readonly GameModel _gameModel;
        [Inject] private readonly LevelModelController _levelModelController;
        [Inject] private readonly Containers _containers;
        [Inject] private readonly GameHelper _gameHelper;

        protected override void OnShow(SettingsMenuViewData menuData)
        {
           

            View.OnSettingsButtonClicked += ButtonClicked;
            View.OnSettingsCloseButtonClicked += CloseButtonClicked;
            View.OnRestartButtonClicked += RestartButtonControl;
           
            ButtonControl();
        }

        private void RestartButtonControl()
        {
            AnalyticEventHelper.LevelFailDesignEvent(_gameModel.Level, FailType.RestartButton.ToString());
            _closeMenuPublisher.Publish(new CloseMenuEvent(MenuNames.SettingsMenu));
            _feelingEventPublisher.Publish(new FeelingEvent
            {
                SoundType = PlaySound.ButtonClick,
                HapticType = HapticPatterns.PresetType.SoftImpact
            });
            _restartButtonClickEventPublisher.Publish(new RestartButtonClickEvent(RestartFrom.GamePlayView));
        }


        protected override void Subscriptions()
        {
        }


        private void CloseButtonClicked()
        {
            _closeMenuPublisher.Publish(new CloseMenuEvent(MenuNames.SettingsMenu));
            
            _feelingEventPublisher.Publish(new FeelingEvent
            {
                SoundType = PlaySound.ButtonClick,
                HapticType = HapticPatterns.PresetType.SoftImpact
            });
        }


        private void ButtonClicked(SettingsButton button)
        {
            if (Equals(button, View.SoundButton))
            {
                _gameModel.Sound = (_gameModel.Sound == 1) ? 0 : 1;
            }
            else if (Equals(button, View.HapticButton))
            {
                _gameModel.Haptic = (_gameModel.Haptic == 1) ? 0 : 1;
            }

            ButtonControl();
        }

        void ButtonControl()
        {
            View.ButtonControl(new SettingsMenuViewData(_gameModel.Sound, _gameModel.Haptic));
        }

        protected override void OnHide()
        {
           
            View.OnSettingsButtonClicked -= ButtonClicked;
            View.OnSettingsCloseButtonClicked -= CloseButtonClicked;
            View.OnRestartButtonClicked -= RestartButtonControl;
            
        }
    }
}