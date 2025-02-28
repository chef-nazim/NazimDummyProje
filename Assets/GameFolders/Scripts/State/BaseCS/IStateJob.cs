using System;

namespace NCG.template.Objects
{
    public interface IStateJob
    {
        public event Action<IStateJob> OnComplete;
        public void Work();
    }
}   