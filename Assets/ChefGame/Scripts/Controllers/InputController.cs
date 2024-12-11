using System.Linq;
using gs.chef.game.Scripts.Interfaces;
using gs.chef.game.Scripts.Item;
using gs.chef.game.Events;
using gs.chef.game.models;
using gs.chef.vcontainer.core.managers;
using Lean.Touch;
using MessagePipe;
using UnityEngine;
using VContainer;


namespace gs.chef.game.Controllers
{
    public class InputController : BaseSubscribable
    {
        [Inject] private readonly LeanSelectByFinger _selectByFinger;
        [Inject] private readonly GameModel _gameModel;
        [Inject] private readonly LevelModelController _levelModelController;


        #region Publusher

        [Inject] private readonly IPublisher<TapITableItemEvent> _tapITableItemEventPublisher;

        #endregion


        protected override void Init()
        {
            //LeanTouch.OnFingerTap += HandleFingerTap;
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

        public override void Dispose()
        {
        //    LeanTouch.OnFingerTap -= HandleFingerTap;
            LeanTouch.OnFingerDown -= HandleFingerDown;
        }

        protected override void Subscriptions()
        {
        }


        private void HandleFingerTap(LeanFinger finger)
        {
        }
    }
}