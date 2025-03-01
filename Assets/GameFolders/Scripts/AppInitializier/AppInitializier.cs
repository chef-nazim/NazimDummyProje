using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NCG.template._NCG.Core.AllEvents;
using NCG.template.Controllers;
using NCG.template.EventBus;
using NCG.template.extensions;
using NCG.template.Managers;
using NCG.template.models;
using NCG.template.Scripts.Others;
using NCG.template.Scripts.ScriptableObjects;
using UnityEngine;

namespace NCG.template.GameFolders.Scripts.AppInitializier
{
    public class AppInitializier: Singleton<AppInitializier>
    {
        [SerializeField] private GameHelper _gameHelper;
        private GameModel _gameModel;
        
        
        public GameModel GameModel => _gameModel;
        public GameHelper GameHelper  => _gameHelper;
        
        

        protected override async void Awake()
        {
            base.Awake();
            _gameModel = new GameModel();
            await InitializeGame();
        }

        private void Start()
        {
            
            Canvas.ForceUpdateCanvases();
            EventBus<CreateGamePlaySceneEvent>.Publish(new CreateGamePlaySceneEvent());
        }

        async UniTask InitializeGame()
        {
            
            DOTween.SetTweensCapacity(1000, 1000);
            
            EventLoader.InitializAllBaseManager();
            EventLoader.InitializAllBaseController();
            //Application.targetFrameRate = 60;
            //  GameAnalytics.Initialize();
            AnalyticEventHelper.GameStart();
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}