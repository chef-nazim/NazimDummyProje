using System;
using gs.chef.vcontainer.plugins.googleads.events;

namespace gs.chef.vcontainer.plugins.googleads
{
    public abstract class AdsBaseView<T, TModel> : IDisposable where T : class where TModel : AdsItemModel
    {
        public Action<AdsEvent, AdsEventStatus> onResponseAdEvent;
        public T AdsItem { get; protected set; }

        public TModel AdsItemModel { get; private set; }

        public AdsBaseView(TModel adsItemModel)
        {
            AdsItemModel = adsItemModel;
        }

        public virtual bool IsLoaded { get; protected set; }

        public virtual void LoadAd()
        {
        }

        public virtual void RequestAd()
        {
        }

        public virtual void ShowAd()
        {
        }

        public virtual void HideAd()
        {
        }

        public virtual void DestroyAd()
        {
        }

        public virtual void AddListeners()
        {
        }

        public virtual void RemoveListeners()
        {
        }

        public virtual void Dispose()
        {
        }
    }

    public class AdsItemModel
    {
        public AdsType AdsType { get; set; }
        public string AdsId { get; set; }

        public int Duration { get; set; }
    }
}