using System;

namespace gs.chef.vcontainer.menu
{
    public class BaseCloseOthersMenuEvent<TMenuName> where TMenuName : Enum
    {
        public BaseCloseOthersMenuEvent(params TMenuName[] keepMenuNames)
        {
        }
    }
}