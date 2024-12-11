using gs.chef.vcontainer.plugins.googleads.events;
using UnityEngine;
#if SELF_PUBLISHING
using GameAnalyticsSDK;
using GoogleMobileAds.Api;
#endif


namespace gs.chef.vcontainer.plugins.googleads
{
#if SELF_PUBLISHING

    public class InterstitialAds : AdsBaseView<InterstitialAd, InterstitialAdsItemModel>
    {
        //private CancellationTokenSource _cancellationTokenSource;
        private AdsEvent _currentAdsEvent;

        public float RequestTime { get; set; }

        public InterstitialAds(InterstitialAdsItemModel adsItemModel) : base(adsItemModel)
        {
        }

        public override bool IsLoaded => AdsItem?.CanShowAd() ?? false;

        public override void LoadAd()
        {
            RequestAd();
        }

        public override void RequestAd()
        {
            _currentAdsEvent = null;
            DestroyAd();

            var adRequest = new AdRequest();
            InterstitialAd.Load(AdsItemModel.AdsId, adRequest, HandleOnAdLoaded);
        }

        private void HandleOnAdLoaded(InterstitialAd ad, LoadAdError error)
        {
            if (error is not null)
            {
                GameAnalytics.NewDesignEvent("InterstitialAd:FailToLoad");
                Debug.Log("InterstitialAd FAILED TO LOAD");
                onResponseAdEvent?.Invoke(new AdsEvent(AdsItemModel.AdsType), AdsEventStatus.ResponseLoadFail);
                return;
            }
            else if (ad is null)
            {
                GameAnalytics.NewDesignEvent("InterstitialAd:FailToLoad");
                Debug.Log("InterstitialAd FAILED TO LOAD");
                onResponseAdEvent?.Invoke(new AdsEvent(AdsItemModel.AdsType), AdsEventStatus.ResponseLoadFail);
                return;
            }

            AdsItem = ad;
            RequestTime = Time.time;
            AddListeners();
            Debug.Log("InterstitialAd Loaded");
        }

        public void ShowAd(AdsEvent e)
        {
            _currentAdsEvent = e;
            ShowAd();
        }

        public override void ShowAd()
        {
            if (AdsItem is null)
            {
                onResponseAdEvent?.Invoke(_currentAdsEvent, AdsEventStatus.NotRequestedYet);
                return;
            }

            var diffTime = Time.time - RequestTime;
            if (diffTime >= AdsItemModel.Duration || AdsItemModel.UseAutoShow)
            {
                if (AdsItem is not null && AdsItem.CanShowAd())
                {
                    AdsItem.Show();
                }
                else
                {
                    onResponseAdEvent?.Invoke(_currentAdsEvent, AdsEventStatus.NotReadyYet);
                }
            }
            else
            {
                onResponseAdEvent?.Invoke(_currentAdsEvent, AdsEventStatus.NotReadyYet);
            }
        }

        public override void DestroyAd()
        {
            if (AdsItem is not null)
            {
                RemoveListeners();
                AdsItem.Destroy();
                AdsItem = null;
            }
        }

        public override void AddListeners()
        {
            if (AdsItem is not null)
            {
                AdsItem.OnAdFullScreenContentClosed += HandleOnAdFullScreenContentClosed;
                AdsItem.OnAdFullScreenContentOpened += HandleOnAdFullScreenContentOpened;
                AdsItem.OnAdFullScreenContentFailed += HandleOnAdFullScreenContentFailed;
            }
        }

        public override void RemoveListeners()
        {
            if (AdsItem is not null)
            {
                AdsItem.OnAdFullScreenContentClosed -= HandleOnAdFullScreenContentClosed;
                AdsItem.OnAdFullScreenContentOpened -= HandleOnAdFullScreenContentOpened;
                AdsItem.OnAdFullScreenContentFailed -= HandleOnAdFullScreenContentFailed;
            }
        }

        private void HandleOnAdFullScreenContentClosed()
        {
            //GameAnalytics.NewDesignEvent("InterstitialAd:Closed");
            onResponseAdEvent?.Invoke(_currentAdsEvent, AdsEventStatus.ResponseClosed);
            LoadAd();
        }

        private void HandleOnAdFullScreenContentOpened()
        {
            GameAnalytics.NewDesignEvent("InterstitialAd:Showed");
            onResponseAdEvent?.Invoke(_currentAdsEvent, AdsEventStatus.ResponseOpened);
        }

        private void HandleOnAdFullScreenContentFailed(AdError error)
        {
            GameAnalytics.NewDesignEvent("InterstitialAd:FailToShow");
            onResponseAdEvent?.Invoke(_currentAdsEvent, AdsEventStatus.ResponseShowFail);
            LoadAd();
        }

        public override void Dispose()
        {
            DestroyAd();
        }
    }

#else
    public class InterstitialAds{}
#endif

    public class InterstitialAdsItemModel : AdsItemModel
    {
        public bool UseAutoShow { get; set; }
    }
}