using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using gs.chef.vcontainer.plugins.googleads.events;
using MessagePipe;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

#if SELF_PUBLISHING
using GoogleMobileAds.Api;
#endif

namespace gs.chef.vcontainer.plugins.googleads
{
    public enum ChefInitializeStatus
    {
        None,
        NotInitialized,
        Success,
        Failed
    }

    [AddComponentMenu("CHEF Game/Chef Google Ads Controller")]
    public class ChefGoogleAdsController : MonoBehaviour, IDisposable
    {
        [SerializeField] private bool _useConsent = true;
        [SerializeField] private bool _useTestAds = false;

        [Space(5f)] [Header("Banner")] [SerializeField]
        private bool _useBanner = true;

        [SerializeField] private int _bannerReloadMinutes = 5;

        [Space(5f)] [Header("Interstitial")] [SerializeField]
        private bool _useInterstitial = true;

        [SerializeField] private int _interstitialDurationSeconds = 45;
        [SerializeField] private bool _interstitialAutoShow = false;

        [Space(5f)] [Header("Rewarded")] [SerializeField]
        private bool _useRewarded = false;


        [Space(5f)]
        [Header(
            "Test Device: \nCopy your test device ID from the XCODE logs and paste it here.\nUMPDebugSettings.testDeviceIdentifiers = @[ @\"D01738DA-B\" ];")]
        [SerializeField]
        private string _testDeviceIdIOS = "";
        [SerializeField]
        private string _testDeviceIdDRD = "";


        [Space(5f)] [SerializeField] private GoogleAdsIds _googleAdsIds;

        private IDisposable _subscription;
        private DisposableBagBuilder _bagBuilder;


        private ISubscriber<AdsEventStatus, AdsEvent> _adsEventSubscriber;
        private IPublisher<AdsEventStatus, AdsEvent> _adsEventPublisher;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            _adsEventSubscriber = resolver.Resolve<ISubscriber<AdsEventStatus, AdsEvent>>();
            _adsEventPublisher = resolver.Resolve<IPublisher<AdsEventStatus, AdsEvent>>();

            AddSubscriptions();
        }

        private void AddSubscriptions()
        {
            _bagBuilder = DisposableBag.CreateBuilder();
            _adsEventSubscriber.Subscribe(AdsEventStatus.Request, @event => OnAdsRequest(@event)).AddTo(_bagBuilder);
            _subscription = _bagBuilder.Build();
        }

        private void OnAdsRequest(AdsEvent e)
        {
#if SELF_PUBLISHING
            switch (e.AdsType)
            {
                case AdsType.Banner:
                    if (_useBanner)
                    {
                        _bannerAds?.ShowAd();
                    }

                    break;
                case AdsType.Interstitial:
                    if (_useInterstitial)
                    {
                        _interstitialAds.ShowAd(e);
                    }

                    break;
                case AdsType.Rewarded:
                    if (_useRewarded)
                    {
                        _rewardedAds.ShowAd(e);
                    }

                    break;
            }
#endif
        }


        public void Dispose()
        {
#if SELF_PUBLISHING
            _bannerAds?.Dispose();
            _interstitialAds?.Dispose();
            _rewardedAds?.Dispose();
#endif
            _subscription?.Dispose();
        }

        private GoogleMobileAdsConsentController _consentController;

#if SELF_PUBLISHING
        private RequestConfiguration _requestConfiguration;

        public static bool IsInterstitialAdReady => _interstitialAds?.IsLoaded ?? false;

        public static bool IsRewardedAdReady => _rewardedAds?.IsLoaded ?? false;

        /*private InterstitialAd _interstitialAd;
        private RewardedAd _rewardedAd;
        */

        private ChefInitializeStatus _initializeResult = ChefInitializeStatus.NotInitialized;
        private ChefInitializeStatus _consentInitializeResult = ChefInitializeStatus.NotInitialized;

        private BannerAds _bannerAds;
        private static InterstitialAds _interstitialAds;
        private static RewardedAds _rewardedAds;
        public ChefInitializeStatus InitializeResult => _initializeResult;
        public ChefInitializeStatus ConsentResult => _consentInitializeResult;
#else
        public ChefInitializeStatus InitializeResult => ChefInitializeStatus.Success;
        public static bool IsInterstitialAdReady => false;
        public static bool IsRewardedAdReady => false;
#endif

        public async UniTask Initialize(CancellationToken token)
        {
#if SELF_PUBLISHING
            if (_requestConfiguration is not null)
                return;

            string testDeviceId = "";

#if UNITY_IOS
            testDeviceId = _testDeviceIdIOS;
#elif UNITY_ANDROID
            testDeviceId = _testDeviceIdDRD;
#endif

            _consentController ??= new GoogleMobileAdsConsentController(testDeviceId);


            // On Android, Unity is paused when displaying interstitial or rewarded video.
            // This setting makes iOS behave consistently with Android.
            MobileAds.SetiOSAppPauseOnBackground(true);

            // When true all events raised by GoogleMobileAds will be raised
            // on the Unity main thread. The default value is false.
            // https://developers.google.com/admob/unity/quick-start#raise_ad_events_on_the_unity_main_thread
            MobileAds.RaiseAdEventsOnUnityMainThread = true;

            // Configure your RequestConfiguration with Child Directed Treatment
            // and the Test Device Ids.
            _requestConfiguration = new RequestConfiguration()
            {
                TagForChildDirectedTreatment = TagForChildDirectedTreatment.Unspecified,
                TagForUnderAgeOfConsent = TagForUnderAgeOfConsent.Unspecified
            };
            MobileAds.SetRequestConfiguration(_requestConfiguration);

            if (_useConsent)
            {
                if (_consentController.CanRequestAds)
                {
                    await InitializeGoogleMobileAds(token).AttachExternalCancellation(token)
                        .SuppressCancellationThrow();
                }
                else
                {
                    var consentResult = await _consentController.GatherConsent(token).AttachExternalCancellation(token)
                        .SuppressCancellationThrow();


                    await InitializeGoogleMobileAds(token).AttachExternalCancellation(token)
                        .SuppressCancellationThrow();
                }
            }
            else
            {
                await InitializeGoogleMobileAds(token).AttachExternalCancellation(token).SuppressCancellationThrow();
            }

            await UniTask.DelayFrame(5, cancellationToken: token);

            PrepareAds();
#endif
            await UniTask.CompletedTask;
        }

#if SELF_PUBLISHING
        private async UniTask InitializeGoogleMobileAds(CancellationToken token)
        {
            if (_initializeResult != ChefInitializeStatus.NotInitialized) return;

            _initializeResult = ChefInitializeStatus.None;

            // Initialize the Google Mobile Ads Unity plugin.
            Debug.Log("##### Google Mobile Ads Initializing.");
            MobileAds.Initialize((InitializationStatus initstatus) =>
            {
                if (initstatus == null)
                {
                    Debug.LogError("##### Google Mobile Ads initialization failed.");
                    _initializeResult = ChefInitializeStatus.Failed;
                    return;
                }

                // If you use mediation, you can check the status of each adapter.
                var adapterStatusMap = initstatus.getAdapterStatusMap();
                if (adapterStatusMap != null)
                {
                    foreach (var item in adapterStatusMap)
                    {
                        Debug.Log(string.Format("Adapter {0} is {1}",
                            item.Key,
                            item.Value.InitializationState));
                    }
                }

                Debug.Log("##### Google Mobile Ads initialization is Success.");
                _initializeResult = ChefInitializeStatus.Success;
            });

            await UniTask.WaitWhile(() => _initializeResult == ChefInitializeStatus.None, cancellationToken: token);
        }

        private void PrepareAds()
        {
            if (_useBanner)
            {
                PrepareBannerAds();
            }

            if (_useInterstitial)
            {
                PrepareInterstitialAds();
            }

            if (_useRewarded)
            {
                PrepareRewardedAds();
            }
        }

        private void PrepareRewardedAds()
        {
            RemoveRewardedAdsHandlers();
            var rewardedId = _googleAdsIds.GetRewardedId(_useTestAds);
            var adsModel = new AdsItemModel
            {
                AdsId = rewardedId,
                AdsType = AdsType.Rewarded,
                Duration = 0
            };

            _rewardedAds ??= new RewardedAds(adsModel);

            AddRewardedAdsHandlers();
            _rewardedAds.LoadAd();
            Debug.Log("##### Prepare Rewarded Ads Complete");
        }

        private void AddRewardedAdsHandlers()
        {
            if (_rewardedAds is null) return;
            _rewardedAds.onResponseAdEvent += OnRewardedAdsResponseAdEvent;
        }

        private void RemoveRewardedAdsHandlers()
        {
            if (_rewardedAds is null) return;
            _rewardedAds.onResponseAdEvent -= OnRewardedAdsResponseAdEvent;
        }

        private void OnRewardedAdsResponseAdEvent(AdsEvent eAdsEvent, AdsEventStatus eventStatus)
        {
            if (eAdsEvent.AdsType == AdsType.Rewarded && eventStatus == AdsEventStatus.ResponseClosed)
            {
                if (_useInterstitial && _interstitialAds is not null)
                    _interstitialAds.RequestTime = Time.time;
            }

            _adsEventPublisher.Publish(eventStatus, eAdsEvent);
        }

        private void PrepareInterstitialAds()
        {
            RemoveInterstitialAdsHandlers();
            var interstitialId = _googleAdsIds.GetInterstitialId(_useTestAds);
            var adsModel = new InterstitialAdsItemModel
            {
                AdsId = interstitialId,
                AdsType = AdsType.Interstitial,
                Duration = _interstitialDurationSeconds,
                UseAutoShow = _interstitialAutoShow
            };
            _interstitialAds ??= new InterstitialAds(adsModel);

            AddInterstitialAdsHandlers();

            _interstitialAds.LoadAd();
            Debug.Log("##### Prepare Interstitial Ads Complete");
        }

        private void RemoveInterstitialAdsHandlers()
        {
            if (_interstitialAds is null) return;
            _interstitialAds.onResponseAdEvent -= OnInterstitialAdsResponseAdEvent;
        }

        private void AddInterstitialAdsHandlers()
        {
            if (_interstitialAds is null) return;
            _interstitialAds.onResponseAdEvent += OnInterstitialAdsResponseAdEvent;
        }

        private void OnInterstitialAdsResponseAdEvent(AdsEvent eAdsEvent, AdsEventStatus eventStatus)
        {
            _adsEventPublisher.Publish(eventStatus, eAdsEvent);
        }

        private void PrepareBannerAds()
        {
            var bannerId = _googleAdsIds.GetBannerId(_useTestAds);
            var adsModel = new AdsItemModel
            {
                AdsId = bannerId,
                AdsType = AdsType.Banner,
                Duration = _bannerReloadMinutes
            };
            _bannerAds ??= new BannerAds(adsModel);
            _bannerAds.RequestAd();
            Debug.Log("##### Prepare Banner Ads Complete");
        }
#endif
    }
}