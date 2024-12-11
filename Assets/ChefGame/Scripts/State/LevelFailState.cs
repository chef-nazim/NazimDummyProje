using gs.chef.game.Controllers;
using gs.chef.game.enums;
using gs.chef.game.Objects;
using VContainer;

namespace gs.chef.game.Scripts.State
{
    public class LevelFailState : ProcessState
    {
        public FailType FailType;
        LevelModelController _levelModelController;
        
        public LevelFailState( FailType failType)
        {
            FailType = failType;
        }
        
        public override void Handle(LevelProcess product)
        {
            
            _levelModelController = product.Resolver.Resolve<LevelModelController>();
            
            _levelModelController.LevelModel.IsLevelRunning = false;
            
            
            WaitForComplete();
        }
    }
     
}