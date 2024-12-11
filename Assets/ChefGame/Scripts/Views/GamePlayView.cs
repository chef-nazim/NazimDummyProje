using System;
using System.Collections.Generic;
using gs.chef.game.enums;
using gs.chef.game.models;
using gs.chef.game.Scripts.Objects;
using gs.chef.vcontainer.menu;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace gs.chef.game.Views
{
    public class GamePlayView : BaseMenuView
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


        private void Start()
        {
            _settingButton.onClick.AddListener(SettingsButtonClick);
            _N.onClick.AddListener(() => OnNButtonClick?.Invoke());
            _P.onClick.AddListener(() => OnPButtonClick?.Invoke());
            
            foreach (var booster in _boosterButtons)
            {
                booster.Button.onClick.AddListener(() => OnBoosterButtonClick?.Invoke(booster));
            }
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
        
        public void BoosterOpenClose( GameModel gameModel)
        {
            foreach (var v in _boosterButtons)
            {
                v.OpenCloseControl(gameModel.Level);
                v.UpdateBoosterCount(gameModel.GetBoosterCount(v.BoosterType));
            }
        }

        
        #endregion

        #region Private Methods

        private void SettingsButtonClick()
        {
            OnSettingsButtonClick?.Invoke();
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