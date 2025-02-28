using System;

namespace NCG.template.Objects
{
    public interface ILevelProcessState
    {
        public ILevelProcessState NextState { get; set; }
        public event Action<ILevelProcessState> OnComplete;
        public void Handle(LevelProcess product);
        
    }
}