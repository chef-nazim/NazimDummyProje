using System;
using Lean.Touch;
using MessagePipe;
using NCG.template.Events;
using NCG.template.extensions;
using NCG.template.models;
using NCG.template.Scripts.Interfaces;
using UnityEngine;
using VContainer;


namespace NCG.template.Controllers
{
    public class InputController  : Singleton<InputController>
    {
         private readonly LeanSelectByFinger _selectByFinger;
        private readonly GameModel _gameModel;
        private readonly LevelModelController _levelModelController;


        #region Publusher

        [Inject] private readonly IPublisher<TapITableItemEvent> _tapITableItemEventPublisher;

        #endregion


        protected override void Awake()
        {
            base.Awake();
            
        }

        private void OnEnable()
        {
            LeanTouch.OnFingerDown += HandleFingerDown;
        }

        private void HandleFingerDown(LeanFinger finger)
        {
            _selectByFinger.DeselectAll();
            _selectByFinger.SelectScreenPosition(finger);


            if (finger.IsOverGui) return;

            if (_levelModelController.LevelModel == null) return;

            if (_levelModelController.LevelModel.IsLevelRunning == false) return;

            if (_levelModelController.LevelModel.IsInputActive == false) return;


            if (_selectByFinger.Selectables.Count <= 0) return;

            else if (_selectByFinger.Selectables[0].GetComponent<ITapableItem>() is ITapableItem item)
            {
                _tapITableItemEventPublisher.Publish(new TapITableItemEvent(item));
            }
            else
            {
                Debug.Log("Not TapableItem");
            }
        }

        private void OnDisable()
        {
            //    LeanTouch.OnFingerTap -= HandleFingerTap;
            LeanTouch.OnFingerDown -= HandleFingerDown;
        }

        
    }
}