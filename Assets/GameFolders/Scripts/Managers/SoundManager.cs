using NCG.template._NCG.Core.AllEvents;
using NCG.template._NCG.Core.BaseClass;
using NCG.template.enums;
using NCG.template.EventBus;
using NCG.template.GameFolders.Scripts.AppInitializier;
using NCG.template.models;
using NCG.template.Scripts.Others;
using NCG.template.Scripts.ScriptableObjects;

namespace NCG.template.Managers
{
    public class SoundManager : BaseManager
    {
        
        private GameModel _gameModel =>GameModel.Instance;
        private Containers _containers => Containers.instance;
        private GameHelper _gameHelper => Containers.instance.GameHelper;

        
        public override void Initialize()
        {
            EventBus<FeelingEvent>.Subscribe(OnFeelingEvent);
        }

        public override void Dispose()
        {
            EventBus<FeelingEvent>.Unsubscribe(OnFeelingEvent);
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