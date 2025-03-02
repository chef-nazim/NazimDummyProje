using Lofelt.NiceVibrations;
using NCG.template.enums;
using NCG.template.EventBus;

namespace NCG.template._NCG.Core.AllEvents
{
    #region fix

    public struct CreateGamePlaySceneEvent : IEvent
    {
    }

    public struct LevelModelCreatEvent : IEvent
    {
    }

    public struct LevelModelCreatedEvent : IEvent
    {
    }

    public struct LevelCreatedEvent : IEvent
    {
    }

    public struct LevelDestroyEvent : IEvent
    {
    }

    public struct LevelCompleteEvent : IEvent
    {
    }

    public class LevelFailEvent : IEvent
    {
        public FailType FailType;

        public LevelFailEvent(FailType failType)
        {
            FailType = failType;
        }
    }

    public class RestartButtonClickEvent : IEvent
    {
        public RestartFrom From;

        public RestartButtonClickEvent(RestartFrom from)
        {
            From = from;
        }
    }

    public class FeelingEvent : IEvent 
    {
        public PlaySound SoundType = PlaySound.None;
        public HapticPatterns.PresetType HapticType = HapticPatterns.PresetType.None;
    }

    public struct CreatePoolsEvent : IEvent
    {
    }
    #endregion
}