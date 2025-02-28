using NCG.template.Controllers;
using NCG.template.enums;
using NCG.template.models;
using NCG.template.Objects;
using VContainer;

namespace NCG.template.Scripts.State
{
    public class LevelFailState : ProcessState
    {
        public FailType FailType;
        LevelModel levelModel => LevelModelController._instance.LevelModel;

        public LevelFailState(FailType failType)
        {
            FailType = failType;
        }

        public override void Handle(LevelProcess product)
        {
            levelModel.IsLevelRunning = false;


            WaitForComplete();
        }
    }
}