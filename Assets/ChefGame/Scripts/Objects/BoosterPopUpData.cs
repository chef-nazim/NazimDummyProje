using System;
using gs.chef.game.enums;
using UnityEngine;

namespace gs.chef.game.Scripts.Objects
{
    [Serializable]
    public class BoosterPopUpData
    {
        public BoosterType BoosterType;
        public Sprite Icon;
        public Sprite BoosterName;
        public Sprite CountSprite;
        public String Description;
    }
}