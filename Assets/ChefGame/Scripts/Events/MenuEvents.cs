using gs.chef.game.enums;
using gs.chef.vcontainer.menu;

namespace gs.chef.game.Events
{
    public class OpenMenuEvent : BaseOpenMenuEvent<MenuNames, IMenuData>
    {
        public OpenMenuEvent(MenuNames menuName, IMenuData menuData) : base(menuName, menuData)
        {
            
        }
    }
    
    public class CloseMenuEvent : BaseCloseMenuEvent<MenuNames>
    {
        public CloseMenuEvent(MenuNames menuName) : base(menuName)
        {
        }
    }
    
    public class CloseOtherMenuEvent : BaseCloseOthersMenuEvent<MenuNames>
    {
        public MenuNames[] KeepMenuNames { get; private set; }
        
        public CloseOtherMenuEvent(params MenuNames[] keepMenuNames) : base(keepMenuNames)
        {
            KeepMenuNames = keepMenuNames;
        }
    }
}