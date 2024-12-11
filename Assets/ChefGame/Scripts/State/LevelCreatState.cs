using gs.chef.game.Controllers;
using gs.chef.game.Objects;
using gs.chef.game.Scripts.ScriptableObjects;
using VContainer;

namespace gs.chef.game.Scripts.State
{
    public class LevelCreatState : ProcessState
    {
        IObjectResolver _resolver;
       
        public async override void Handle(LevelProcess product)
        {
            _resolver = product.Resolver;
            var levelmodel = _resolver.Resolve<LevelModelController>().LevelModel;
            var gameHelper = _resolver.Resolve<GameHelper>();
            
           
            
         
            
            
            WaitForComplete();
        }
    }
}