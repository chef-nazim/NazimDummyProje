using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

#if SELF_PUBLISHING
using GoogleMobileAds.Ump.Api;
#endif

namespace gs.chef.vcontainer.plugins.googleads
{
    public class ChefPrivacyBtn : MonoBehaviour
    {
        private Button _btn;
        private CancellationTokenSource _cts;

        private void Awake()
        {
            bool btnShow = false;
            TryGetComponent(out _btn);

            if (_btn is not null)
            {
#if SELF_PUBLISHING
                btnShow = GoogleMobileAdsConsentController.PrivacyBtnInteractable;

                _btn.interactable = btnShow;
                gameObject.SetActive(btnShow);
#else
                gameObject.SetActive(true);
                _btn.interactable = true;
#endif
            }
        }

        private void OnDisable()
        {
            _cts?.Cancel();
            if (_btn is not null)
            {
                _btn.onClick.RemoveAllListeners();
            }
        }

        private void OnEnable()
        {
            _cts?.Cancel();
            if (_btn is not null)
            {
                _btn.onClick.AddListener(() =>
                {
                    _cts = new CancellationTokenSource();
                    OnClick(_cts.Token).SuppressCancellationThrow();
                });
            }
        }

        private async UniTask OnClick(CancellationToken token)
        {
            _btn.interactable = false;
            bool result = false;
#if SELF_PUBLISHING
            await GoogleMobileAdsConsentController.ShowPrivacyForm(token).AttachExternalCancellation(token)
                .SuppressCancellationThrow();

            var btnShow = ConsentInformation.PrivacyOptionsRequirementStatus ==
                          PrivacyOptionsRequirementStatus.Required;

            gameObject.SetActive(btnShow);
            _btn.interactable = btnShow;


#else
            gameObject.SetActive(true);
            _btn.interactable = true;
#endif
            _cts.Cancel();
        }
    }
}