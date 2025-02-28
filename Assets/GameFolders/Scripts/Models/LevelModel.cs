using System;
using System.Collections.Generic;
using FluffyUnderware.Curvy;
using NCG.template._NCG.Core.Model;
using NCG.template.Scripts.Objects;
using UnityEngine;

namespace NCG.template.models
{
    [Serializable]
    public class LevelModel
    {
        public readonly WrapperLevelData _levelData;

        public bool IsLevelRunning { get; set; }
        public bool IsInputActive { get; set; }


        public LevelModel(WrapperLevelData levelData)
        {
            _levelData = levelData;

            IsLevelRunning = false;
            IsInputActive = false;
        }
        
        public void DisposeLevelModel()
        {
        }
    }
}