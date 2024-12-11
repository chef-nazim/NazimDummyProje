using DG.Tweening;
using gs.chef.game.Scripts.Others;
using gs.chef.game.Scripts.ScriptableObjects;
using gs.chef.game.Scripts.State;
using gs.chef.game.Controllers;
using gs.chef.game.Events;
using gs.chef.game.enums;
using gs.chef.game.models;
using gs.chef.game.Objects;
using gs.chef.game.Views;
using gs.chef.vcontainer.core.managers;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace gs.chef.game.Managers
{
    public class LevelManager : BaseSubscribable
    {
        #region subscribe

        [Inject] private readonly ISubscriber<CreateGamePlaySceneEvent> _createGamePlaySceneEventSubscriber;

        [Inject] private readonly ISubscriber<LevelCompleteEvent> _levelCompleteEventSubscriber;
        [Inject] private readonly ISubscriber<LevelFailEvent> _levelFailEventSubscriber;
        [Inject] private readonly ISubscriber<RestartButtonClickEvent> _restartButtonClickEventSubscriber;
        [Inject] private readonly ISubscriber<LevelDestroyEvent> _levelDestroyEventSubscriber;
        [Inject] private readonly ISubscriber<PlayOnButtonClickEvent> _playOnButtonClickEventSubscriber;
        [Inject] private readonly ISubscriber<RewardPlayOnButtonClickEvent> _rewardPlayOnButtonClickEventSubscriber;
        [Inject] private readonly ISubscriber<TapBoosterButtonEvent> _tapBoosterButtonEventSubscriber;
        #endregion

        #region publish

        [Inject] private readonly IPublisher<LevelCreatedEvent> _levelCreatedEventPublisher;
        [Inject] private readonly IPublisher<CloseMenuEvent> _closeMenuEventPublisher;
        [Inject] private readonly IPublisher<OpenMenuEvent> _openMenuEventPublisher;
        [Inject] private readonly IPublisher<LevelDestroyEvent> _levelDestroyEventPublisher;
        [Inject] private readonly IPublisher<LevelModelCreatEvent> _levelModelCreatEventPublisher;
        [Inject] private readonly IPublisher<UseBoosterEvent> _useBoosterEventPublisher;
        #endregion


        [Inject] private readonly GameModel _gameModel;
        [Inject] private readonly PoolManager _poolManager;
        [Inject] readonly IObjectResolver _resolver;
        [Inject] private readonly LevelModelController _levelModelController;
        [Inject] private readonly GameHelper _gameHelper;
        [Inject] private readonly Containers _containers;

        protected override void Subscriptions()
        {
            _createGamePlaySceneEventSubscriber.Subscribe(CreateGamePlayScene);
            _restartButtonClickEventSubscriber.Subscribe(s => RestartButtonClick(s));
            _levelCompleteEventSubscriber.Subscribe(s => LevelComplete(s));
            _levelFailEventSubscriber.Subscribe(s => LevelFail(s));
            _levelDestroyEventSubscriber.Subscribe(s => LevelDestroy(s));
            _playOnButtonClickEventSubscriber.Subscribe(s => PlayOnButtonClick(s));
            _rewardPlayOnButtonClickEventSubscriber.Subscribe(s => RewardPlayOnButtonClick(s));
            _tapBoosterButtonEventSubscriber.Subscribe(s => TapBoosterButton(s));
        }
        private void TapBoosterButton(TapBoosterButtonEvent tapBoosterButtonEvent)
        {
            switch (tapBoosterButtonEvent.BoosterType)
            {
                case BoosterType.PlayOn:

                    if (CheckPayment() == true)
                    {
                        _closeMenuEventPublisher.Publish(new CloseMenuEvent(MenuNames.FailMenu));
                        AnalyticEventHelper.BoosterUsed(_gameModel.Level, tapBoosterButtonEvent.BoosterType.ToString());
                        _useBoosterEventPublisher.Publish(new UseBoosterEvent(tapBoosterButtonEvent.BoosterType));
                    }
                    return;
                 
            }

            _gameModel.UseBooster(tapBoosterButtonEvent.BoosterType);
            AnalyticEventHelper.BoosterUsed(_gameModel.Level, tapBoosterButtonEvent.BoosterType.ToString());
            _useBoosterEventPublisher.Publish(new UseBoosterEvent(tapBoosterButtonEvent.BoosterType));

            bool CheckPayment()
            {
                int price = GameHelper.GetBoosterPrice(tapBoosterButtonEvent.BoosterType);
                Debug.Log("price: " + price);
                if (_gameModel.CoinCount >= price)
                {
                    _gameModel.CoinCount -= price;
                    return true;
                }
                else
                {
                    /*_levelModelController.LevelModel.FeelEffect(new FeelingEvent
                    {
                        SoundType = PlaySound.None,
                        HapticType = HapticPatterns.PresetType.Failure
                    });*/
                    return false;
                }
            }
        }
        private void RewardPlayOnButtonClick(RewardPlayOnButtonClickEvent rewardPlayOnButtonClickEvent)
        {
            switch (rewardPlayOnButtonClickEvent.FailPopUpData.FailPopUpType)
            {
              
            }
        }

        private void PlayOnButtonClick(PlayOnButtonClickEvent playOnButtonClickEvent)
        {
            switch (playOnButtonClickEvent.FailPopUpData.FailPopUpType)
            {
              
            }
        }

        private void LevelDestroy(LevelDestroyEvent levelDestroyEvent)
        {
            _poolManager.DisposeAllPool();
        }

        private void LevelFail(LevelFailEvent levelFailEvent)
        {
            AnalyticEventHelper.LevelFailDesignEvent(_gameModel.Level, levelFailEvent.FailType.ToString());
            OpenMenuEvent menu = new OpenMenuEvent(MenuNames.FailMenu,
                new FailPanelViewData(levelFailEvent.FailType));
            _openMenuEventPublisher.Publish(menu);
        }

        private async void RestartButtonClick(RestartButtonClickEvent restartButtonClickEvent)
        {
            AnalyticEventHelper.LevelRestart(_gameModel.Level, restartButtonClickEvent.From.ToString());
            _levelDestroyEventPublisher.Publish(new LevelDestroyEvent());
            CreateLevel();
        }

        private void LevelComplete(LevelCompleteEvent levelCompleteEvent)
        {
            /*AnalyticEventHelper.LevelComplete(_gameModel.Level, (int)(_levelModelController.LevelModel.LevelTime - _levelModelController.LevelModel.LevelTimer.TimerModel.RemainingTime),
                (int)(_levelModelController.LevelModel.LevelTimer.TimerModel.RemainingTime), _gameModel.CoinCount);*/
            OpenMenuEvent menu = new OpenMenuEvent(MenuNames.WinMenu, new WinMenuViewData());
            _openMenuEventPublisher.Publish(menu);
        }

        private void CreateGamePlayScene(CreateGamePlaySceneEvent createGamePlaySceneEvent)
        {
            CreateLevel();
        }

        public void CreateLevel()
        {
            AnalyticEventHelper.LevelStart(_gameModel.Level);
           
            _levelModelCreatEventPublisher.Publish(new LevelModelCreatEvent());
            
            if (_levelModelController.LevelModel?._levelData.Seed != -1)
            {
                UnityEngine.Random.InitState(_levelModelController.LevelModel._levelData.Seed);
            }
            else
            {
                UnityEngine.Random.InitState(_gameModel.Level);
            }
            
            LevelProcess levelProcess = new LevelProcess(_resolver);
            levelProcess.OnProcessCompleted += process =>
            {
                _levelCreatedEventPublisher.Publish(new LevelCreatedEvent());
                DOVirtual.DelayedCall(0f, () =>
                {
                  _levelModelController.LevelModel.IsLevelRunning = true;
                  _levelModelController.LevelModel.IsInputActive = true;
                });
            };

            levelProcess.State = new LevelCreatState();
        }
    }
}