using Cysharp.Threading.Tasks;
using DG.Tweening;
using Lofelt.NiceVibrations;
using MessagePipe;
using NCG.template._NCG.Core.AllEvents;
using NCG.template._NCG.Core.BaseClass;
using NCG.template.Controllers;
using NCG.template.enums;
using NCG.template.EventBus;
using NCG.template.Events;
using NCG.template.models;
using NCG.template.Objects;
using NCG.template.Scripts.Others;
using NCG.template.Scripts.ScriptableObjects;
using NCG.template.Scripts.State;
using NCG.template.Views;
using UnityEngine;
using VContainer;

namespace NCG.template.Managers
{
    public class LevelManager : BaseManager
    {
        private GameModel _gameModel => GameModel.Instance;
        private LevelModelController _levelModelController => LevelModelController._instance;
        
        public static LevelManager Instance;

        
        public override void Initialize()
        {
            Instance = this;
            EventBus<CreateGamePlaySceneEvent>.Subscribe(CreateGamePlayScene);
            EventBus<RestartButtonClickEvent>.Subscribe(s => RestartButtonClick(s));
            EventBus<LevelCompleteEvent>.Subscribe(s => LevelComplete(s));
            EventBus<LevelFailEvent>.Subscribe(s => LevelFail(s));
            EventBus<LevelDestroyEvent>.Subscribe(s => LevelDestroy(s));
            EventBus<PlayOnButtonClickEvent>.Subscribe(s => PlayOnButtonClick(s));
            EventBus<RewardPlayOnButtonClickEvent>.Subscribe(s => RewardPlayOnButtonClick(s));
            EventBus<TapBoosterButtonEvent>.Subscribe(s => TapBoosterButton(s));
        }

        public override void Dispose()
        {
            EventBus<CreateGamePlaySceneEvent>.Unsubscribe(CreateGamePlayScene);
            EventBus<RestartButtonClickEvent>.Unsubscribe(s => RestartButtonClick(s));
            EventBus<LevelCompleteEvent>.Unsubscribe(s => LevelComplete(s));
            EventBus<LevelFailEvent>.Unsubscribe(s => LevelFail(s));
            EventBus<LevelDestroyEvent>.Unsubscribe(s => LevelDestroy(s));
            EventBus<PlayOnButtonClickEvent>.Unsubscribe(s => PlayOnButtonClick(s));
            EventBus<RewardPlayOnButtonClickEvent>.Unsubscribe(s => RewardPlayOnButtonClick(s));
            EventBus<TapBoosterButtonEvent>.Unsubscribe(s => TapBoosterButton(s));
        }


        private void TapBoosterButton(TapBoosterButtonEvent tapBoosterButtonEvent)
        {
            switch (tapBoosterButtonEvent.BoosterType)
            {
                case BoosterType.PlayOn:

                    if (CheckPayment() == true)
                    {
                        EventBus<CloseMenuEvent>.Publish(new CloseMenuEvent(MenuNames.FailMenu));
                        AnalyticEventHelper.BoosterUsed(_gameModel.Level, tapBoosterButtonEvent.BoosterType.ToString());
                        EventBus<UseBoosterEvent>.Publish(new UseBoosterEvent(tapBoosterButtonEvent.BoosterType));
                    }

                    return;
            }

            _gameModel.UseBooster(tapBoosterButtonEvent.BoosterType);
            AnalyticEventHelper.BoosterUsed(_gameModel.Level, tapBoosterButtonEvent.BoosterType.ToString());
            EventBus<UseBoosterEvent>.Publish(new UseBoosterEvent(tapBoosterButtonEvent.BoosterType));

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
                    EventBus<FeelingEvent>.Publish(new FeelingEvent
                    {
                        SoundType = PlaySound.None,
                        HapticType = HapticPatterns.PresetType.Failure
                    });
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
        }

        private void LevelFail(LevelFailEvent levelFailEvent)
        {
            AnalyticEventHelper.LevelFailDesignEvent(_gameModel.Level, levelFailEvent.FailType.ToString());
            OpenMenuEvent menu = new OpenMenuEvent(MenuNames.FailMenu, new FailPanelViewData(levelFailEvent.FailType));
            EventBus<OpenMenuEvent>.Publish(menu);
        }

        private async void RestartButtonClick(RestartButtonClickEvent restartButtonClickEvent)
        {
            AnalyticEventHelper.LevelRestart(_gameModel.Level, restartButtonClickEvent.From.ToString());
            EventBus<LevelDestroyEvent>.Publish(new LevelDestroyEvent());
            CreateLevel();
        }

        private void LevelComplete(LevelCompleteEvent levelCompleteEvent)
        {
            AnalyticEventHelper.LevelComplete(_gameModel.Level);
            OpenMenuEvent menu = new OpenMenuEvent(MenuNames.WinMenu, new WinMenuViewData());

            EventBus<OpenMenuEvent>.Publish(menu);
        }

        private void CreateGamePlayScene(CreateGamePlaySceneEvent createGamePlaySceneEvent)
        {
            CreateLevel();
        }

        public void CreateLevel()
        {
            AnalyticEventHelper.LevelStart(_gameModel.Level);

            EventBus<LevelModelCreatEvent>.Publish(new LevelModelCreatEvent());

            if (_levelModelController.LevelModel?._levelData.Seed != -1)
            {
                UnityEngine.Random.InitState(_levelModelController.LevelModel._levelData.Seed);
            }
            else
            {
                UnityEngine.Random.InitState(_gameModel.Level);
            }

            LevelProcess levelProcess = new LevelProcess();
            levelProcess.OnProcessCompleted += process =>
            {
                EventBus<LevelCreatedEvent>.Publish(new LevelCreatedEvent());
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