using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cysharp.Threading.Tasks;
using Lofelt.NiceVibrations;
using NCG.template._NCG.Core.AllEvents;
using NCG.template._NCG.Core.Model;
using NCG.template._NCG.Core.View;
using NCG.template.Controllers;
using NCG.template.enums;
using NCG.template.EventBus;
using NCG.template.Events;
using NCG.template.models;
using NCG.template.Scripts.Objects;
using NCG.template.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace NCG.template.Views
{
    public class GamePlayView : BaseView
    {
        #region UIElements

        public List<BoosterButton> _boosterButtons = new List<BoosterButton>();
        [SerializeField] Text _coinText;
        [SerializeField] Text _levelText;

        [SerializeField] private Button _settingButton;
        [SerializeField] private Button _N;
        [SerializeField] private Button _P;
        public BoosterPopUp BoosterPopUp;

        #endregion

        #region Actions

        public event Action OnSettingsButtonClick;
        public event Action OnNButtonClick;
        public event Action OnPButtonClick;
        public event Action<BoosterButton> OnBoosterButtonClick;

        #endregion
        
        GameModel _gameModel => GameModel.Instance;
        LevelModelController _levelModelController => LevelModelController._instance;

        protected override void OnEnable()
        {
            base.OnEnable();
            _settingButton.onClick.AddListener(SettingsButtonClick);
            _N.onClick.AddListener(NButton);
            _P.onClick.AddListener(PButton);
            BoosterPopUp.OnCloseButtonClicked += CloseBoosterPopUp;
            foreach (var booster in _boosterButtons)
            {
                booster.Button.onClick.AddListener(() => BoosterButtonClick(booster));
            }
            
            BoosterPopUp.OnBuyButtonClicked += BuyBooster;
            BoosterPopUp.OnRewardButtonClicked += RewardBooster;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _gameModel.PropertyChanged -= OnPropertyChanged;
            _settingButton.onClick.RemoveListener(SettingsButtonClick);
            _N.onClick.RemoveAllListeners();
            _P.onClick.RemoveAllListeners();
            foreach (var booster in _boosterButtons)
            {
                booster.Button.onClick.RemoveAllListeners();
            }
            BoosterPopUp.OnCloseButtonClicked -= CloseBoosterPopUp;
            BoosterPopUp.OnBuyButtonClicked -= BuyBooster;
            BoosterPopUp.OnRewardButtonClicked -= RewardBooster;
        }
        
        protected override void SubscribeEvents()
        {
            EventBus<LevelModelCreatedEvent>.Subscribe(LevelModelCreated);
        }

        
        protected override void UnSubscribeEvents()
        {
            EventBus<LevelModelCreatedEvent>.Unsubscribe(LevelModelCreated);
        }
        
        public override void Show(MenuData menuData)
        {
            _gameModel.PropertyChanged += OnPropertyChanged;
             BoosterPopUp.StartSettings();
             BoosterOpenClose();
             SetCoinText(_gameModel.CoinCount);
             SetLevelText(_gameModel.Level);
             
             View.SetActive(true);
        }

        public override void Hide()
        {
            
            View.SetActive(false);
        }
        
        
        
        

        
        private void LevelModelCreated(LevelModelCreatedEvent obj)
        {
            
        }
        

        #region Public Methods

        public void SetCoinText(int coinText)
        {
            _coinText.text = coinText.ToString();
        }

        public void SetLevelText(int level)
        {
            _levelText.text = $"Level {level}";
        }

        public void BoosterOpenClose()
        {
            foreach (var v in _boosterButtons)
            {
                v.OpenCloseControl(_gameModel.Level);
                v.UpdateBoosterCount(_gameModel.GetBoosterCount(v.BoosterType));
            }
        }
        #endregion

        #region Private Methods
        private void RewardBooster(BoosterType obj)
        {
            _gameModel.AddBooster(obj);
            BoosterPopUp.ClosePopUp();
        }

        private void SettingsButtonClick()
        {
            if (_gameModel.Tutorial != -1) return;
            
            EventBus<FeelingEvent>.Publish(new FeelingEvent
            {
                SoundType = PlaySound.ButtonClick,
                HapticType = HapticPatterns.PresetType.SoftImpact
            });
            
            EventBus<OpenMenuEvent>.Publish(new OpenMenuEvent(MenuNames.SettingsMenu, new SettingsMenuViewData()));
        }
        private async void CloseBoosterPopUp()
        {
            BoosterPopUp.ClosePopUp();
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }
        private void BoosterButtonClick(BoosterButton obj)
        {
            if (!_levelModelController.LevelModel.IsLevelRunning) return;
            
            if (_gameModel.GetBoosterCount(obj.BoosterType) <= 0) //todo test bittiginde acilacak
            {
                BoosterPopUp.OpenPopUp(obj.BoosterType, _levelModelController.LevelModel, _gameModel);
            }
            else
            {
                EventBus<TapBoosterButtonEvent>.Publish(new TapBoosterButtonEvent(obj.BoosterType));
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

                BoosterPopUp.ClosePopUp();
                
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
        private void PButton()
        {
            _gameModel.Level -= 1;
            if (_gameModel.Level < 1)
                _gameModel.Level = 1;
            EventBus<LevelDestroyEvent>.Publish(new LevelDestroyEvent());
            EventBus<CreateGamePlaySceneEvent>.Publish(new CreateGamePlaySceneEvent());
        }
        private void NButton()
        {
            _gameModel.Level += 1;
            EventBus<LevelDestroyEvent>.Publish(new LevelDestroyEvent());
            EventBus<CreateGamePlaySceneEvent>.Publish(new CreateGamePlaySceneEvent());
        }
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_gameModel.CoinCount))
            {
                SetCoinText(_gameModel.CoinCount);
            }
            else if (e.PropertyName == nameof(_gameModel.Level))
            {
                SetLevelText(_gameModel.Level);
            }
            else if (e.PropertyName == nameof(_gameModel.Reload))
            {
                BoosterOpenClose();
            }
        }
        #endregion
    }

    public class GamePlayViewData : MenuData
    {
        public GamePlayViewData()
        {
        }
    }
}