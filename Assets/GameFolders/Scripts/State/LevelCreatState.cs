using NCG.template.Controllers;
using NCG.template.Objects;
using NCG.template.Scripts.ScriptableObjects;
using VContainer;

namespace NCG.template.Scripts.State
{
    public class LevelCreatState : ProcessState
    {
        private LevelModelController _levelModelController => LevelModelController._instance;
       
        public async override void Handle(LevelProcess product)
        {
            
            
            WaitForComplete();
        }
    }
}