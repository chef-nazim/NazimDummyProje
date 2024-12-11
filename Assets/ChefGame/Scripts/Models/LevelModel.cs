using System;
using System.Collections.Generic;
using FluffyUnderware.Curvy;
using gs.chef.game.Scripts.Objects;
using UnityEngine;

namespace gs.chef.game.models
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