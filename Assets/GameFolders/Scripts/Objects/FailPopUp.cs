using System;
using System.Collections.Generic;
using DG.Tweening;
using NCG.template.Scripts.ScriptableObjects;
using NCG.template.enums;
using NCG.template.models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace gs.chef.bakerymerge.Objects
{
    public class FailPopUp : MonoBehaviour
    {
        [SerializeField] GameObject _failPopUp;

        [SerializeField] Button _restartButton;
        [SerializeField] RewardButton _rewardButton;
        [SerializeField] Button _playOnButton;
        [SerializeField] Button _closeButton;

        [SerializeField] private Image DescriptionTitleImage;
        [SerializeField] private Image IconImage;
        [SerializeField] private TextMeshProUGUI DescriptionText;
        [SerializeField] private Text _priceText;

        public List<FailPopUpData> FailPopUpDatas = new List<FailPopUpData>();

        private FailPopUpData _currentFailPopUpData;
        private bool _isHaveCoin;
        private int _price;

        public event Action<FailPopUpData>OnPlayOnButtonClick;
        public event Action<FailPopUpData> OnRestartButtonClick;
        
        public event Action<FailPopUpData> OnRewardButtonClicked; 


        private void Start()
        {
            _closeButton.onClick.AddListener(CloseButtonClick);
            _restartButton.onClick.AddListener(RestartButtonClick);
            _playOnButton.onClick.AddListener(PlayOnButtonClick);
            _rewardButton.OnRewardButtonClicked += RewardButtonOnRewardButtonClicked;
        }

        public void OpenPopUp(FailPopUpType failPopUpType, bool isHaveCoin, int price)
        {
            _rewardButton.SetStartSettings(true);
            _failPopUp.transform.localScale = Vector3.zero;
            var failPopUpData = FailPopUpDatas.Find(x => x.FailPopUpType == failPopUpType);
            
            if (failPopUpData != null)
            {
                _currentFailPopUpData = failPopUpData;
                _isHaveCoin = isHaveCoin;
                _price = price;
                DescriptionTitleImage.sprite = failPopUpData.descriptionTitleSprite;
                IconImage.sprite = failPopUpData.iconSprite;
                DescriptionText.text = failPopUpData.descriptionText;
                _priceText.text = price.ToString();

                _closeButton.gameObject.SetActive(failPopUpData.IsClose);
                _restartButton.gameObject.SetActive(failPopUpData.IsRestart);

                
                
                
                if (failPopUpData.IsRewarded)
                {
                    if (isHaveCoin)
                    {
                        _playOnButton.gameObject.SetActive(true);
                        _rewardButton.gameObject.SetActive(false);
                    }
                    else
                    {
                        _playOnButton.gameObject.SetActive(false);
                        _rewardButton.gameObject.SetActive(true);
                    }
                }
                else
                {
                    _playOnButton.gameObject.SetActive(false);
                    _rewardButton.gameObject.SetActive(false);
                }
                
                
            }

            _failPopUp.SetActive(true);

            _failPopUp.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }


        private void RewardButtonOnRewardButtonClicked()
        {
            OnRewardButtonClicked?.Invoke(_currentFailPopUpData);
            _rewardButton.SetCloseSettings();
        }

        private void PlayOnButtonClick()
        { 
            OnPlayOnButtonClick?.Invoke(_currentFailPopUpData);
            _rewardButton.SetCloseSettings();
        }

        private void RestartButtonClick()
        {
            OnRestartButtonClick?.Invoke(_currentFailPopUpData);
            _rewardButton.SetCloseSettings();
        }

        private void CloseButtonClick()
        {
            _rewardButton.SetCloseSettings();
            _failPopUp.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                if (_currentFailPopUpData.NextFailPopUpType != FailPopUpType.None)
                {
                    OpenPopUp(_currentFailPopUpData.NextFailPopUpType, _isHaveCoin,_price);
                }
                else
                {
                    _failPopUp.SetActive(false);
                }
            });
        }
    }

    [Serializable]
    public class FailPopUpData
    {
        public FailPopUpType FailPopUpType;
        public bool IsRewarded;
        public bool IsRestart;
        public bool IsClose;
        public FailPopUpType NextFailPopUpType;

        public Sprite descriptionTitleSprite;
        public Sprite iconSprite;
        public string descriptionText;
    }

    public enum FailPopUpType
    {
        None,
        SlotIsFullReward,
        SlotIsFull,
        TimeIsOutReward,
        TimeIsOut,
        TimeAndSlotReward,
        TimeAndSlot,
        NoPossibleMove
    }
}