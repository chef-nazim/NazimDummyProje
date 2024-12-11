using System;
using System.Collections.Generic;
using gs.chef.bakerymerge.Objects;
using gs.chef.game.Scripts.ScriptableObjects;
using gs.chef.game.enums;
using gs.chef.game.models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace gs.chef.game.Scripts.Objects
{
    public class BoosterPopUp : MonoBehaviour
    {
        [SerializeField] GameObject _popUpGO;
        [SerializeField] GameObject _popUpBg;
        [SerializeField] Button CloseButton;
        [SerializeField] Button BuyButton;
        [SerializeField] RewardButton RewardedButton;
        [SerializeField] List<BoosterPopUpData> _boosterPopUpDatas = new List<BoosterPopUpData>();
        
        [SerializeField] Image _iconImage;
        [SerializeField] Image _boosterNameImage;
        [SerializeField] TextMeshProUGUI _descriptionText;
        [SerializeField] Image _countImage;
        [SerializeField] Text _priceText;
         
        BoosterPopUpData _boosterPopUpData=null;
        
        [SerializeField] Sprite _openBuyButtonSprite;
        [SerializeField] Sprite _closeButtonSprite;
        [SerializeField] Image _buyButtonImage;
        
        
        LevelModel _levelModel;
        GameModel _gameModel;
        public event Action OnCloseButtonClicked;
        public event Action<BoosterType> OnBuyButtonClicked;
        public event Action<BoosterType>  OnRewardButtonClicked;
        
        

        public void StartSettings()
        {
            _popUpGO.SetActive(false);
            _popUpBg.SetActive(false);
            CloseButton.onClick.AddListener(CloseAction);
            BuyButton.onClick.AddListener(() =>
            {

                if (_boosterPopUpData != null)
                {
                    OnBuyButtonClicked?.Invoke(_boosterPopUpData.BoosterType);    
                }
            });
        }
        
        public void OpenPopUp(BoosterType boosterType,LevelModel levelModel,GameModel gameModel)
        {
            _levelModel=levelModel;
            _gameModel = gameModel;
            var data = _boosterPopUpDatas.Find(x => x.BoosterType == boosterType);
            _boosterPopUpData = data;
            _iconImage.sprite = data.Icon;
            _boosterNameImage.sprite = data.BoosterName;
            _descriptionText.text = data.Description;
            _countImage.sprite = data.CountSprite;
            _priceText.text = GameHelper.GetBoosterPrice(boosterType).ToString();
           
           // LionAds.OnRewardedStatusChanged += RewardedStatusChanged;
            RewardedButton.OnRewardButtonClicked += RewardButtonClicked; 
            //RewardedButton.SetStartSettings(LionAds.IsRewardedReady);
            RewardedButton.SetStartSettings(true);
            
            
            if (_gameModel.CoinCount>=GameHelper.GetBoosterPrice(boosterType))
            {
                BuyButton.interactable = true;
                _buyButtonImage.sprite = _openBuyButtonSprite;
            }
            else
            {
                _buyButtonImage.sprite = _closeButtonSprite;
                BuyButton.interactable = false;
            }

            
            _popUpGO.SetActive(true);
            _popUpBg.SetActive(true);
        }

        private void RewardButtonClicked()
        {
            //LionAds.TryShowRewarded(_boosterPopUpData.BoosterType.ToString(), RewardButtonClickedTrigger, CloseAction);
            RewardButtonClickedTrigger();
            //AnalyticEventHelper.RVShow(_boosterPopUpData.BoosterType,_levelModel.GameModel.Level);
        }
        
        public void RewardButtonClickedTrigger()
        {
            OnRewardButtonClicked?.Invoke(_boosterPopUpData.BoosterType);
        }
        public void CloseAction()
        {
            OnCloseButtonClicked?.Invoke();
        }
        private void RewardedStatusChanged(bool isReady)
        {
            RewardedButton.StatusChanged(isReady);
        }
        
        public void ClosePopUp()
        {
          //  LionAds.OnRewardedStatusChanged -= RewardedStatusChanged;
            RewardedButton.OnRewardButtonClicked -= RewardButtonClicked;
            RewardedButton.SetCloseSettings();
            _boosterPopUpData = null;
            _popUpGO.SetActive(false);
            _popUpBg.SetActive(false);
        }
    }
}