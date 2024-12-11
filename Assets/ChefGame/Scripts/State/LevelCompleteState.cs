using gs.chef.game.Controllers;
using gs.chef.game.models;
using gs.chef.game.Objects;

using VContainer;

namespace gs.chef.game.Scripts.State
{
    public class LevelCompleteState : ProcessState
    {
        LevelModel levelModel;

        public override void Handle(LevelProcess product)
        {
            levelModel = product.Resolver.Resolve<LevelModelController>().LevelModel;
            
            
            levelModel.IsLevelRunning = false;
            WaitForComplete();
        }
    }
}