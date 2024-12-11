namespace gs.chef.vcontainer.plugins.googleads.events
{
    public class AdsEvent
    {
        public AdsType AdsType { get; private set; }
        public string From { get; private set; }

        public int AdId { get; set; } = -1;

        public bool IsEarnedReward { get; set; } = false;

        public AdsEvent(AdsType adsType, string from="", int adId = -1)
        {
            AdsType = adsType;
            From = from;
            AdId = adId;
        }
    }
}