using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace gs.chef.game.Objects
{
    public abstract class ProcessState : ILevelProcessState
    {
        public virtual ILevelProcessState NextState { get; set; }
        public event Action<ILevelProcessState> OnComplete;
        protected List<IStateJob> SubJobList = new List<IStateJob>();
        public abstract void Handle(LevelProcess product);

        protected virtual void AddJob(IStateJob job)
        {
            job.OnComplete += RemoveJob;
            SubJobList.Add(job);
        }

        protected virtual void RemoveJob(IStateJob job)
        {
            job.OnComplete -= RemoveJob;
            SubJobList.Remove(job);
        }

        protected async UniTaskVoid WaitForComplete()
        {
            if (SubJobList.Count != 0)
            {
                await UniTask.WaitUntilValueChanged(SubJobList, x => x.Count == 0);
            }
   
            OnComplete?.Invoke(this);
            
            
            
        }
    }
}