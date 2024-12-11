using gs.chef.game.Scripts.Others;
using gs.chef.game.Scripts.State;
using gs.chef.game.enums;
using gs.chef.game.Events;
using gs.chef.game.models;
using gs.chef.game.Objects;
using gs.chef.vcontainer.core.managers;
using Lofelt.NiceVibrations;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace gs.chef.game.Controllers
{
    public class ProcessSystemController : BaseSubscribable
    {
        #region Subscriber

        [Inject] private readonly ISubscriber<TapITableItemEvent> _tapITableItemEventSubscriber;
        [Inject] private readonly ISubscriber<LevelCreatedEvent> _levelCreatedEventSubscriber;
        [Inject] private readonly ISubscriber<UseBoosterEvent> _useBoosterEventSubscriber;
        #endregion

        #region Publisher

        [Inject] private readonly IPublisher<LevelCompleteEvent> _levelCompleteEventPublisher;
        [Inject] private readonly IPublisher<LevelFailEvent> _levelFailEventPublisher;
        [Inject] private readonly IPublisher<FeelingEvent> _feelingEventPublisher;

        #endregion

        [Inject] private readonly IObjectResolver _resolver;
        [Inject] private readonly GameModel _gameModel;
        [Inject] private readonly Containers _containers;
        [Inject] private readonly LevelModelController _levelModelController;
        
        

        protected override void Subscriptions()
        {
            _tapITableItemEventSubscriber.Subscribe(s => HandleTapItemEvent(s));
            _levelCreatedEventSubscriber.Subscribe(s => LevelCreated(s));
            _useBoosterEventSubscriber.Subscribe(s => UseBooster(s));
        }


        protected override void Init()
        {
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
            LevelProcess levelProcess = new LevelProcess(_resolver);

            levelProcess.OnLevelComplete += () => { _levelCompleteEventPublisher.Publish(new LevelCompleteEvent()); };
            levelProcess.OnLevelFail += (failType) => {_levelFailEventPublisher.Publish(new LevelFailEvent(failType));};
            levelProcess.State = new TapControlState(tapITableItemEvent.TapableItemItem);
        }

        private void LevelCreated(LevelCreatedEvent levelCreatedEvent)
        {
            

        }

      
    }
}