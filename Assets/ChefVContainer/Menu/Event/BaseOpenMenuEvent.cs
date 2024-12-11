using System;

namespace gs.chef.vcontainer.menu
{
    public class BaseOpenMenuEvent<TMenuName, TMenuData> where TMenuName : Enum where TMenuData : IMenuData
    {
        public TMenuName MenuName { get; }
        public TMenuData MenuData { get; }

        public BaseOpenMenuEvent(TMenuName menuName, TMenuData menuData)
        {
            MenuName = menuName;
            MenuData = menuData;
        }
    }
}