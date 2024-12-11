using System.Threading;
using Cysharp.Threading.Tasks;
using gs.chef.vcontainer.core.events;
using gs.chef.vcontainer.plugins.attauth;
using gs.chef.vcontainer.plugins.googleads;
using gs.chef.vcontainer.plugins.googleads.events;
using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace gs.chef.vcontainer.core.managers
{
    public abstract class BaseAppManager : BaseSubscribable, IAsyncStartable
    {
        [Inject] private readonly IPublisher<AppReadyEvent> AppReadyEventPublisher;
        [Inject] protected readonly ISubscriber<AppReadyEvent> AppReadyEventSubscriber;

#if SELF_PUBLISHING
        [Inject] private readonly CHEF_ATTAuth_Manager _attAuthManager;
        [Inject] private readonly ChefGoogleAdsController _chefGoogleAdsController;
#endif
        [Inject] protected readonly IPublisher<AdsEventStatus, AdsEvent> _adsEventPublisher;

        public async UniTask StartAsync(CancellationToken cancellation)
        {
#if SELF_PUBLISHING
            await InitializeSelfPublishing(cancellation).AttachExternalCancellation(cancellation)
                .SuppressCancellationThrow();
#endif
            await InitializeGame(cancellation).AttachExternalCancellation(cancellation)
                .SuppressCancellationThrow();

            AppReadyEventPublisher.Publish(new AppReadyEvent());

            await UniTask.CompletedTask;
        }

        protected abstract UniTask InitializeGame(CancellationToken token);

#if SELF_PUBLISHING
        private async UniTask InitializeSelfPublishing(CancellationToken token)
        {
            await _attAuthManager.Initialize(token).AttachExternalCancellation(token)
                .SuppressCancellationThrow();
            await _chefGoogleAdsController.Initialize(token).AttachExternalCancellation(token)
                .SuppressCancellationThrow();
        }
#endif

        protected void ShowBanner()
        {
            _adsEventPublisher.Publish(AdsEventStatus.Request, new AdsEvent(AdsType.Banner));
        }

        protected override void Subscriptions()
        {
            AppReadyEventSubscriber.Subscribe(OnAppReady).AddTo(_bagBuilder);
        }

        protected abstract void OnAppReady(AppReadyEvent appReadyEvent);
    }
}