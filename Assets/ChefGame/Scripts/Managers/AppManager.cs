using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using gs.chef.game.Events;
using gs.chef.game.Scripts.Others;
using gs.chef.vcontainer.core.events;
using gs.chef.vcontainer.core.managers;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace gs.chef.game.Managers
{
    public class AppManager : BaseAppManager
    {
        [Inject] private readonly IPublisher<CreateGamePlaySceneEvent> _crateGamePlaySceneEventPublisher;


        protected override async UniTask InitializeGame(CancellationToken token)
        {
            DOTween.SetTweensCapacity(1000, 1000);
            //Application.targetFrameRate = 60;
          //  GameAnalytics.Initialize();
            AnalyticEventHelper.GameStart();
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            await UniTask.CompletedTask;
        }


        protected override void OnAppReady(AppReadyEvent appReadyEvent)
        {
            Debug.Log("App Ready");
            _crateGamePlaySceneEventPublisher.Publish(new CreateGamePlaySceneEvent());
        }

        public override void Dispose()
        {
            base.Dispose();
            AnalyticEventHelper.GameEnd();
        }
    }
}