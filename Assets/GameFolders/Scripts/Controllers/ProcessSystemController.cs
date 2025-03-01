using MessagePipe;
using NCG.template._NCG.Core.AllEvents;
using NCG.template._NCG.Core.BaseClass;
using NCG.template.enums;
using NCG.template.EventBus;
using NCG.template.Events;
using NCG.template.Managers;
using NCG.template.models;
using NCG.template.Objects;
using NCG.template.Scripts.Others;
using NCG.template.Scripts.State;


namespace NCG.template.Controllers
{
    public class ProcessSystemController : BaseController
    {
        GameModel _gameModel => GameModel.Instance;
        Containers _containers => Containers.instance;
        LevelModelController _levelModelController => LevelModelController._instance;
        
        public override void Initialize()
        {
            EventBus<TapITableItemEvent>.Subscribe(HandleTapItemEvent);
            EventBus<LevelCreatedEvent>.Subscribe(LevelCreated);
            EventBus<UseBoosterEvent>.Subscribe(UseBooster);
        }

        public override void Dispose()
        {
            EventBus<TapITableItemEvent>.Unsubscribe(HandleTapItemEvent);
            EventBus<LevelCreatedEvent>.Unsubscribe(LevelCreated);
            EventBus<UseBoosterEvent>.Unsubscribe(UseBooster);
        }
        
        
        
        


        private async void UseBooster(UseBoosterEvent useBoosterEvent)
        {
            switch (useBoosterEvent.BoosterType)
            {
                case BoosterType.PlayOn:

                    break;

                /*case BoosterType.PlayOn:
                    _levelModel.GameModel.MoveCount = 1;
                    _levelModel.IsLevelRunning = true;
                    break;*/
            }
        }


        private void HandleTapItemEvent(TapITableItemEvent tapITableItemEvent)
        {
            LevelProcess levelProcess = new LevelProcess();

            levelProcess.OnLevelComplete += () => { EventBus<LevelCompleteEvent>.Publish(new LevelCompleteEvent()); };
            levelProcess.OnLevelFail += (failType) =>
            {
                EventBus<LevelFailEvent>.Publish(new LevelFailEvent(failType));
            };
            levelProcess.State = new TapControlState(tapITableItemEvent.TapableItemItem);
        }

        private void LevelCreated(LevelCreatedEvent levelCreatedEvent)
        {
        }

        
    }
}