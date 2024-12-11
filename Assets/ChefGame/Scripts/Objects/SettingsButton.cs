using UnityEngine;
using UnityEngine.UI;

namespace gs.chef.game.Scripts.Objects
{
    public class SettingsButton : MonoBehaviour
    {
        public Button button;
        public Image image;
        public Sprite on;
        public Sprite off;
        
        public Image imageBG;
        public Sprite onBG;
        public Sprite offBG;
        public void SetSprite(bool isOn)
        {
            if (isOn)
            {
                image.sprite = on;
                imageBG.sprite = onBG;
            }
            else
            {
                image.sprite = off;
                imageBG.sprite = offBG;
            }
        }
    }
}