using System;
using UnityEngine;

namespace gs.chef.vcontainer.menu
{
    public interface IMenuManager<TEnum> where TEnum : Enum
    {
        Transform MenuRoot { get; }
    }
}