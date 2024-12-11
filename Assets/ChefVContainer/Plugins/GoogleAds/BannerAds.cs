using System;
using System.Threading;
using Cysharp.Threading.Tasks;
#if SELF_PUBLISHING
using GoogleMobileAds.Api;
#endif

//using UnityEngine;

namespace gs.chef.vcontainer.plugins.googleads
{
#if SELF_PUBLISHING
    public class BannerAds : AdsBaseView<BannerView, AdsItemModel>
    {
        private bool _isShowing = false;

        private CancellationTokenSource _cancellationTokenSource;

        public BannerAds(AdsItemModel adsItemModel) : base(adsItemModel)
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }


        public override void RequestAd()
        {
            // If we already have a banner, destroy the old one.
            DestroyAd();

            AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

            AdsItem = null;

            AdsItem = new BannerView(AdsItemModel.AdsId, adaptiveSize, AdPosition.Bottom);

            // Listen to events the banner may raise.
            //AddListeners();
        }

        public override void ShowAd()
        {
            if (AdsItem != null)
            {
                if (!_isShowing)
                {
                    AdsItem.LoadAd(new AdRequest());
                    _isShowing = true;
                    _cancellationTokenSource?.Cancel();
                    _cancellationTokenSource = new CancellationTokenSource();
                    StartReloadTimer(_cancellationTokenSource.Token).Forget();
                }
            }
            else
            {
                _isShowing = false;
                //_cancellationTokenSource?.Cancel();
                RequestAd();
                ShowAd();
            }
        }

        private async UniTask StartReloadTimer(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromMinutes(AdsItemModel.Duration), cancellationToken: token);

            _isShowing = false;
            if (!token.IsCancellationRequested)
            {
                DestroyAd();
                ShowAd();
            }
        }

        public override void HideAd()
        {
            if (AdsItem is not null)
            {
                AdsItem.Hide();
            }
        }

        public override void DestroyAd()
        {
            if (AdsItem != null)
            {
                _isShowing = false;
                _cancellationTokenSource?.Cancel();
                //RemoveListeners();
                AdsItem.Destroy();
                AdsItem = null;
            }
        }

        public override void Dispose()
        {
            _cancellationTokenSource?.Cancel();
        }
    }
#else
    public class BannerAds {}
#endif
}