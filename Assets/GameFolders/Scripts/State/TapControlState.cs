using NCG.template.Scripts.Item;
using NCG.template.models;
using NCG.template.Objects;
using NCG.template.Scripts.Interfaces;

namespace NCG.template.Scripts.State
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