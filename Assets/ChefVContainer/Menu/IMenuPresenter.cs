using System;
using UnityEngine;

namespace gs.chef.vcontainer.menu
{
    public interface IMenuPresenter<TMenuName> where TMenuName : Enum
    {
        TMenuName MenuName { get; }
        MenuMode MenuMode { get; }
        void Close();
        void DestroyView();

        bool IsAlreadyOpened { get; }

        void Open<TData>(TData data) where TData : IMenuData;

        TMenu GetView<TMenu>() where TMenu : MonoBehaviour, IMenu;
    }
}