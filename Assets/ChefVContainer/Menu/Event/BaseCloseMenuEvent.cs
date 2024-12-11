using System;

namespace gs.chef.vcontainer.menu
{
    public class BaseCloseMenuEvent<TMenuName> where TMenuName : Enum
    {
        public TMenuName MenuName { get; }

        public BaseCloseMenuEvent(TMenuName menuName)
        {
            MenuName = menuName;
        }
    }
}