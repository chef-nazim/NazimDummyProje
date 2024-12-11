using System;
using System.ComponentModel;
using Cysharp.Threading.Tasks;
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
using UnityEngine;
using VContainer;


namespace gs.chef.game.Scripts.Presenters
{
    public class GamePlayPresenter : BaseMenuPresenter<MenuNames, GamePlayViewData, GamePlayView>
    {
        #region Subscriptions

        [Inject] private readonly ISubscriber<LevelModelCreatedEvent> _levelModelCreatedEventSubscriber;
        //[Inject] private readonly ISubscriber<OpenBoosterBuyPopUpEvent> _openBoosterBuyPopUpEventSubscriber;
        #endregion

        #region Publish

        [Inject] private readonly IPublisher<OpenMenuEvent> _openMenuEventPublisher;
        [Inject] private readonly IPublisher<LevelDestroyEvent> _levelDestroyEventPublisher;
        [Inject] private readonly IPublisher<FeelingEvent> _feelingEventPublisher;
        [Inject] private readonly IPublisher<CreateGamePlaySceneEvent> _crateGamePlaySceneEventPublisher;
        [Inject] private readonly IPublisher<TapBoosterButtonEvent> _tapBoosterButtonEventPublisher;
        #endregion


        [Inject] private readonly GameModel _gameModel;
        [Inject] private readonly GameHelper _gameHelper;
        [Inject] private readonly Containers _containers;
        [Inject] private readonly IObjectResolver _resolver;

        [Inject] private readonly LevelModelController _levelModelController;

        
        public override MenuMode MenuMode => MenuMode.Single;
        public override MenuNames MenuName => MenuNames.GamePlayMenu;
        
        

        protected override void Subscriptions()
        {
            _levelModelCreatedEventSubscriber.Subscribe(s => LevelModelCreated());
           // _openBoosterBuyPopUpEventSubscriber.Subscribe(s=>OpenBuyBoosterPopUp(s));
        }

        private void LevelModelCreated()
        {
           
        }

        
        
        /*private void OpenBuyBoosterPopUp(OpenBoosterBuyPopUpEvent openDiceBuyPopUpEvent)
        {
            View.OpenBuyBoosterPopUp(openDiceBuyPopUpEvent.BoosterType ,_levelModelController.LevelModel,_gameModel);
        }*/
        protected override void OnShow(GamePlayViewData menuData)
        {
            _gameModel.PropertyChanged += OnPropertyChanged;
            View.OnSettingsButtonClick += SettingsButtonClick;
            View.BoosterPopUp.StartSettings();
            View.OnBoosterButtonClick += BoosterButtonClick;
            View.BoosterOpenClose(_gameModel);
            View.SetCoinText(_gameModel.CoinCount);
            View.SetLevelText(_gameModel.Level);
            View.OnNButtonClick += NButton;
            View.OnPButtonClick += PButton;
            View.BoosterPopUp.OnCloseButtonClicked += CloseBoosterPopUp;
            
            View.BoosterPopUp.OnBuyButtonClicked += BuyBooster;
            View.BoosterPopUp.OnRewardButtonClicked += RewardBooster;
            
        }

        private async void CloseBoosterPopUp()
        {
            View.BoosterPopUp.ClosePopUp();
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }

        private void PButton()
        {
            _gameModel.Level -= 1;
            if (_gameModel.Level < 1)
                _gameModel.Level = 1;
            _levelDestroyEventPublisher.Publish(new LevelDestroyEvent());
            _crateGamePlaySceneEventPublisher.Publish(new CreateGamePlaySceneEvent());
        }

        private void NButton()
        {
            _gameModel.Level += 1;
            _levelDestroyEventPublisher.Publish(new LevelDestroyEvent());
            _crateGamePlaySceneEventPublisher.Publish(new CreateGamePlaySceneEvent());
        }

        protected override void OnHide()
        {
            //_gameModel.PropertyChanged -= OnPropertyChanged;
            View.OnSettingsButtonClick -= SettingsButtonClick;
            View.BoosterPopUp.OnCloseButtonClicked -= CloseBoosterPopUp;
            View.OnBoosterButtonClick -= BoosterButtonClick;
            View.BoosterPopUp.OnBuyButtonClicked -= BuyBooster;
            View.BoosterPopUp.OnRewardButtonClicked -= RewardBooster;
        }


        #region Public Methods

        #endregion

        #region Private Methods
        private void BoosterButtonClick(BoosterButton obj)
        {
            if (!_levelModelController.LevelModel.IsLevelRunning) return;
            
            if (_gameModel.GetBoosterCount(obj.BoosterType) <= 0) //todo test bittiginde acilacak
            {
                View.BoosterPopUp.OpenPopUp(obj.BoosterType, _levelModelController.LevelModel, _gameModel);
            }
            else
            {
                _tapBoosterButtonEventPublisher.Publish(new TapBoosterButtonEvent(obj.BoosterType));
            }
        }
        private async void BuyBooster(BoosterType obj)
        {
            Debug.Log("BuyBooster");
            int price = GameHelper.GetBoosterPrice(obj);
            if (_gameModel.CoinCount >= price)
            {
                _gameModel.CoinCount -= price;
                _gameModel.AddBooster(obj);

                View.BoosterPopUp.ClosePopUp();
                
            }
            else
            {
                /*_levelModel.FeelEffect(new FeelingEvent
                {
                    SoundType = PlaySound.None,
                    HapticType = HapticPatterns.PresetType.Failure
                });*/
            }
        }

        private void RewardBooster(BoosterType obj)
        {
            _gameModel.AddBooster(obj);
            View.BoosterPopUp.ClosePopUp();
        }
        private void SettingsButtonClick()
        {
            if (_gameModel.Tutorial != -1) return;
            
            _feelingEventPublisher.Publish(new FeelingEvent
            {
                SoundType = PlaySound.ButtonClick,
                HapticType = HapticPatterns.PresetType.SoftImpact
            });
           
            
            _openMenuEventPublisher.Publish(new OpenMenuEvent(MenuNames.SettingsMenu,
                new SettingsMenuViewData(_gameModel.Sound, _gameModel.Haptic)));
        }

        #endregion

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_gameModel.CoinCount))
            {
                View.SetCoinText(_gameModel.CoinCount);
            }
            else if (e.PropertyName == nameof(_gameModel.Level))
            {
                View.SetLevelText(_gameModel.Level);
            }
            else if (e.PropertyName == nameof(_gameModel.Reload))
            {
                View.BoosterOpenClose(_gameModel);
            }
        }
    }
}