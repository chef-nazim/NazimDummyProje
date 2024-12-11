using gs.chef.game.Scripts.Others;
using gs.chef.game.enums;
using gs.chef.game.Events;
using gs.chef.game.models;
using gs.chef.game.timer;
using gs.chef.onnectnext.timer;
using gs.chef.vcontainer.core.managers;
using MessagePipe;
using VContainer;

namespace gs.chef.game.Controllers
{
    public class LevelModelController : BaseSubscribable
    {
        #region Subscribe

        [Inject] private readonly ISubscriber<LevelModelCreatEvent> _levelModelCreatEventSubscriber;

        #endregion

        #region Publish

        [Inject] private readonly IPublisher<LevelModelCreatedEvent> _levelModelCreatedEventPublisher;
        [Inject] private readonly IPublisher<LevelFailEvent> _levelFailEventPublisher;
        
        #endregion

        
        LevelModel _levelModel;
        [Inject] private readonly GameModel _gameModel;
        [Inject] private readonly ChefTimerFactory _chefTimerFactory;
        [Inject] private readonly Containers _containers;
        public LevelModel LevelModel => _levelModel;

        protected override void Subscriptions()
        {
            _levelModelCreatEventSubscriber.Subscribe(s => LevelModelCreat(s));
        }

        private void LevelModelCreat(LevelModelCreatEvent levelModelCreatEvent)
        {
            _chefTimerFactory.RemoveTimer<LevelTimer>(LevelTimer.TimerName);
            _levelModel?.DisposeLevelModel();
            
            
            
            WrapperLevelData levelData = _gameModel.GetLevelData(_gameModel.Level);
            
            
            _levelModel = new LevelModel(levelData);
            _levelModelCreatedEventPublisher.Publish(new LevelModelCreatedEvent());
        }
    }
}