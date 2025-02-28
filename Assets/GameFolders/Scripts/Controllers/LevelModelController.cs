using System;
using gs.chef.onnectnext.timer;
using MessagePipe;
using NCG.template._NCG.Core.AllEvents;
using NCG.template._NCG.Core.Model;
using NCG.template.EventBus;
using NCG.template.Events;
using NCG.template.extensions;
using NCG.template.models;
using NCG.template.Scripts.Others;
using NCG.template.timer;
using VContainer;

namespace NCG.template.Controllers
{
    public class LevelModelController 
    {
        public static LevelModelController _instance;
        
        public LevelModelController()
        {
            _instance = this;
            
        }
        
        #region Subscribe

        EventBinding<LevelModelCreatEvent> _levelModelCreatEventSubscriber;

        #endregion

        #region Publish
        // create prop eventbus levelmodelcreatedevent
             
        #endregion
        // create prop eventbus from publisher levelmodelcreatedevent

        
        LevelModel _levelModel;

        [Inject] private readonly GameModel _gameModel;

        //[Inject] private readonly ChefTimerFactory _chefTimerFactory;
        [Inject] private readonly Containers _containers;
        public LevelModel LevelModel => _levelModel;

        private void OnEnable()
        {
            _levelModelCreatEventSubscriber = new EventBinding<LevelModelCreatEvent>(s => LevelModelCreat(s));
        }


        private void LevelModelCreat(LevelModelCreatEvent levelModelCreatEvent)
        {
            //  _chefTimerFactory.RemoveTimer<LevelTimer>(LevelTimer.TimerName);
            _levelModel?.DisposeLevelModel();


            WrapperLevelData levelData = FileModel.GetLevelData(_gameModel.Level);


            _levelModel = new LevelModel(levelData);

            EventBus<LevelModelCreatedEvent>.Publish(new LevelModelCreatedEvent());
        }
    }
}