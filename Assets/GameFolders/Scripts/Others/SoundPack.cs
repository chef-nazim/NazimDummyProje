using System;
using System.Collections.Generic;
using NCG.template.enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace NCG.template.Scripts.Others
{
    [Serializable]
    public class SoundPackItem
    {
        public PlaySound SoundTyp;
        public AudioClip Clip;
    }
}