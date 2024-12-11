using gs.chef.game.Scripts.Others;
using gs.chef.game.Scripts.ScriptableObjects;
using gs.chef.game.enums;
using gs.chef.game.Events;
using gs.chef.game.models;
using gs.chef.vcontainer.core.managers;
using MessagePipe;
using VContainer;

namespace gs.chef.game.Managers
{
    public class SoundManager : BaseSubscribable
    {
        [Inject] private readonly GameModel _gameModel;
        [Inject] private readonly ISubscriber<FeelingEvent> _feelingEvent;
        [Inject] private readonly Containers _containers;
        [Inject] private readonly GameHelper _gameHelper;

        protected override void Subscriptions()
        {
            _feelingEvent.Subscribe(s => OnFeelingEvent(s));
        }

        private void OnFeelingEvent(FeelingEvent feelingEvent)
        {
            if (_gameModel.Sound == 0)
            {
                return;
            }

            if (feelingEvent.SoundType == PlaySound.None)
            {
                return;
            }

            var audioSource = _containers.AudioSource;
            var clip = _gameHelper.Clips.Find(x => x.SoundTyp == feelingEvent.SoundType).Clip;
            if (clip != null)
            {
                audioSource.PlayOneShot(clip, 1);
            }
        }
    }
}