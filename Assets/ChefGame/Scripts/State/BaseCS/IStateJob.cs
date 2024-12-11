using System;

namespace gs.chef.game.Objects
{
    public interface IStateJob
    {
        public event Action<IStateJob> OnComplete;
        public void Work();
    }
}   