using System;
using UnityEngine;

namespace gs.chef.vcontainer.plugins.googleads
{
    [Serializable]
    public class GoogleAdsIds
    {
        [Header("Android")] public string drdBannerId;
        public string drdInterstitialId;
        public string drdRewardedId;

        [Space(10f)] [Header("iOS")] public string iosBannerId;
        public string iosInterstitialId;
        public string iosRewardedId;

        public const string drdTestBannerId = "ca-app-pub-3940256099942544/6300978111";
        public const string drdTestInterstitialId = "ca-app-pub-3940256099942544/1033173712";
        public const string drdTestRewardedId = "ca-app-pub-3940256099942544/5224354917";

        public const string iosTestBannerId = "ca-app-pub-3940256099942544/2934735716";
        public const string iosTestInterstitialId = "ca-app-pub-3940256099942544/4411468910";
        public const string iosTestRewardedId = "ca-app-pub-3940256099942544/1712485313";

        public string GetBannerId(bool useTestAds)
        {
#if UNITY_ANDROID
            return useTestAds ? drdTestBannerId : drdBannerId;
#elif UNITY_IPHONE
            return useTestAds ? iosTestBannerId : iosBannerId;
#else
            return "";
#endif
        }

        public string GetInterstitialId(bool useTestAds)
        {
#if UNITY_ANDROID
            return useTestAds ? drdTestInterstitialId : drdInterstitialId;
#elif UNITY_IPHONE
            return useTestAds ? iosTestInterstitialId : iosInterstitialId;
#else
            return "";
#endif
        }

        public string GetRewardedId(bool useTestAds)
        {
#if UNITY_ANDROID
            return useTestAds ? drdTestRewardedId : drdRewardedId;
#elif UNITY_IPHONE
            return useTestAds ? iosTestRewardedId : iosRewardedId;
#else
            return "";
#endif
        }
    }
}