using System;

namespace gs.chef.game.Objects
{
    public interface ILevelProcessState
    {
        public ILevelProcessState NextState { get; set; }
        public event Action<ILevelProcessState> OnComplete;
        public void Handle(LevelProcess product);
        
    }
}