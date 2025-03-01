using System;
using gs.chef.onnectnext.timer;
using MessagePipe;
using NCG.template._NCG.Core.AllEvents;
using NCG.template._NCG.Core.BaseClass;
using NCG.template._NCG.Core.Model;
using NCG.template.EventBus;
using NCG.template.Events;
using NCG.template.extensions;
using NCG.template.models;
using NCG.template.Scripts.Others;
using NCG.template.timer;
using UnityEngine;
using VContainer;

namespace NCG.template.Controllers
{
    public class LevelModelController  : BaseController
    {
        public static LevelModelController _instance;
        LevelModel _levelModel;
        
        
        public LevelModel LevelModel => _levelModel;
        GameModel _gameModel => GameModel.Instance;
        
        public LevelModelController()
        {
            Debug.Log("LevelModelController Created");
            _instance = this;
        }
        
        public override void Initialize()
        {
            EventBus<LevelModelCreatEvent>.Subscribe(LevelModelCreat);
        }

        public override void Dispose()
        {
            EventBus<LevelModelCreatEvent>.Unsubscribe(LevelModelCreat);
        }
        
        private void LevelModelCreat(LevelModelCreatEvent levelModelCreatEvent)
        {
            //  _chefTimerFactory.RemoveTimer<LevelTimer>(LevelTimer.TimerName);
            _levelModel?.DisposeLevelModel();
            WrapperLevelData levelData = FileModel.GetLevelData(_gameModel.Level);
            _levelModel = new LevelModel(levelData);
            Debug.Log("LevelModel Created");
            EventBus<LevelModelCreatedEvent>.Publish(new LevelModelCreatedEvent());
        }

        
    }
}