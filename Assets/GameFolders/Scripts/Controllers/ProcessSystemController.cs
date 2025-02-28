using MessagePipe;
using NCG.template._NCG.Core.AllEvents;
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
    public class ProcessSystemController
    {
        #region Subscriber

        EventBinding<TapITableItemEvent> _tapITableItemEventSubscriber;
        EventBinding<LevelCreatedEvent> _levelCreatedEventSubscriber;
        EventBinding<UseBoosterEvent> _useBoosterEventSubscriber;

        #endregion

       

        //GameManager _gameManager => GameManager.instance;

        GameModel _gameModel;
        
        Containers _containers => Containers.instance;
        
        LevelModelController _levelModelController => LevelModelController._instance;


        public ProcessSystemController()
        {
            _gameModel =GameModel.Instance;
            Init();
        }


        protected void Init()
        {
            _tapITableItemEventSubscriber = new EventBinding<TapITableItemEvent>(HandleTapItemEvent);
            EventBus<TapITableItemEvent>.Subscribe(_tapITableItemEventSubscriber);

            _levelCreatedEventSubscriber = new EventBinding<LevelCreatedEvent>(LevelCreated);
            EventBus<LevelCreatedEvent>.Subscribe(_levelCreatedEventSubscriber);

            _useBoosterEventSubscriber = new EventBinding<UseBoosterEvent>(UseBooster);
            EventBus<UseBoosterEvent>.Subscribe(_useBoosterEventSubscriber);
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