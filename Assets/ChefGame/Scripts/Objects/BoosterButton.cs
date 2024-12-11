using System.Collections.Generic;
using gs.chef.game.enums;
using UnityEngine;
using UnityEngine.UI;

namespace gs.chef.game.Scripts.Objects
{
    public class BoosterButton : MonoBehaviour
    {
        public Text LockText;
        public Button Button;
        
        
        [SerializeField] GameObject LockGameObject;
        [SerializeField] GameObject UnlockedGameObject;

        [SerializeField] int OpenLevel;
        [SerializeField] List<BoosterPopUpData> _boosterPopUpDatas = new List<BoosterPopUpData>();
        public BoosterType BoosterType;
        public Animator ButtonAnimator;
         [SerializeField] Image IconImage;
         [SerializeField] Image boosterBGImage;
         
        
        [SerializeField] Text _boosterCountText;
        
        [SerializeField] GameObject _countGameObject;
        [SerializeField] GameObject _palasGameObject;
        [SerializeField] Sprite _unusableBGSprite; 
        [SerializeField] Sprite _usableBGSprite; 

        private void Start()
        {
        
        }

        public void UpdateBoosterCount(int boosterCount)
        {
            if (boosterCount > 0)
            {
                _boosterCountText.text = boosterCount.ToString();
                _countGameObject.SetActive(true);
                _palasGameObject.SetActive(false);
               
            }
            else
            {
                _countGameObject.SetActive(false);
                _palasGameObject.SetActive(true);
            }
            
        }
        public void SetBoosterBG(bool isUsable)
        {
            if (isUsable)
            {
                boosterBGImage.sprite = _usableBGSprite;
                Button.interactable = true;
            }
            else
            {
                boosterBGImage.sprite = _unusableBGSprite;
                Button.interactable = false;
            }
        }

        public void SetPaymentText(int value, bool isOn)
        {
        }

        

        public void OpenCloseControl(int level)
        {
            
            if (level >= OpenLevel)
            {
                LockGameObject.SetActive(false);
                UnlockedGameObject.SetActive(true);
                var data = _boosterPopUpDatas.Find(x => x.BoosterType == BoosterType);

                if (data!=null)
                {
                    IconImage.sprite = data.Icon;
                }
                Button.interactable = true;
            }
            else
            {
                LockGameObject.SetActive(true);
                UnlockedGameObject.SetActive(false);
                Button.interactable = false;
                LockText.text = $"Level {OpenLevel}";
            }
        }
    }
}