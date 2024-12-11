using gs.chef.game.Events;
using gs.chef.game.models;
using gs.chef.vcontainer.core.managers;
using Lofelt.NiceVibrations;
using MessagePipe;
using VContainer;

namespace gs.chef.game.Managers
{
    public class HapticManager : BaseSubscribable
    {
        [Inject] private readonly GameModel _gameModel;
        [Inject] private readonly ISubscriber<FeelingEvent> _feelingEvent;
        protected override void Subscriptions()
        {
            _feelingEvent.Subscribe(s=>OnFeelingEvent(s));
        }

        private void OnFeelingEvent(FeelingEvent feelingEvent)
        {
            if (_gameModel.Haptic==0)
            {
                return;
            }

            if (feelingEvent.HapticType == HapticPatterns.PresetType.None)
            {
                return;
            }
            
            HapticPatterns.PlayPreset(feelingEvent.HapticType);
        }

        protected override void Init()
        {
            
        }
    }
}