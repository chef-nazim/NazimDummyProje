using Lofelt.NiceVibrations;
using NCG.template._NCG.Core.AllEvents;
using NCG.template.EventBus;
using NCG.template.models;

namespace NCG.template.Managers
{
    public class HapticManager
    {
        private GameModel _gameModel;

        
        
        public HapticManager()
        {
            _gameModel = GameModel.Instance;
            Subscriptions();
        }

        protected void Subscriptions()
        {
           
            EventBus<FeelingEvent>.Subscriber(OnFeelingEvent);
        }

        private void OnFeelingEvent(FeelingEvent feelingEvent)
        {
            if (_gameModel.Haptic == 0)
            {
                return;
            }

            if (feelingEvent.HapticType == HapticPatterns.PresetType.None)
            {
                return;
            }

            HapticPatterns.PlayPreset(feelingEvent.HapticType);
        }
        
    }
}