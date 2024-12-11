using System;
using System.ComponentModel;
using Cysharp.Threading.Tasks;
using gs.chef.bakerymerge.Objects;
using gs.chef.game.Scripts.Objects;
using gs.chef.game.Scripts.Others;
using gs.chef.game.Scripts.ScriptableObjects;
using gs.chef.game.Controllers;
using gs.chef.game.Events;
using gs.chef.game.models;
using gs.chef.vcontainer.core.managers;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace gs.chef.game.Managers
{
    public class TutorialManager : BaseSubscribable
    {
       
        [Inject] private readonly GameModel _gameModel;
        [Inject] private readonly TutorialCanvas _tutorialCanvas;
        [Inject] private readonly LevelModelController _levelModelController;
        [Inject] private readonly Containers _containers;
        [Inject] private readonly GameHelper _gameHelper;
        
        
        
        
        [Inject] private readonly ISubscriber<LevelModelCreatedEvent> _levelModelCreatedEventSubscriber;
        
        [Inject] private readonly ISubscriber<RestartButtonClickEvent> _restartButtonClickEventSubscriber;
        
        
        
        
        protected override void Init()
        {
            _gameModel.PropertyChanged += OnGameModelOnPropertyChanged;
            _tutorialCanvas.gameObject.SetActive(false);
        }

        protected override void Subscriptions()
        {
            _levelModelCreatedEventSubscriber.Subscribe(s => OnLevelModelCreated(s));
        
            _restartButtonClickEventSubscriber.Subscribe(s => OnRestartButtonClick(s));
        }

        private void OnLevelModelCreated(LevelModelCreatedEvent levelModelCreatedEvent)
        {
            CloseTutorial();
        }

        private void OnRestartButtonClick(RestartButtonClickEvent restartButtonClickEvent)
        {
            CloseTutorial();
        }
            
        void CloseTutorial()
        {
            _tutorialCanvas.gameObject.SetActive(false);
            _tutorialCanvas.TutorialHand.SetActive(false);
            _tutorialCanvas.TutorialText_Level1.SetActive(false);
            _tutorialCanvas.TutorialText_Level2_Text1.SetActive(false);
            _tutorialCanvas.TutorialText_Level2_Text2.SetActive(false);
            _tutorialCanvas.RatateHand.SetActive(false);
            _tutorialCanvas.RatateGameObject.SetActive(false);
            
        }

       

        private async void OnGameModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_gameModel.Tutorial))
            {
                
            }
        }
    }
}