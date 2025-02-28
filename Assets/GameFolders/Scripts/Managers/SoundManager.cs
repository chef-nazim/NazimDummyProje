using NCG.template._NCG.Core.AllEvents;
using NCG.template.enums;
using NCG.template.EventBus;
using NCG.template.models;
using NCG.template.Scripts.Others;
using NCG.template.Scripts.ScriptableObjects;

namespace NCG.template.Managers
{
    public class SoundManager 
    {
        
        private GameModel _gameModel;
        private Containers _containers => Containers.instance;
        private GameHelper _gameHelper; // todo bi sekilde buraya getir

        EventBinding<FeelingEvent> _feelingEventSubscriber;
        
        public SoundManager()
        {
            _gameModel = GameModel.Instance;
            Subscriptions();
        }
        protected  void Subscriptions()
        {
            _feelingEventSubscriber = new EventBinding<FeelingEvent>(OnFeelingEvent);
            EventBus<FeelingEvent>.Subscribe(_feelingEventSubscriber);
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