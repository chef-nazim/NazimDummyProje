using System;
using System.Collections.Generic;
using UnityEngine;

namespace gs.chef.vcontainer.menu
{
    public class BaseMenuPrefabList<TMenuName> : ScriptableObject where TMenuName : System.Enum
    {
        public List<BaseMenuList<TMenuName>> MenuList;
    }

    [Serializable]
    public class BaseMenuList<TMenuName> where TMenuName : Enum
    {
        public TMenuName MenuName;
        public BaseMenuView Prefab;
    }
}