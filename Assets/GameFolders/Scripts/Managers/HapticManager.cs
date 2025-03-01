using System;
using Cysharp.Threading.Tasks;
using Lofelt.NiceVibrations;
using NCG.template._NCG.Core.AllEvents;
using NCG.template._NCG.Core.BaseClass;
using NCG.template.EventBus;
using NCG.template.models;
using UnityEngine;

namespace NCG.template.Managers
{
    public class HapticManager : BaseManager
    {
        private GameModel _gameModel => GameModel.Instance;

        public override async void Initialize()
        {
            EventBus<FeelingEvent>.Subscribe(OnFeelingEvent);
        }

        public override void Dispose()
        {
            EventBus<FeelingEvent>.Unsubscribe(OnFeelingEvent);
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