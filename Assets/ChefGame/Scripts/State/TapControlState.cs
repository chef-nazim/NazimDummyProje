using gs.chef.game.models;
using gs.chef.game.Objects;
using gs.chef.game.Scripts.Interfaces;
using gs.chef.game.Scripts.Item;

namespace gs.chef.game.Scripts.State
{
    public class TapControlState : ProcessState
    {
        LevelModel levelModel;
        ITapableItem tapableItem;
        
        public TapControlState(ITapableItem tapableItem)
        {
            this.tapableItem = tapableItem;
        }
        
        public override void Handle(LevelProcess product)
        {

           

            WaitForComplete();
        }
    }
}