using System;
using System.Collections.Generic;
using gs.chef.game.enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace gs.chef.game.Scripts.Others
{
    [Serializable]
    public class SoundPackItem
    {
        public PlaySound SoundTyp;
        public AudioClip Clip;
    }
}