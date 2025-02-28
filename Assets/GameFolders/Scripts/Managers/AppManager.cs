using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NCG.template._NCG.Core.AllEvents;
using NCG.template.Controllers;
using NCG.template.EventBus;
using NCG.template.Events;
using NCG.template.extensions;
using NCG.template.models;
using NCG.template.Scripts.Others;
using NCG.template.Scripts.ScriptableObjects;
using UnityEngine;
namespace NCG.template.Managers
{
    public class AppManager : Singleton<AppManager>
    {
        [SerializeField] private GameHelper _gameHelper;
        private GameModel _gameModel;
        public GameModel GameModel => _gameModel;
        public GameHelper GameHelper  => _gameHelper;


        #region Manager

        private HapticManager _hapticManager;
        private SoundManager _soundManager;

        #endregion

        #region Controller

        private ProcessSystemController _processSystemController;

        #endregion
        
        
        
        
        

        protected override async void Awake()
        {
            base.Awake();
            
            await InitializeGame();
            _gameModel = new GameModel();

            _processSystemController = new ProcessSystemController();

            _hapticManager = new HapticManager();
            _soundManager = new SoundManager();

            EventBus<CreateGamePlaySceneEvent>.Publish(new CreateGamePlaySceneEvent());
        }


        async UniTask InitializeGame()
        {
            DOTween.SetTweensCapacity(1000, 1000);
            //Application.targetFrameRate = 60;
            //  GameAnalytics.Initialize();
            AnalyticEventHelper.GameStart();
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}