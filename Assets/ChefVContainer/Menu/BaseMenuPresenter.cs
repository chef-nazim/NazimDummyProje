using System;

namespace gs.chef.vcontainer.menu
{
    public class BaseMenuPresenter<TMenuName, TData, TMenu> : AbsMenuPresenter<TMenuName, TData, TMenu>
        where TMenuName : Enum
        where TData : IMenuData
        where TMenu : BaseMenuView

    {
        public override MenuMode MenuMode { get; }
        public override TMenuName MenuName { get; }
    }
}