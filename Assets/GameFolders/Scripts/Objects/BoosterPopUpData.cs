using System;
using NCG.template.enums;
using UnityEngine;

namespace NCG.template.Scripts.Objects
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