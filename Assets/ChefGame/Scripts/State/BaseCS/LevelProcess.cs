using System;
using System.Threading;
using gs.chef.game.Scripts.State;
using gs.chef.game.enums;
using UnityEngine;
using VContainer;


namespace gs.chef.game.Objects
{
    public class LevelProcess
    {
        private ILevelProcessState _state;
        public CancellationTokenSource cancellationTokenSource;
        public event Action<LevelProcess> OnProcessCompleted;
        public event Action OnLevelComplete;
        public event Action<FailType> OnLevelFail;
        public IObjectResolver Resolver { get; set; }

        public ILevelProcessState State
        {
            get { return _state; }
            set
            {
                value.OnComplete += StateComplete;
                _state = value;
                _state.Handle(this);
            }
        }

        public LevelProcess(IObjectResolver resolver)
        {
            Resolver = resolver;
            cancellationTokenSource = new CancellationTokenSource();
        }

        private void StateComplete(ILevelProcessState obj)
        {
            obj.OnComplete -= StateComplete;


            if (obj.NextState == null)
            {
                if (obj is LevelCompleteState)
                {
                    //Debug .Log("Level Complete");
                    OnLevelComplete?.Invoke();
                }
                else if (obj is LevelFailState)
                {
                    //Debug.Log("Level Fail");
                    OnLevelFail?.Invoke(((LevelFailState)obj).FailType);
                }
                else
                {
                    //Debug.Log("Process Completed");
                    OnProcessCompleted?.Invoke(this);
                }
            }
            else
            {
                State = obj.NextState;
            }
        }
    }
}