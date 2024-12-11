using System;
using gs.chef.vcontainer.core.managers;
using UnityEngine;
using VContainer;

namespace gs.chef.vcontainer.menu
{
    public abstract class AbsMenuPresenter<TMenuName, TData, TMenu> : BaseSubscribable, IMenuPresenter<TMenuName>
        where TData : IMenuData
        where TMenuName : Enum
        where TMenu : MonoBehaviour,IMenu
    {
        protected TMenu _view;
        
        [Inject] protected readonly Func<TMenu> _menuFactory;
        //[Inject] protected readonly IMenuManager<TMenuName> _menuManager;
        
        protected TData MenuData { get; private set; }

        public TMenu View => _view;
        
        public bool IsAlreadyOpened
        {
            get { return (_view != null && _view.ActiveSelf); }
        }

        public abstract MenuMode MenuMode { get; }
        public abstract TMenuName MenuName { get; }

        private void SetView(TMenu view)
        {
            _view = view;
        }

        public TMenu1 GetView<TMenu1>() where TMenu1 : MonoBehaviour, IMenu
        {
            return View as TMenu1;
        }

        protected TMenu GetMenu(TData menuData = null)
        {
            if (_view == null)
            {
                var view = _menuFactory.Invoke();
                view.InitializeView();
                SetView(view);
                view.SetActive(false);
                MenuData = menuData;
                OnShow(menuData);
            }
            else
            {
                _view.UpdateView();
                MenuData = menuData;
                UpdatePresenter();
                OnShow(menuData);
            }

            return _view;
        }
        
        protected virtual void UpdatePresenter()
        {
        }

        public void Open<TData1>(TData1 data) where TData1 : IMenuData
        {
            Open(data as TData);
        }

        

        public void Open(TData data = null)
        {
            GetMenu(data);
            _view?.Open();
        }

        protected virtual void OnShow(TData menuData)
        {
        }


        public void Close()
        {
            OnHide();
            if (_view != null)
            {
                _view.Close();
                if (_view.DestroyWhenClosed)
                {
                    DestroyView();
                }
            }
                
        }

        protected virtual void OnHide()
        {
        }
        

        public void DestroyView()
        {
            if (_view != null)
            {
                OnHide();
                _view.Dispose(true);
                _view = null;
            }
        }
    }
}