using NCG.template.Controllers;
using NCG.template.models;
using NCG.template.Objects;
using VContainer;

namespace NCG.template.Scripts.State
{
    public class LevelCompleteState : ProcessState
    {
        LevelModel levelModel => LevelModelController._instance.LevelModel;

        public override void Handle(LevelProcess product)
        {
            
            levelModel.IsLevelRunning = false;
            WaitForComplete();
        }
    }
}