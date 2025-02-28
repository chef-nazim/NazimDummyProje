using NCG.template._NCG.Core.Model;
using NCG.template.enums;
using NCG.template.EventBus;


namespace NCG.template.Events
{
    public class OpenMenuEvent : IEvent
    {
        public MenuNames _MenuName;
        public MenuData _MenuData;
        public OpenMenuEvent(MenuNames menuName, MenuData menuData)
        {
            _MenuName = menuName;
            _MenuData = menuData;
        }
    }
    
    public class CloseMenuEvent  : IEvent
    {
        public MenuNames _MenuName;
        public CloseMenuEvent(MenuNames menuName)
        {
            _MenuName = menuName;
        } 
    }
    
    public class CloseOtherMenuEvent  : IEvent
    {
        public MenuNames[] KeepMenuNames { get; private set; }
        
        public CloseOtherMenuEvent(params MenuNames[] keepMenuNames) 
        {
            KeepMenuNames = keepMenuNames;
        }
    }
}