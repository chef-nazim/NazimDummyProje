using System;
using NCG.template._NCG.Core.Model;
using UnityEngine;

namespace NCG.template._NCG.Core.View
{
    public abstract class BaseView : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            
            
        }
        protected virtual void OnDisable()
        {
            Hide();
        }


        protected abstract void SubscribeEvents();


        protected abstract void UnSubscribeEvents();

        
        
        
        public abstract void Show(MenuData menuData);


        public abstract void Hide();
        
       
    }
}