using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

#if SELF_PUBLISHING
using GoogleMobileAds.Ump.Api;
using GoogleMobileAds.Api;
#endif

/*#if SELF_PUBLISHING
using GoogleMobileAds.Api;
#endif*/

namespace gs.chef.vcontainer.plugins.googleads
{
    public enum ChefConsentStatus
    {
        None,
        Failed,
        Unknown,
        NotRequired,
        Required,
        Obtained,
        ShowForm
    }

    public class GoogleMobileAdsConsentController
    {
        private static ChefConsentStatus _initializeResult = ChefConsentStatus.None;
#if SELF_PUBLISHING

        private string _testDeviceId;

        public GoogleMobileAdsConsentController(string testDeviceId)
        {
            _testDeviceId = testDeviceId;
        }
        

        public bool CanRequestAds
        {
            get
            {
                UpdatePrivacyButton();
                return ConsentInformation.ConsentStatus == ConsentStatus.Obtained ||
                       ConsentInformation.ConsentStatus == ConsentStatus.NotRequired;
            }
        }

        private static bool _privacyBtnInteractable = false;

        public static bool PrivacyBtnInteractable => _privacyBtnInteractable;

        public static async UniTask<bool> ShowPrivacyForm(CancellationToken token)
        {
            bool? result = null;

            ConsentForm.LoadAndShowConsentFormIfRequired((showError =>
            {
                UpdatePrivacyButton();
                if (showError != null)
                {
                    Debug.Log(
                        $"##### Show ConsentForm failed. ErrorCode: {showError.ErrorCode} ErrorMessage: {showError.Message}");
                    result = false;
                    //_initializeResult = ChefConsentStatus.Failed;
                }
                // Form showing succeeded.
                else
                {
                    Debug.Log($"##### Show ConsentForm succeeded. ConsentStatus is {ConsentInformation.ConsentStatus}");
                    //_initializeResult = ChefConsentStatus.ShowForm;
                    result = true;
                }
            }));

            await UniTask.WaitWhile((() => result is null), cancellationToken: token).SuppressCancellationThrow();
            return result != null && result.Value;
        }

        private static void UpdatePrivacyButton()
        {
            _privacyBtnInteractable =
                ConsentInformation.PrivacyOptionsRequirementStatus ==
                PrivacyOptionsRequirementStatus.Required;
        }

#endif

        public async UniTask<ChefConsentStatus> GatherConsent(CancellationToken token)
        {
#if SELF_PUBLISHING && GOOGLEADS_TESTDEVICE
            Debug.unityLogger.logEnabled = true;
            ConsentInformation.Reset();
            var requestParameters = new ConsentRequestParameters
            {
                TagForUnderAgeOfConsent = false,
                ConsentDebugSettings = new ConsentDebugSettings
                {
                    DebugGeography = DebugGeography.EEA
                }
            };
            if (string.IsNullOrEmpty(_testDeviceId))
            {
                requestParameters.ConsentDebugSettings.TestDeviceHashedIds = new List<string>
                {
                    AdRequest.TestDeviceSimulator,
                };
            }
            else
            {
                requestParameters.ConsentDebugSettings.TestDeviceHashedIds = new List<string>
                {
                    AdRequest.TestDeviceSimulator,
                    _testDeviceId
                };
            }
#elif SELF_PUBLISHING && !GOOGLEADS_TESTDEVICE
            
            var requestParameters = new ConsentRequestParameters
            {
                TagForUnderAgeOfConsent = false
            };
            
#endif

#if SELF_PUBLISHING
            
            
            _initializeResult = ChefConsentStatus.None;

            // The Google Mobile Ads SDK provides the User Messaging Platform (Google's
            // IAB Certified consent management platform) as one solution to capture
            // consent for users in GDPR impacted countries. This is an example and
            // you can choose another consent management platform to capture consent.
            ConsentInformation.Update(requestParameters, (FormError updateError) =>
            {
                // Enable the change privacy settings button.
                UpdatePrivacyButton();

                if (updateError != null)
                {
                    Debug.Log(
                        $"##### GoogleAds Consent failed ErrorCode: {updateError.ErrorCode} ErrorMessage: {updateError.Message}");
                    _initializeResult = ChefConsentStatus.Failed;
                    return;
                }

                // Determine the consent-related action to take based on the ConsentStatus.
                if (CanRequestAds)
                {
                    Debug.Log(
                        $"##### Consent has already been gathered or not required. ConsentStatus is {ConsentInformation.ConsentStatus}");
                    // Consent has already been gathered or not required.
                    // Return control back to the user.
                    _initializeResult = (ConsentStatus.Obtained == ConsentInformation.ConsentStatus)
                        ? ChefConsentStatus.Obtained
                        : ChefConsentStatus.NotRequired;
                    return;
                }

                // Consent not obtained and is required.
                // Load the initial consent request form for the user.
                ConsentForm.LoadAndShowConsentFormIfRequired((FormError showError) =>
                {
                    UpdatePrivacyButton();
                    if (showError != null)
                    {
                        Debug.Log(
                            $"##### Show ConsentForm failed. ErrorCode: {showError.ErrorCode} ErrorMessage: {showError.Message}");
                        _initializeResult = ChefConsentStatus.Failed;
                    }
                    // Form showing succeeded.
                    else
                    {
                        Debug.Log(
                            $"##### Show ConsentForm succeeded. ConsentStatus is {ConsentInformation.ConsentStatus}");
                        _initializeResult = ChefConsentStatus.ShowForm;
                    }
                });
            });

            await UniTask.WaitWhile(() => _initializeResult == ChefConsentStatus.None, cancellationToken: token)
                .AttachExternalCancellation(token).SuppressCancellationThrow();

            Debug.Log($"##### GoogleAds Consent Initialize Complete");
#endif
            return _initializeResult;
        }
    }
}