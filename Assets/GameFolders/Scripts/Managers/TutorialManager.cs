using System.ComponentModel;
using NCG.template._NCG.Core.AllEvents;
using NCG.template.Controllers;
using NCG.template.EventBus;
using NCG.template.extensions;
using NCG.template.models;
using NCG.template.Scripts.Objects;
using NCG.template.Scripts.Others;
using NCG.template.Scripts.ScriptableObjects;
using VContainer;

namespace NCG.template.Managers
{
    public class TutorialManager : Singleton<TutorialManager>
    {
        [Inject] private readonly GameModel _gameModel;
        [Inject] private readonly TutorialCanvas _tutorialCanvas;
        [Inject] private readonly LevelModelController _levelModelController;
        [Inject] private readonly Containers _containers;
        [Inject] private readonly GameHelper _gameHelper;
        

        private void OnEnable()
        {
            _gameModel.PropertyChanged += OnGameModelOnPropertyChanged;
            _tutorialCanvas.gameObject.SetActive(false);
            Subscriptions();
        }


        void Subscriptions()
        {
            
            EventBus<LevelModelCreatedEvent>.Subscribe(OnLevelModelCreated);


            
            EventBus<RestartButtonClickEvent>.Subscribe(OnRestartButtonClick);
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