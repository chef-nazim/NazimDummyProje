using gs.chef.vcontainer.plugins.googleads.events;
#if SELF_PUBLISHING
using GameAnalyticsSDK;
using GoogleMobileAds.Api;
using UnityEngine;
#endif

namespace gs.chef.vcontainer.plugins.googleads
{
#if SELF_PUBLISHING
    public class RewardedAds : AdsBaseView<RewardedAd, AdsItemModel>
    {
        private AdsEvent _currentAdsEvent;

        public RewardedAds(AdsItemModel adsItemModel) : base(adsItemModel)
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
            RewardedAd.Load(AdsItemModel.AdsId, adRequest, HandleOnAdLoaded);
        }

        private void HandleOnAdLoaded(RewardedAd arg1, LoadAdError arg2)
        {
            if (arg2 is not null)
            {
                GameAnalytics.NewDesignEvent("RewardedAd:FailToLoad");
                Debug.Log("RewardedAd FAILED TO LOAD");
                onResponseAdEvent?.Invoke(new AdsEvent(AdsItemModel.AdsType), AdsEventStatus.ResponseLoadFail);
                return;
            }
            else if (arg1 is null)
            {
                GameAnalytics.NewDesignEvent("RewardedAd:FailToLoad");
                Debug.Log("RewardedAd FAILED TO LOAD");
                onResponseAdEvent?.Invoke(new AdsEvent(AdsItemModel.AdsType), AdsEventStatus.ResponseLoadFail);
                return;
            }

            AdsItem = arg1;
            AddListeners();
            Debug.Log("RewardedAd Loaded");
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

            if (IsLoaded)
            {
                AdsItem.Show((Reward reward) =>
                {
                    _currentAdsEvent.IsEarnedReward = true;
                    onResponseAdEvent?.Invoke(_currentAdsEvent, AdsEventStatus.ResponseEarnReward);
                });
            }
            else
            {
                onResponseAdEvent?.Invoke(_currentAdsEvent, AdsEventStatus.NotReadyYet);
            }
        }

        public override void DestroyAd()
        {
            if (AdsItem != null)
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
            onResponseAdEvent?.Invoke(_currentAdsEvent, AdsEventStatus.ResponseClosed);
            LoadAd();
        }

        private void HandleOnAdFullScreenContentOpened()
        {
            if (string.IsNullOrEmpty(_currentAdsEvent.From))
            {
                GameAnalytics.NewDesignEvent("RewardedAd:Showed");
            }
            else
            {
                GameAnalytics.NewDesignEvent($"RewardedAd:Showed:{_currentAdsEvent.From}");
            }

            onResponseAdEvent?.Invoke(_currentAdsEvent, AdsEventStatus.ResponseOpened);
        }

        private void HandleOnAdFullScreenContentFailed(AdError error)
        {
            if (string.IsNullOrEmpty(_currentAdsEvent.From))
            {
                GameAnalytics.NewDesignEvent("RewardedAd:FailToShow");
            }
            else
            {
                GameAnalytics.NewDesignEvent($"RewardedAd:FailToShow:{_currentAdsEvent.From}");
            }

            onResponseAdEvent?.Invoke(_currentAdsEvent, AdsEventStatus.ResponseShowFail);
            LoadAd();
        }

        public override void Dispose()
        {
            DestroyAd();
        }
    }
#else
    public class RewardedAds{}
#endif
}